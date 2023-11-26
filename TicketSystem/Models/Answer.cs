using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Answer
    {
        [Key]
        public int AnswerID { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        [ForeignKey("Ticket")]
        public int TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}