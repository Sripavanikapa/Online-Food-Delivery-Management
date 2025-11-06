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
        private readonly ISmsService smsService;

        public DeliveryAssignmentService(OpenRouteServiceClient openRouteServiceClient, AppDbContext appDbContext,ISmsService smsService)
        {
            this.openRouteServiceClient = openRouteServiceClient;
            this.appDbContext = appDbContext;
            this.smsService = smsService;
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
            var validAgents = availableDeliveryAgents
          .Where(a=> a.Agent.Role == "deliveryagent" )
        .ToList();
            var agentDistances = new List<(DeliveryAgent Agent, double Distance, double Duration)>();

            foreach (var agent in validAgents)
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
            var agentUser = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == bestAgent.AgentId);
            if (agentUser != null && !string.IsNullOrWhiteSpace(agentUser.Phoneno))
            {
                string message = $"Hi {agentUser.Name}, you have been assigned a new delivery order (Order ID: {order.OrderId}). Please check your dashboard.";
                await smsService.SendSmsAsync(agentUser.Phoneno, message);
            }


            return delivery;
        }
    }
}
