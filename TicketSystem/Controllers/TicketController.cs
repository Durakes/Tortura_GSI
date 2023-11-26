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
                            .AsNoTracking()
                            .OrderBy(e => e.TicketID)
                            .ToListAsync();
            var viewModel = new TicketDashboard
            {
                Name = objUser.Name,
                Tickets = tickets
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new TicketCreateViewModel
            {
                Games = _context.CasinoGames.Select(g => new SelectListItem { Value = g.CasinoGameID.ToString(), Text = g.Name }),
                RequestTypes = _context.RequestTypes.Select(r => new SelectListItem { Value = r.RequestTypeID.ToString(), Text = r.Name }),
                Priorities = _context.Priorities.Select(p => new SelectListItem { Value = p.PriorityID.ToString(), Text = p.Name }),
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
                    Timestamp = viewModel.Date,
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
            viewModel.Priorities = _context.Priorities.Select(p => new SelectListItem { Value = p.PriorityID.ToString(), Text = p.Name });
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
                ticket.Timestamp = viewModel.Date;
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

            var answers = await _context.Answers.Where(e => e.TicketID == ticket.TicketID).ToListAsync();

            var viewModel = new TicketDetailViewModel
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
    }

}
