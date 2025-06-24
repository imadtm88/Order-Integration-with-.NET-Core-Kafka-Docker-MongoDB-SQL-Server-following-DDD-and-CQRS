using Application.DTO;
using Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Communication
{
    public class OrderHttpSender
    {
        private readonly HttpClient _httpClient;

        public OrderHttpSender(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task SendOrdersAsync(List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                throw new ArgumentNullException(nameof(orders));
            }

            try
            {
                // Convertir la liste d'objets Order en JSON
                string jsonOrders = JsonSerializer.Serialize(orders);

                // Créer le contenu de la demande
                var content = new StringContent(jsonOrders, Encoding.UTF8, "application/json");

                // Envoyer la demande HTTP POST
                HttpResponseMessage response = await _httpClient.PostAsync("/api/Order", content);

                // Vérifier si la demande a réussi
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'envoi des commandes : {ex.Message}");
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/Order");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(content);
                return orders;
            }
            return new List<OrderDTO>();
        }
    }
}