using Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Communication
{
    public class ReportHttp 
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReportHttp> _logger;

        public ReportHttp(HttpClient httpClient, ILogger<ReportHttp> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<Order?> GetOrderAsync(string Id)
        {
            try
            {
                _logger.LogInformation($"Fetching order with Id: {Id}");
                var response = await _httpClient.GetAsync($"api/Order/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var order = JsonConvert.DeserializeObject<Order>(content);
                    return order;
                }
                else
                {
                    _logger.LogError($"Failed to fetch order with Id: {Id}. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while fetching order with Id: {Id}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders");
            var response = await _httpClient.GetAsync("api/Order");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(content);
                return orders;
            }

            _logger.LogError($"Failed to fetch orders. Status Code: {response.StatusCode}");
            return new List<Order>();
        }

    }
}