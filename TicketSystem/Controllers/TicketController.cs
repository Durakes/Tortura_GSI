using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TicketSystem.Controllers
{
    public class TicketController : Controller
    {
        public readonly ApplicationDbContext _context;
        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("SesionSoporte");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("NoEncontrado");
            }

            var objUser = JsonConvert.DeserializeObject<Employee>(user);
            var tickets = await _context.Tickets
                            .Where(e => e.SupportTechID == objUser.EmployeeID)
                            .Include(e => e.Engineer)
                            .Include(e => e.Status)
                            .Include(e => e.Priority)
                            .AsNoTracking()
                            .OrderByDescending(e => e.TicketID)
                            .ToListAsync();
            var viewModel = new TicketDashboard
            {
                Name = objUser.Name,
                Tickets = tickets
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int filtroEstado)
        {
            var user = HttpContext.Session.GetString("SesionSoporte");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("NoEncontrado");
            }

            var objUser = JsonConvert.DeserializeObject<Employee>(user);
            var tickets = await _context.Tickets
                            .Where(e => e.SupportTechID == objUser.EmployeeID)
                            .Include(e => e.Engineer)
                            .Include(e => e.Status)
                            .Include(e => e.Priority)
                            .AsNoTracking()
                            .OrderByDescending(e => e.TicketID)
                            .ToListAsync();
            if (filtroEstado != 0)
            {
                var filteredTickets = tickets.Where(e => e.StatusID == filtroEstado).ToList();
                var viewModel = new TicketDashboard
                {
                    Name = objUser.Name,
                    Tickets = filteredTickets
                };

                return View(viewModel);
            }
            var views = new TicketDashboard
            {
                Name = objUser.Name,
                Tickets = tickets
            };
            return View(views);

        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TicketCreateViewModel
            {
                Games = _context.CasinoGames.Select(g => new SelectListItem { Value = g.CasinoGameID.ToString(), Text = g.Name }),
                RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name }),
                Priorities = _context.Priorities.ToList(),
                Engineers = _context.Employees.Where(e => e.JobPositionID == 2).Select(e => new SelectListItem { Value = e.EmployeeID.ToString(), Text = e.Name }),
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TicketCreateViewModel viewModel)
        {
            var user = HttpContext.Session.GetString("SesionSoporte");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("NoEncontrado");
            }

            var objUser = JsonConvert.DeserializeObject<Employee>(user);
            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local),
                    StatusID = 1,
                    PriorityID = viewModel.SelectedPriorityID,
                    EngineerID = viewModel.SelectedEngineerID,
                    SupportTechID = objUser.EmployeeID,
                    RequestTypeID = viewModel.SelectedRequestTypeID,
                    CasinoGameID = viewModel.SelectedCasinoGameID
                };

                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            viewModel.Priorities = _context.Priorities.ToList();
            viewModel.RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name });

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _context.Tickets
                            .Include(t => t.Status)
                            .Include(t => t.Priority)
                            .Include(t => t.Engineer)
                            .Include(t => t.RequestType)
                            .Include(t => t.CasinoGame)
                            .FirstOrDefaultAsync(t => t.TicketID == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var viewModel = new TicketEditViewModel
            {
                TicketID = id,
                Title = ticket.Title,
                Description = ticket.Description,
                Date = ticket.Timestamp,
                SelectedStatusID = ticket.StatusID,
                Statuses = _context.Statuses.Select(p => new SelectListItem { Value = p.StatusID.ToString(), Text = p.Name }),
                SelectedPriorityID = ticket.PriorityID,
                Priorities = _context.Priorities.Select(p => new SelectListItem { Value = p.PriorityID.ToString(), Text = p.Name }),
                SelectedEngineerID = ticket.PriorityID,
                Engineers = _context.Employees.Where(e => e.JobPositionID == 2).Select(p => new SelectListItem { Value = p.EmployeeID.ToString(), Text = p.Name }),
                SelectedRequestTypeID = ticket.RequestTypeID,
                RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name }),
                SelectedCasinoGameID = ticket.CasinoGameID,
                Games = _context.CasinoGames.Select(r => new SelectListItem { Value = r.CasinoGameID.ToString(), Text = r.Name })
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TicketEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var ticket = await _context.Tickets.FindAsync(id);

                if (ticket is null)
                {
                    return NotFound();
                }

                ticket.Title = viewModel.Title;
                ticket.Description = viewModel.Description;
                ticket.StatusID = viewModel.SelectedStatusID;
                ticket.PriorityID = viewModel.SelectedPriorityID;
                ticket.EngineerID = viewModel.SelectedEngineerID;
                ticket.RequestTypeID = viewModel.SelectedRequestTypeID;
                ticket.CasinoGameID = viewModel.SelectedCasinoGameID;

                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            viewModel.Statuses = _context.Statuses.Select(r => new SelectListItem { Value = r.StatusID.ToString(), Text = r.Name });
            viewModel.Priorities = _context.Priorities.Select(p => new SelectListItem { Value = p.PriorityID.ToString(), Text = p.Name });
            viewModel.Engineers = _context.Employees.Where(e => e.JobPositionID == 2).Select(r => new SelectListItem { Value = r.EmployeeID.ToString(), Text = r.Name });
            viewModel.RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name });
            viewModel.Games = _context.CasinoGames.Select(r => new SelectListItem { Value = r.CasinoGameID.ToString(), Text = r.Name });

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var ticket = await _context.Tickets
                            .Include(t => t.Status)
                            .Include(t => t.Priority)
                            .Include(t => t.Engineer)
                            .Include(t => t.RequestType)
                            .Include(t => t.CasinoGame)
                            .FirstOrDefaultAsync(t => t.TicketID == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var answers = await _context.Answers
                                .Include(e => e.Employee)
                                .AsNoTracking()
                                .Where(e => e.TicketID == ticket.TicketID).ToListAsync();

            var viewModel = new TicketDetailViewModel
            {
                TicketID = id,
                Title = ticket.Title,
                Description = ticket.Description,
                Date = ticket.Timestamp,
                StatusTicket = ticket.Status,
                SelectedStatusID = ticket.StatusID,
                Statuses = _context.Statuses.Select(p => new SelectListItem { Value = p.StatusID.ToString(), Text = p.Name }),
                SelectedPriorityID = ticket.PriorityID,
                Priorities = _context.Priorities.Select(p => new SelectListItem { Value = p.PriorityID.ToString(), Text = p.Name }),
                SelectedEngineerID = ticket.PriorityID,
                Engineers = _context.Employees.Where(e => e.JobPositionID == 2).Select(p => new SelectListItem { Value = p.EmployeeID.ToString(), Text = p.Name }),
                SelectedRequestTypeID = ticket.RequestTypeID,
                RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name }),
                SelectedCasinoGameID = ticket.CasinoGameID,
                Games = _context.CasinoGames.Select(r => new SelectListItem { Value = r.CasinoGameID.ToString(), Text = r.Name }),
                Answers = answers
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateTicket(int ticketId, string selectedValue, string propertyName)
        {
            var ticket = _context.Tickets.FirstOrDefault(t => t.TicketID == ticketId);
            if (ticket != null)
            {
                switch (propertyName)
                {
                    case "SelectedStatusID":
                        ticket.StatusID = Convert.ToInt32(selectedValue);
                        break;
                    case "SelectedPriorityID":
                        ticket.PriorityID = Convert.ToInt32(selectedValue);
                        break;
                    case "SelectedEngineerID":
                        ticket.EngineerID = Convert.ToInt32(selectedValue);
                        break;
                }
                _context.Update(ticket);
                _context.SaveChanges();
                return Json(new { success = true, message = "Ticket actualizado correctamente." });
            }

            return Json(new { success = false, message = "Error al actualizar el ticket." });
        }

    }

}
