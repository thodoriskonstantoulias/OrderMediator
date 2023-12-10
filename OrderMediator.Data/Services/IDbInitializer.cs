using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMediator.Data.Services
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
