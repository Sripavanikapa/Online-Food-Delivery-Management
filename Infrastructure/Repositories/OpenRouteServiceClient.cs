using Domain.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OpenRouteServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenRouteServiceConfig _config;

        public OpenRouteServiceClient(HttpClient httpClient, OpenRouteServiceConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // Method to get ETA between two points (lat/lon)
        public async Task<(double Distance, double Duration)> GetRouteAsync(double startLat, double startLng, double endLat, double endLng)
        {
            var url = $"https://api.openrouteservice.org/v2/directions/driving-car?api_key={_config.ApiKey}&start={startLng},{startLat}&end={endLng},{endLat}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var features = doc.RootElement.GetProperty("features")[0];
            var summary = features.GetProperty("properties").GetProperty("summary");

            double distance = summary.GetProperty("distance").GetDouble(); // in meters
            double duration = summary.GetProperty("duration").GetDouble(); // in seconds

            return (distance, duration);
        }

        public async Task<(double Lat, double Lng)> GeocodeAddressAsync(string address)
        {
            var url = $"https://api.openrouteservice.org/geocode/search?api_key={_config.ApiKey}&text={Uri.EscapeDataString(address)}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var features = doc.RootElement.GetProperty("features");
            if (features.GetArrayLength() == 0)
                throw new Exception($"No results found for address: {address}");

            var coords = features[0].GetProperty("geometry").GetProperty("coordinates");
            double lng = coords[0].GetDouble();
            double lat = coords[1].GetDouble();

            return (lat, lng);
        }
    }
}
