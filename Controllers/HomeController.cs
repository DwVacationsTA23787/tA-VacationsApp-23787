using Dw23787.Data;
using Dw23787.Models;
using Dw23787.Models.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using static Dw23787.Models.Trips;


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
                var applicationDbContext = _Context.Trips.Where(t => t.UserFK != user.Id).Include(t => t.Group).Include(t => t.User);
                return View("HomePageLogged", await applicationDbContext.ToListAsync());
            }
                else
                {

                        ViewBag.numUsers = _Context.UsersApp.Count();
                        ViewBag.numTrips = _Context.Trips.Count();
                        return View();
                }

        }


        [HttpPost]
        public async Task<ActionResult> SearchAsync(string searchText, string category)
        {

            var query = _Context.Trips.AsQueryable();

            // Apply category filter if category is not empty or null
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                if (Enum.TryParse(typeof(Trips.TripCategory), category, true, out var parsedCategory))
                {
                    query = query.Where(t => t.Category == (Trips.TripCategory)parsedCategory);
                }
                else
                {
                    return BadRequest("Invalid category.");
                }
            }

            // Apply search filter if search is not empty or null
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(t => t.TripName.Contains(searchText) || t.Description.Contains(searchText));
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Users user = _Context.UsersApp.FirstOrDefault(u => u.UserID == userId);
            ViewData["UserName"] = user.Name;

            // Filter out trips where UserFK matches the provided id
            query = query.Where(t => t.UserFK != user.Id);

            var applicationDbContext = query.Include(t => t.Group).Include(t => t.User);
            return View("HomePageLogged", await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// This function will return the Detail Page.
        /// Logic:
        ///   If id is null return the page previously loaded.
        ///   if id is not null:
        ///     Load Trip from DB, case not exists returns page previously loaded, case exists continue.
        ///     
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> TravelInfoAsync(string id)
        {

            if (id == null)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Users user = _Context.UsersApp.FirstOrDefault(u => u.UserID == userId);
                ViewData["UserName"] = user.Name;
                var applicationDbContext = _Context.Trips.Include(t => t.Group).Include(t => t.User);
                return View("HomePageLogged", await applicationDbContext.ToListAsync());
            }

            var trip = await _Context.Trips
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trip == null)
            {
                // put train 
                return NotFound();
            }

            var tripDto = new TripDto
            {
                Id = trip.Id,
                TripName = trip.TripName,
                Description = trip.Description,
                Category = GetCategoryText(trip.Category), // Convert enum to text
                Transport = GetTransportText(trip.Transport), // Convert enum to text
                InicialBudget = trip.InicialBudget,
                FinalBudget = trip.FinalBudget,
                Banner = trip.Banner,
                Location = trip.Location,
                Closed = trip.Closed,
                GroupId = trip.GroupId,
                User = new UserDto
                {
                    Id = trip.User.Id,
                    Name = trip.User.Name,
                    Age = trip.User.Age,
                    Gender = trip.User.Gender,
                    Nationality = trip.User.Nationality,
                    ProfilePicture = trip.User.ProfilePicture,
                    DataNascimento = trip.User.DataNascimento,
                    Quote = trip.User.Quote
                }
            };

            return View("TravelInfo", tripDto);
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


        // Aux Functions
        private string GetCategoryText(TripCategory category)
        {
            switch (category)
            {
                case TripCategory.Adventure:
                    return "Adventure";
                case TripCategory.Leisure:
                    return "Leisure";
                case TripCategory.Cultural:
                    return "Cultural";
                case TripCategory.Business:
                    return "Business";
                case TripCategory.Family:
                    return "Family";
                default:
                    return string.Empty;
            }
        }

        private string GetTransportText(TripTransport transport)
        {
            switch (transport)
            {
                case TripTransport.Plane:
                    return "Plane";
                case TripTransport.Bus:
                    return "Bus";
                case TripTransport.Train:
                    return "Train";
                case TripTransport.Hitchhiking:
                    return "Hitchhiking";
                default:
                    return string.Empty;
            }
        }
    }


}
