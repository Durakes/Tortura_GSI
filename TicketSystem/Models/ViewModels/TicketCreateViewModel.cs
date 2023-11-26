using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketSystem.Models
{
    public class TicketCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int SelectedCasinoGameID { get; set; }
        public IEnumerable<SelectListItem> Games { get; set; }
        public int SelectedRequestTypeID { get; set; }
        public IEnumerable<SelectListItem> RequestTypes { get; set; }

        public int SelectedPriorityID { get; set; }
        public IEnumerable<Priority> Priorities { get; set; }

        public int SelectedEngineerID { get; set; }
        public IEnumerable<SelectListItem> Engineers { get; set; }

    }
}