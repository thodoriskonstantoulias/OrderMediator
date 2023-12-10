using OrderMediator.Models;

namespace OrderMediator.Services
{
    public interface IOrderService
    {
        Task<OrderMediatorResult> SendOrderAsync(IFormFile file);
    }
}
