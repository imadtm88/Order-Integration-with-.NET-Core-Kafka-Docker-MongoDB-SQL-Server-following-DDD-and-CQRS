using Application.Communication;
using Domain.Entity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderIntegrationController : ControllerBase
    {
        private readonly OrderIntegrationHttp _orderIntegrationHttp;
        private readonly IOrderRepository _orderRepository;

        public OrderIntegrationController(OrderIntegrationHttp orderIntegrationHttp, IOrderRepository orderRepository)
        {
            _orderIntegrationHttp = orderIntegrationHttp;
            _orderRepository = orderRepository;
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder(string id)
        {
            var order = await _orderIntegrationHttp.GetOrderAsync(id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound(new { message = $"Order with ID {id} not found." });
            }
        }

        [HttpPost("order")]
        public async Task<IActionResult> SaveOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest(new { message = "Invalid order data." });
            }

            try
            {
                await _orderRepository.SaveOrderAsync(order);
                return Ok(new { message = "Order saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while saving the order: {ex.Message}" });
            }
        }
    }
}
