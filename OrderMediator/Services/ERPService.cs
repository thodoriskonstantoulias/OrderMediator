using OrderMediator.Models;
using System.Net.Http;

namespace OrderMediator.Services
{
    // Mock
    // Real world scenario - we have documentation on how to call the erp, with request and response models
    public class ERPService : IERPService
    {
        private readonly HttpClient client;

        public ERPService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ErpResponse?> CheckStockAvailabilityAsync(OrderModel orderModel)
        {
            var model = orderModel.OrderDetails.Select(x => new { Code = x.EANArticle, Quantity = x.Quantity });

            // Below is what we would do if we had a valid endpoint

            //using (HttpResponseMessage response = await client.PostAsJsonAsync("checkStockRoute", model))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var responseModel = await response.Content.ReadFromJsonAsync<ErpResponse>();

            //    return responseModel;
            //}

            // Below is the happy path for our mock use case 
            return new ErpResponse
            {
                Success = true
            };
        }

        public async Task<ErpUpdateResponse?> UpdateStockAsync(OrderModel orderModel)
        {
            var model = orderModel.OrderDetails.Select(x => new { Code = x.EANArticle, Quantity = x.Quantity });

            // Below is what we would do if we had a valid endpoint

            //using (HttpResponseMessage response = await client.PostAsJsonAsync("updateStockRoute", model))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var responseModel = await response.Content.ReadFromJsonAsync<ErpUpdateResponse>();

            //    return responseModel;
            //}

            // Below is the happy path for our mock use case 
            return new ErpUpdateResponse
            {
                Success = true
            };
        }
    }

    public class ErpResponse
    {
        public bool Success { get; set; }

        public List<string>? UnavailableItems { get; set; }
    }

    public class ErpUpdateResponse
    {
        public bool Success { get; set; }
    }
}
