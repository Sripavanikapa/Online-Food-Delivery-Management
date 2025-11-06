using Domain.Data;
using Domain.DTO;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TrackingApiClient:ITrackingApiClient
    {
        private readonly HttpClient httpClient;
        private readonly AppDbContext appDbContext;
        private readonly OpenRouteServiceClient openRouteServiceClient;

        public TrackingApiClient(HttpClient httpClient,AppDbContext appDbContext,OpenRouteServiceClient openRouteServiceClient)
        {
            this.httpClient = httpClient;
            this.appDbContext = appDbContext;
            this.openRouteServiceClient = openRouteServiceClient;
        }

        public async Task<TrackingStatusDto> TrackDeliveryByOrderIdAsync(int orderId)
        {
          
            var delivery = await appDbContext.Deliveries
                .FirstOrDefaultAsync(d => d.OrderId == orderId);

            if (delivery == null)
                throw new Exception("No delivery assigned for this order");

            var agentId = delivery.AgentId;

           
            var agentUser = await appDbContext.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == agentId);

            var agentAddress = agentUser?.Addresses.FirstOrDefault()?.Address1;
            if (string.IsNullOrWhiteSpace(agentAddress))
                throw new Exception("Assigned agent has no address");

            
            var order = await appDbContext.Orders
                .Include(o => o.User)
                .ThenInclude(u => u.Addresses)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            var customerAddress = order?.User?.Addresses.FirstOrDefault()?.Address1;
            if (string.IsNullOrWhiteSpace(customerAddress))
                throw new Exception("Customer address missing");

            
            var (agentLat, agentLng) = await openRouteServiceClient.GeocodeAddressAsync(agentAddress);
            var (customerLat, customerLng) = await openRouteServiceClient.GeocodeAddressAsync(customerAddress);

           
            var route = await openRouteServiceClient.GetRouteAsync(agentLat, agentLng, customerLat, customerLng);

           
            return new TrackingStatusDto
            {
                Status = "In Transit",
                ETA = $"{Math.Round(route.Duration / 60)} mins",
                
            };
        }

    }
}
