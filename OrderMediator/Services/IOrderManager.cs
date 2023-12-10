using OrderMediator.Models;

namespace OrderMediator.Services
{
    public interface IOrderManager
    {
        Task<OrderMediatorResult> SendOrderAsync(IFormFile file);
    }
}
