using Domain.Data;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DeliveryAssignmentService : IDeliveryAssignment
    {
        private readonly OpenRouteServiceClient openRouteServiceClient;
        private readonly AppDbContext appDbContext;

        public DeliveryAssignmentService(OpenRouteServiceClient openRouteServiceClient, AppDbContext appDbContext)
        {
            this.openRouteServiceClient = openRouteServiceClient;
            this.appDbContext = appDbContext;
        }

        public async Task<Delivery> AssignOrderAsync(Order order, Restaurant restaurant, User customer, List<DeliveryAgent> availableDeliveryAgents)
        {
            if (availableDeliveryAgents == null || availableDeliveryAgents.Count == 0)
                throw new Exception("No delivery agent available");

            // Load restaurant with user and address
            var restaurantEntity = await appDbContext.Restaurants
                .Include(r => r.User)
                .ThenInclude(u => u.Addresses)
                .FirstOrDefaultAsync(r => r.RestaurantId == order.RestaurantId);

            // Load customer with address
            var customerEntity = await appDbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == order.UserId);

            var restaurantAddress = restaurantEntity?.User?.Addresses.FirstOrDefault()?.Address1;
            var customerAddress = customerEntity?.Addresses.FirstOrDefault()?.Address1;

            if (string.IsNullOrWhiteSpace(restaurantAddress) || string.IsNullOrWhiteSpace(customerAddress))
                throw new Exception("Missing address information");

            var (restaurantLat, restaurantLng) = await openRouteServiceClient.GeocodeAddressAsync(restaurantAddress);
            var (customerLat, customerLng) = await openRouteServiceClient.GeocodeAddressAsync(customerAddress);
            var routeToCustomer = await openRouteServiceClient.GetRouteAsync(restaurantLat, restaurantLng, customerLat, customerLng);

            var agentDistances = new List<(DeliveryAgent Agent, double Distance, double Duration)>();

            foreach (var agent in availableDeliveryAgents)
            {
                var agentAddress = agent.Agent?.Addresses?.FirstOrDefault()?.Address1;
                if (string.IsNullOrWhiteSpace(agentAddress))
                    continue;

                var (agentLat, agentLng) = await openRouteServiceClient.GeocodeAddressAsync(agentAddress);
                var routeToRestaurant = await openRouteServiceClient.GetRouteAsync(agentLat, agentLng, restaurantLat, restaurantLng);

                double totalDuration = routeToRestaurant.Duration + routeToCustomer.Duration;
                double totalDistance = routeToRestaurant.Distance + routeToCustomer.Distance;

                agentDistances.Add((agent, totalDistance, totalDuration));
            }

            if (!agentDistances.Any())
                throw new Exception("No valid delivery agent with address found");

            var bestAgent = agentDistances.OrderBy(x => x.Duration).First().Agent;

            var delivery = new Delivery
            {
                OrderId = order.OrderId,
                AgentId = bestAgent.AgentId,
                Status = true
            };

            appDbContext.Deliveries.Add(delivery);
            await appDbContext.SaveChangesAsync();

            return delivery;
        }
    }
}
