namespace CustomerSinglePage.Models
{
    public class ShopDTO
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime Date { get; set; }
        
        public int MarketingId { get; set; }
        public string MarketingName { get; set; } = "";
        public string MarketingColor { get; set; }
    }
}