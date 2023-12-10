using OrderMediator.Models;

namespace OrderMediator.Services
{
    public class PriceResolver : IPriceResolver
    {
        public bool ResolveFinalPrices(OrderModel orderModel, Dictionary<string, decimal?> dbPrices)
        {
            var mismatch = false;
            foreach (var article in orderModel.OrderDetails)
            {
                var currentDbPrice = dbPrices[article.EANArticle!];
                if (currentDbPrice != article.UnitPrice)
                {
                    mismatch = true;
                    article.UnitPrice = currentDbPrice;
                }
            }

            return mismatch;
        }
    }
}
