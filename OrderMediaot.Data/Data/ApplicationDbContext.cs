using Microsoft.EntityFrameworkCore;
using OrderMediator.Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArticlePrice>? ArticlePrices { get; set; }

        public DbSet<PriceList>? PriceLists { get; set; }
    }
}
