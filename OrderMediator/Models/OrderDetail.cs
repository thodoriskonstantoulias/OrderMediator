namespace OrderMediator.Models
{
    public class OrderDetail
    {
        public string? EANArticle { get; set; }
        public string? ArticleDescription { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
