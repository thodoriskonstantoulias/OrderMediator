using OrderMediator.Models;

namespace OrderMediator.Services
{
    public interface IPriceResolver
    {
        bool ResolveFinalPrices(OrderModel orderModel, Dictionary<string, decimal?> dbPrices);
    }
}
