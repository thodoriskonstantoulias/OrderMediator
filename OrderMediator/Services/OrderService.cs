using OrderMediator.Exceptions;
using OrderMediator.Models;
using System.Text;

namespace OrderMediator.Services
{
    public class OrderService : IOrderService
    {
        public async Task<OrderMediatorResult> SendOrderAsync(IFormFile file)
        {
            try
            {
                var orderDetails = await this.ReadFromFileAsync(file);
                this.ValidateOrderContent(orderDetails);
            }
            catch (OrderException ex)
            {
                return new OrderMediatorResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new OrderMediatorResult
                {
                    Success = false,
                    ErrorMessage = "There is problem with the order"
                };
            }

            return new OrderMediatorResult
            {
                Success = true
            };
        }

        private void ValidateOrderContent(OrderModel orderDetails)
        {
            if (!orderDetails.OrderDetails.Any())
            {
                throw new OrderException("No order details provided");
            }

            var faultedQuantity = orderDetails.OrderDetails.FirstOrDefault(x => x.Quantity <= 0);
            if (faultedQuantity != null)
            {
                throw new OrderException($"Quantity for {faultedQuantity.EANArticle} is wrong");
            }

            var faultedPrice = orderDetails.OrderDetails.FirstOrDefault(x => x.UnitPrice <= 0);
            if (faultedPrice != null)
            {
                throw new OrderException($"Price for {faultedPrice.EANArticle} is wrong");
            }
        }

        private async Task<OrderModel> ReadFromFileAsync(IFormFile file)
        {
            var result = new OrderModel();
            int count = 0;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    count++;
                    var line = await reader.ReadLineAsync();

                    if (count == 1)
                    {
                        result.OrderHeader = GetOrderHeader(line);
                    }
                    else
                    {
                        result.OrderDetails.Add(this.GetOrderDetailLine(line));
                    }
                }
            }

            return result;
        }

        private OrderDetail GetOrderDetailLine(string line)
        {
            return new OrderDetail
            {
                EANArticle = line.Substring(0, 13),
                ArticleDescription = line.Substring(13, 65),
                Quantity = int.Parse(line.Substring(78, 10)),
                UnitPrice = decimal.Parse(line.Substring(88, 10))
            };
        }

        private OrderHeader GetOrderHeader(string line)
        {
            return new OrderHeader
            {
                FileType = line.Substring(0, 3),
                OrderNumber = line.Substring(3, 20),
                OrderDate = DateTime.ParseExact(line.Substring(23, 13), "yyyyMMddTHHmm", null),
                EANBuyer = line.Substring(36, 13),
                EANSupplier = line.Substring(49, 13),
                FreeText = line.Substring(62, 100),
            };
        }
    }
}
