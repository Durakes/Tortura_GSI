using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using Newtonsoft.Json;

namespace TicketSystem.Controllers
{
    public class AnswerController : Controller
    {
        public readonly ApplicationDbContext _context;
        public AnswerController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(NewAnswerViewModel model)
        {
            var user = HttpContext.Session.GetString("SesionSoporte");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("NoEncontrado");
            }
            
            var objUser = JsonConvert.DeserializeObject<Employee>(user);

            var answer = new Answer
            {
                TicketID = model.TicketID,
                Name = model.Name,
                EmployeeID = objUser.EmployeeID,
                CreationTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local)
            };

            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail", "Ticket", new {id = model.TicketID});
        }
    }
}
