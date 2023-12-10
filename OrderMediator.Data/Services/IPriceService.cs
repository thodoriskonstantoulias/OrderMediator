using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Services
{
    public interface IPriceService
    {
        // return article code - dbPrice
        Task<Dictionary<string, decimal?>> CheckAndResolvePricesMismatchAsync(IEnumerable<string> articleCodes, string buyer);
    }
}
