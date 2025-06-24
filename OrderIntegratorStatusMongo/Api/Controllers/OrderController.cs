using Application.Command.Services;
using Domain.Entities;
using Domain.Model.Enum;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServiceApplication _orderService;
        private readonly IOrderRepository _orderRepository;

        public OrderController(OrderServiceApplication orderService, IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrders([FromBody] List<Order> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return BadRequest(new { message = "No orders provided." });
            }

            foreach (var order in orders)
            {
                order.CreationDate = order.CreationDate.ToUniversalTime();
            }

            await _orderService.CreateOrdersAsync(orders);

            return Ok(new { message = "Orders created successfully." });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrder([FromRoute] string Id)
        {
            var order = await _orderService.GetOrderAsync(Id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] string orderId, [FromBody] string status)
        {
            if (!Enum.TryParse(status, out OrderStatus newStatus))
            {
                return BadRequest(new { message = "Invalid status value." });
            }

            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
                return Ok(new { message = $"Order status updated to {newStatus}." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
