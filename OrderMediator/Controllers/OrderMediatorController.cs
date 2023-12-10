using Microsoft.AspNetCore.Mvc;
using OrderMediator.Models;
using OrderMediator.Services;

namespace OrderMediator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderMediatorController : ControllerBase
    {
        private readonly ILogger<OrderMediatorController> _logger;
        private readonly IOrderManager orderService;

        public OrderMediatorController(ILogger<OrderMediatorController> logger,
            IOrderManager orderService)
        {
            _logger = logger;
            this.orderService = orderService;
        }

        [HttpGet("GetOrderFile")]
        public async Task<IActionResult> GetOrderFile(IFormFile orderFile)
        {
            var orderResult = await this.orderService.SendOrderAsync(orderFile);

            if (!string.IsNullOrWhiteSpace(orderResult.ErrorMessage))
            {
                return BadRequest(orderResult);
            }

            return Ok(orderResult);
        }
    }
}