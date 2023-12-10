namespace OrderMediator.Models
{
    public class OrderModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
