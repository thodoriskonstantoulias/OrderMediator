using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Data.Entities
{
    public class PriceList
    {
        public int ID { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}