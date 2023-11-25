namespace TicketSystem.Models
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public int StatusID { get; set; }
        public int PriorityID { get; set; }
        public int DeveloperID { get; set; }
        public int RequestTypeID { get; set; }
        public int GameID { get; set; }

        //!TODO NAVIGATION
    }
}