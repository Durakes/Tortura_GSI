using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
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

                

                return RedirectToAction("Index", "Home");
            }

            // Si llegamos aquí, significa que hubo un error en la validación
            return View();
        }
    }
}
