using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TicketSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        [ForeignKey("JobPosition")]
        public int JobPositionID { get; set; }
        public virtual JobPosition JobPosition { get; set; }
        // Navigation property for Tickets where the employee is an engineer
        [InverseProperty("Engineer")]
        public ICollection<Ticket> EngineerTickets { get; set; }

        // Navigation property for Tickets where the employee is a support tech
        [InverseProperty("SupportTech")]
        public ICollection<Ticket> SupportTechTickets { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}