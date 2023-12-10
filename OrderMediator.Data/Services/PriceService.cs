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

        public async Task<Dictionary<string, decimal?>> CheckAndResolvePricesMismatchAsync(IEnumerable<string> articleCodes, string buyer)
        {
            var dict = new Dictionary<string, decimal?>();
            var defaultPricelist = await this.dbContext.PriceLists!.FirstOrDefaultAsync(x => x.Code == "Default");
            var buyerPriceList = await this.dbContext.PriceLists!.FirstOrDefaultAsync(x => x.Code == buyer);
            if (defaultPricelist == null && buyerPriceList == null)
            {
                return dict;
            }

            var articles = await this.dbContext.ArticlePrices!
                .Where(x => articleCodes.Contains(x.ArticleCode))
                .ToListAsync();

            foreach (var article in articleCodes)
            {
                var price = articles.FirstOrDefault(x => x.ArticleCode == article && x.PriceListID == buyerPriceList?.ID)?.Price ??
                    articles.FirstOrDefault(x => x.ArticleCode == article && x.PriceListID == defaultPricelist?.ID)?.Price;
                dict.Add(article, price);
            }

            return dict;
        }
    }
}
