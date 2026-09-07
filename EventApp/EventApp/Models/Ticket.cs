namespace EventApp.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
    }
}