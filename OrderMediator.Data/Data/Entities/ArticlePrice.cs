using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Data.Entities
{
    public class ArticlePrice
    {
        [Key]
        public string? ArticleCode { get; set; }

        [Key]
        [Required]
        public int? PriceListID { get; set; }

        [ForeignKey(nameof(PriceListID))]
        public PriceList? PriceList { get; set; }

        [Required]
        public decimal? Price { get; set; }
    }
}