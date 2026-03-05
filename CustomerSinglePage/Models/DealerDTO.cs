namespace CustomerSinglePage.Models
{
    public class DealerDTO
    {
        public int DealerId { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal DueAmount { get; set; }
        public int MarketingId { get; set; }
        public string MarketingName { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
