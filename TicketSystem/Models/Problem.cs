using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class Problem
    {
        [Key]
        public int ProblemID { get; set; }
        public string Description { get; set; }
        [ForeignKey("Priority")]
        public int PriorityID { get; set; }
        public virtual Priority Priority { get; set; }
        [ForeignKey("CasinoGame")]
        public int CasinoGameID { get; set; }
        public virtual CasinoGame CasinoGame { get; set; }
    }
}