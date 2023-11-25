namespace TicketSystem.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int IdJob {get; set;}

        //! TODO Navegation
    }
}