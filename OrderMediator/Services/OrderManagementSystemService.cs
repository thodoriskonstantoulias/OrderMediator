using OrderMediator.Extensions;
using OrderMediator.Models;

namespace OrderMediator.Services
{
    public class OrderManagementSystemService : IOrderManagementSystemService
    {
        private readonly HttpClient client;

        public OrderManagementSystemService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<OrderManagementSystemResponse?> SendOrderToManagementSystemAsync(OrderModel orderModel)
        {
            var xmlString = orderModel.TransformToXml();
            var model = new { Order = xmlString };

            // Below is what we would do if we had a valid endpoint

            //using (HttpResponseMessage response = await client.PostAsJsonAsync("orderManagementSystemRoute", model))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var responseModel = await response.Content.ReadFromJsonAsync<OrderManagementSystemResponse>();

            //    return responseModel;
            //}

            // Below is the happy path for our mock use case 
            return new OrderManagementSystemResponse
            {
                Success = true
            };
        }
    }

    public class OrderManagementSystemResponse
    {
        public bool Success { get; set; }
    }
}
