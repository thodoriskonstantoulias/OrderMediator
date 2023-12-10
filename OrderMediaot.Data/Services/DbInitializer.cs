using OrderMediator.Data.Data;
using OrderMediator.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext dbContext;

        public DbInitializer(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            var priceListsExist = dbContext.PriceLists!.Any();
            if (!priceListsExist)
            {
                var defaultPriceList = new PriceList
                {
                    Code = "Default"
                };

                dbContext.PriceLists!.Add(defaultPriceList);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
