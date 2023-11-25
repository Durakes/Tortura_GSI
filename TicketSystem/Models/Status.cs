using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Status
    {
        [Key]
        public int StatusID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}