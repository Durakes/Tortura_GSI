using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public virtual Status Status { get; set; }
        [ForeignKey("Priority")]
        public int PriorityID { get; set; }
        public virtual Priority Priority { get; set; }
        [ForeignKey("Employee")]
        public int EngineerID { get; set; }
        public virtual Employee Engineer { get; set; }
        [ForeignKey("Employee")]
        public int SupportTechID { get; set; }
        public virtual Employee SupportTech { get; set; }
        [ForeignKey("RequestType")]
        public int RequestTypeID { get; set; }
        public virtual RequestType RequestType { get; set; }
        [ForeignKey("CasinoGame")]
        public int CasinoGameID { get; set; }
        public virtual CasinoGame CasinoGame { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}