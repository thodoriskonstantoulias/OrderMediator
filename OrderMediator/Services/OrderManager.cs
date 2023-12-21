using OrderMediator.Data.Services;
using OrderMediator.Exceptions;
using OrderMediator.Models;
using System.Text;

namespace OrderMediator.Services
{
    // feedback: wanted better exception handling here - showing stack trace too when error thrown from validations
    public class OrderManager : IOrderManager
    {
        private readonly IPriceService priceService;
        private readonly IPriceResolver priceResolver;
        private readonly IEmailService emailService;
        private readonly IERPService erpService;
        private readonly IOrderManagementSystemService orderManagementSystemService;

        public OrderManager(IPriceService priceService, 
            IPriceResolver priceResolver,
            IEmailService emailService,
            IERPService erpService,
            IOrderManagementSystemService orderManagementSystemService)
        {
            this.priceService = priceService;
            this.priceResolver = priceResolver;
            this.emailService = emailService;
            this.erpService = erpService;
            this.orderManagementSystemService = orderManagementSystemService;
        }

        public async Task<OrderMediatorResult> SendOrderAsync(IFormFile file)
        {
            try
            {
                // Validate file contents
                var orderDetails = await this.ReadFromFileAsync(file);
                this.ValidateOrderContent(orderDetails);

                // Prices step
                var prices = await this.priceService.CheckAndResolvePricesMismatchAsync(orderDetails.OrderDetails.Select(x => x.EANArticle)!, orderDetails.OrderHeader!.EANBuyer!);
                if (!prices.Any())
                {
                    return new OrderMediatorResult
                    {
                        Success = false,
                        ErrorMessage = "Prices not resolved"
                    };
                }

                var mismatch = this.priceResolver.ResolveFinalPrices(orderDetails, prices);

                if (mismatch)
                {
                    //send mail - mock
                    await this.emailService.SendMailAsync("There is a price mismatch");
                }

                //ERP Step
                var erpResult = await this.erpService.CheckStockAvailabilityAsync(orderDetails);
                if (!erpResult!.Success)
                {
                    //send mail - mock
                    await this.emailService.SendMailAsync($"Following articles are not available from erp : {string.Join(", ", erpResult.UnavailableItems!)}");
                    return new OrderMediatorResult
                    {
                        Success = false,
                        ErrorMessage = "Stock not available"
                    };
                }

                var erpUpdateResult = await this.erpService.UpdateStockAsync(orderDetails);
                if (!erpUpdateResult!.Success)
                {
                    return new OrderMediatorResult
                    {
                        Success = false,
                        ErrorMessage = "Error in updating stock"
                    };
                }

                //Send order Step
                var sendOrder = await this.orderManagementSystemService.SendOrderToManagementSystemAsync(orderDetails);
                if (!sendOrder!.Success)
                {
                    return new OrderMediatorResult
                    {
                        Success = false,
                        ErrorMessage = "Error in sending order"
                    };
                }

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
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

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
