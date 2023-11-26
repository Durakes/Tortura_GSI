using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Priority
    {
        [Key]
        public int PriorityID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}