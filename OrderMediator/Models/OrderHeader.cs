namespace OrderMediator.Models
{
    public class OrderHeader
    {
        public string? FileType { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? EANBuyer { get; set; }
        public string? EANSupplier { get; set; }
        public string? FreeText { get; set; }
    }
}
