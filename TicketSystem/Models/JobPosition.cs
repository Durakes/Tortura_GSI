using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class JobPosition
    {
        [Key]
        public int JobPositionID { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}