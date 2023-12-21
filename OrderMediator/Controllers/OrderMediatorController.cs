using Microsoft.AspNetCore.Mvc;
using OrderMediator.Models;
using OrderMediator.Services;

namespace OrderMediator.Controllers
{
    // Unit testing missing from feedback also
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
            //Based on feedback we do not need to get file as parameter but get it like below

            //var file = Request.Form.Files.FirstOrDefault();
            //using (var reader = new StreamReader(file.OpenReadStream()))
            //{
            //    var str = reader.ReadToEnd();
            //    return Ok();
            //}

            var orderResult = await this.orderService.SendOrderAsync(orderFile);

            if (!string.IsNullOrWhiteSpace(orderResult.ErrorMessage))
            {
                return BadRequest(orderResult);
            }

            return Ok(orderResult);
        }
    }
}