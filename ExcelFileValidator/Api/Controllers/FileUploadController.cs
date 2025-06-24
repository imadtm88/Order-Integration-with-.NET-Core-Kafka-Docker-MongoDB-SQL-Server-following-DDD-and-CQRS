using Microsoft.AspNetCore.Mvc;
using Application.Communication;
using Infrastructure.UploadExcelFileInfra.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly OrderHttpSender _orderHttpSender;

        public FileUploadController(IFileUploadService fileUploadService, OrderHttpSender orderHttpSender)
        {
            _fileUploadService = fileUploadService;
            _orderHttpSender = orderHttpSender;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file provided." });
            }

            try
            {
                // Appel de `UploadFileAsync` dans `IFileUploadService`
                await _fileUploadService.UploadFileAsync(file);

                return Ok(new { message = "File uploaded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderHttpSender.GetAllOrdersAsync();
            if (orders != null && orders.Count > 0)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound(new { message = "No orders found." });
            }
        }

        [HttpGet("By")]
        public async Task<IActionResult> GetAllOrders([FromRoute] string searchBy, string query)
        {
            var orders = await _orderHttpSender.GetAllOrdersAsync();

            if (orders == null || orders.Count == 0)
            {
                return NotFound(new { message = "No orders found." });
            }

            // Handle null or empty query
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest(new { message = "Query parameter is required." });
            }

            // Normalize searchBy parameter to lower case
            searchBy = searchBy?.Trim().ToLower();

            // Filter the orders based on the search criteria
            switch (searchBy)
            {
                case "lastname":
                    orders = orders.Where(o => !string.IsNullOrEmpty(o.LastName) && o.LastName.ToLower().Contains(query.ToLower())).ToList();
                    break;
                case "firstname":
                    orders = orders.Where(o => !string.IsNullOrEmpty(o.FirstName) && o.FirstName.ToLower().Contains(query.ToLower())).ToList();
                    break;
                case "productid":
                    orders = orders.Where(o => !string.IsNullOrEmpty(o.ProductId) && o.ProductId.ToLower().Contains(query.ToLower())).ToList();
                    break;
                case "orderstatus":
                    orders = orders.Where(o => !string.IsNullOrEmpty(o.OrderStatus) && o.OrderStatus.ToLower().Contains(query.ToLower())).ToList();
                    break;
                default:
                    return BadRequest(new { message = "Invalid search criteria." });
            }

            return Ok(orders);
        }



    }
}
