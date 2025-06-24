using Application.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderValidatorController : ControllerBase
    {
        private readonly OrderHttpService _orderHttpService;

        public OrderValidatorController(OrderHttpService orderHttpService)
        {
            _orderHttpService = orderHttpService;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrder(string Id)
        {
            var order = await _orderHttpService.GetOrdersAsync(Id);

            // Stocker l'objet dans une variable locale
            var myOrder = order;

            return Ok(order);
        }

    }
}