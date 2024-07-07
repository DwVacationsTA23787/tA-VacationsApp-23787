using Dw23787.Data;
using Dw23787.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;


namespace Dw23787.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext _Context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _Context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {

                if (User.Identity.IsAuthenticated)
                {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Users user = _Context.UsersApp.FirstOrDefault(u => u.UserID == userId);
                ViewData["UserName"] = user.Name;
                // Aqui é onde tenho de filtrar as viagens para nao aparecer as do user
                var applicationDbContext = _Context.Trips.Include(t => t.Group).Include(t => t.User);
                return View("HomePageLogged", await applicationDbContext.ToListAsync());
            }
                else
                {

                        ViewBag.numUsers = _Context.UsersApp.Count();
                        ViewBag.numTrips = _Context.Trips.Count();
                        return View();
                }

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
