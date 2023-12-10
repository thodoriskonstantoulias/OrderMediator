using OrderMediator.Models;

namespace OrderMediator.Services
{
    public interface IERPService
    {
        Task<ErpResponse?> CheckStockAvailabilityAsync(OrderModel orderModel);
        Task<ErpUpdateResponse?> UpdateStockAsync(OrderModel orderModel);
    }
}
