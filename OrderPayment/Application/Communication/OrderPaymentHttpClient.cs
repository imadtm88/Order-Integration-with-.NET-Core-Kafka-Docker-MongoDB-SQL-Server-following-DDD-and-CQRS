using Infrastructure.Interfaces;
using Infrastructure.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Communication
{
    public class OrderPaymentHttpClient : IOrderPaymentHttp
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderPaymentHttpClient> _logger;

        public OrderPaymentHttpClient(HttpClient httpClient, ILogger<OrderPaymentHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<OrderModel?> GetOrderAsync(string Id)
        {
            _logger.LogInformation($"Fetching order with Id: {Id}");
            var response = await _httpClient.GetAsync($"api/Order/{Id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<OrderModel>(content);
                return order;
            }

            _logger.LogError($"Failed to fetch order with Id: {Id}. Status Code: {response.StatusCode}");
            return null;
        }
    }
}