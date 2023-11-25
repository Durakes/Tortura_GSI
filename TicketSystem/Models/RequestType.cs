using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class RequestType
    {
        [Key]
        public int RequestTypeID { get; set; }
        public string Name { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}