using OrderMediator.Models;

namespace OrderMediator.Services
{
    public interface IOrderManagementSystemService
    {
        Task<OrderManagementSystemResponse?> SendOrderToManagementSystemAsync(OrderModel orderModel);
    }
}
