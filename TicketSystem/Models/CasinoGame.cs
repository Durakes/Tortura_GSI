using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class CasinoGame
    {
        [Key]
        public int CasinoGameID { get; set; }
        public string Name { get; set; }
        public string Casino { get; set; }
        public ICollection<Problem> Problems { get; set; }
    }
}