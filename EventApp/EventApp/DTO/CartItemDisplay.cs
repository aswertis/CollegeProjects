namespace EventApp.DTO
{
    public class CartItemDisplay
    {
        public int OrderId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public decimal TotalAmount { get; set; }
    }
}