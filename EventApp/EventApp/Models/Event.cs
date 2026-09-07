namespace EventApp.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int AvailableTickets { get; set; }
        public decimal Price { get; set; }
    }
}