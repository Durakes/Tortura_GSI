using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using Newtonsoft.Json;

namespace TicketSystem.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Employees.SingleOrDefaultAsync(e => e.Email == model.Email && e.Password == model.Password);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                    ViewBag.ShowError = true;
                    TempData["Error"] = "Inténtalo más tarde";
                    return View(model);
                }

                switch (user.JobPositionID)
                {
                    case 1:
                        HttpContext.Session.SetString("SesionSoporte", JsonConvert.SerializeObject(user));
                        return RedirectToAction("Index", "Ticket");
                    case 2:
                        HttpContext.Session.SetString("SesionTecnico", JsonConvert.SerializeObject(user));
                        return RedirectToAction("Index", "Ticket");
                    default:
                        return View();
                }
            }
            return View();
        }
    }
}
