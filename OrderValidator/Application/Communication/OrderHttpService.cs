using Infrastructure.Interfaces;
using Infrastructure.Model;
using Newtonsoft.Json;

namespace Application.Communication
{
    public class OrderHttpService : IOrderService
    {
        private readonly HttpClient _httpClient;
        public OrderHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5187");
        }

        public async Task<Order> GetOrdersAsync(string Id)
        {
            var response = await _httpClient.GetAsync($"api/Order/{Id}");
            if (response.IsSuccessStatusCode)
            {
                // Convertissez la réponse JSON en une chaîne
                var content = await response.Content.ReadAsStringAsync();

                // Désérialisez la chaîne JSON en objet Order
                var order = JsonConvert.DeserializeObject<Order>(content);

                return order;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve order with ID {Id}. Status code: {response.StatusCode}");
            }
        }
    }
}