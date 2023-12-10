using Microsoft.EntityFrameworkCore;
using OrderMediator.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Services
{
    public class PriceService : IPriceService
    {
        private readonly ApplicationDbContext dbContext;

        public PriceService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Dictionary<string, decimal>> CheckAndResolvePricesMismatchAsync(IEnumerable<string> articleCodes, string buyer)
        {
            var priceList = await this.dbContext.PriceLists!.FirstOrDefaultAsync(x => x.Code == buyer || x.Code == "Default");
            if (priceList == null)
            {
                return new Dictionary<string, decimal>();
            }

            return await this.dbContext.ArticlePrices!
                .Where(x => articleCodes.Contains(x.ArticleCode) && x.PriceListID == priceList.ID)
                .ToDictionaryAsync(x => x.ArticleCode!, y => y.Price!.Value);
        }
    }
}
