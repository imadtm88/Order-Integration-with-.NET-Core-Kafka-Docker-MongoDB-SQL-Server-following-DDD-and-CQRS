using Application.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderPaymentController : ControllerBase
    {
        private readonly OrderPaymentHttpClient _orderPaymentHttpClient;

        public OrderPaymentController(OrderPaymentHttpClient orderPaymentHttpClient)
        {
            _orderPaymentHttpClient = orderPaymentHttpClient;
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder(string Id)
        {
            var order = await _orderPaymentHttpClient.GetOrderAsync(Id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound(new { message = $"Order with ID {Id} not found." });
            }
        }
    }
}