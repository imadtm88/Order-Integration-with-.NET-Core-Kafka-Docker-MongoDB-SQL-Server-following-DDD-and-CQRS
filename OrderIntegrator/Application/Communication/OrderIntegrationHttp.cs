using Domain.Entity;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Communication
{
    public class OrderIntegrationHttp : IOrderIntegrationHttp
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderIntegrationHttp> _logger;

        public OrderIntegrationHttp(HttpClient httpClient, ILogger<OrderIntegrationHttp> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<Order?> GetOrderAsync(string Id)
        {
            _logger.LogInformation($"Fetching order with Id: {Id}");
            var response = await _httpClient.GetAsync($"api/Order/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);
                return order;
            }

            _logger.LogError($"Failed to fetch order with Id: {Id}. Status Code: {response.StatusCode}");
            return null;
        }
    }
}
