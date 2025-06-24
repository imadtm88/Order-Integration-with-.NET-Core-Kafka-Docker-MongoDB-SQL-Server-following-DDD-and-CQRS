using Application.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpportController : ControllerBase
    {
        private readonly ReportHttp _reportHttp;
        public RpportController(ReportHttp reportHttp)
        {
            _reportHttp = reportHttp;
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "The Id field is required." });
            }

            var order = await _reportHttp.GetOrderAsync(id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound(new { message = $"Order with ID {id} not found." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _reportHttp.GetAllOrdersAsync();
            if (orders != null && orders.Count > 0)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound(new { message = "No orders found." });
            }
        }
    }
}