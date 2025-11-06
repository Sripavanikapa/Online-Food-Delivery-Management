using Domain.Models;
using Microsoft.Extensions.Options;
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

        

        public OpenRouteServiceClient(IOptions<OpenRouteServiceConfig> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;
            
        }
        // Method to get ETA between two points (lat/lon)



     

public async Task<(double Distance, double Duration)> GetRouteAsync(
    double startLat, double startLng, double endLat, double endLng)
        {
            var url = $"https://api.openrouteservice.org/v2/directions/driving-car" +
                      $"?api_key={_config.ApiKey}&start={startLng},{startLat}&end={endLng},{endLat}";

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Route API failed: {response.StatusCode} - {content}");

            using var doc = JsonDocument.Parse(content);

            
            if (doc.RootElement.TryGetProperty("routes", out var routes) && routes.GetArrayLength() > 0)
            {
                var summary = routes[0].GetProperty("summary");
                double distance = summary.GetProperty("distance").GetDouble();
                double duration = summary.GetProperty("duration").GetDouble();
                return (distance, duration);
            }

           
            if (doc.RootElement.TryGetProperty("features", out var features) && features.GetArrayLength() > 0)
            {
                var props = features[0].GetProperty("properties");
                if (props.TryGetProperty("segments", out var segments) && segments.GetArrayLength() > 0)
                {
                    var segment = segments[0];
                    double distance = segment.TryGetProperty("distance", out var d) ? d.GetDouble() : 0.0;
                    double duration = segment.TryGetProperty("duration", out var t) ? t.GetDouble() : 0.0;
                    return (distance, duration);
                }
            }

            throw new Exception($"Route data missing or malformed. Response: {content}");
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
