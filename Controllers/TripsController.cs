using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dw23787.Data;
using Dw23787.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace Dw23787.Controllers
{

    [Authorize]
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// objecto que contém os dados do Servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TripsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            //ViewData["ActivePage"] = "Trips";
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Users user = _context.UsersApp.FirstOrDefault(u => u.UserID == userId);

            if(user.isAdmin == false)
            {
            var applicationDbContext = _context.Trips.Where(t => t.UserFK == user.Id).Include(t => t.Group).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());

            }

            var applicationDbContexts = _context.Trips.Include(t => t.Group).Include(t => t.User);
            return View(await applicationDbContexts.ToListAsync());

        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips
                .Include(t => t.Group)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trips == null)
            {
                return NotFound();
            }

            return View(trips);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            var countries = new List<Country>
            {
               new Country { Id = 1, Name = "Afghanistan" },
                new Country { Id = 2, Name = "Albania" },
                new Country { Id = 3, Name = "Algeria" },
                new Country { Id = 4, Name = "Andorra" },
                new Country { Id = 5, Name = "Angola" },
                new Country { Id = 6, Name = "Antigua and Barbuda" },
                new Country { Id = 7, Name = "Argentina" },
                new Country { Id = 8, Name = "Armenia" },
                new Country { Id = 9, Name = "Australia" },
                new Country { Id = 10, Name = "Austria" },
                new Country { Id = 11, Name = "Azerbaijan" },
                new Country { Id = 12, Name = "Bahamas" },
                new Country { Id = 13, Name = "Bahrain" },
                new Country { Id = 14, Name = "Bangladesh" },
                new Country { Id = 15, Name = "Barbados" },
                new Country { Id = 16, Name = "Belarus" },
                new Country { Id = 17, Name = "Belgium" },
                new Country { Id = 18, Name = "Belize" },
                new Country { Id = 19, Name = "Benin" },
                new Country { Id = 20, Name = "Bhutan" },
                new Country { Id = 21, Name = "Bolivia" },
                new Country { Id = 22, Name = "Bosnia and Herzegovina" },
                new Country { Id = 23, Name = "Botswana" },
                new Country { Id = 24, Name = "Brazil" },
                new Country { Id = 25, Name = "Brunei" },
                new Country { Id = 26, Name = "Bulgaria" },
                new Country { Id = 27, Name = "Burkina Faso" },
                new Country { Id = 28, Name = "Burundi" },
                new Country { Id = 29, Name = "Cabo Verde" },
                new Country { Id = 30, Name = "Cambodia" },
                new Country { Id = 31, Name = "Cameroon" },
                new Country { Id = 32, Name = "Canada" },
                new Country { Id = 33, Name = "Central African Republic" },
                new Country { Id = 34, Name = "Chad" },
                new Country { Id = 35, Name = "Chile" },
                new Country { Id = 36, Name = "China" },
                new Country { Id = 37, Name = "Colombia" },
                new Country { Id = 38, Name = "Comoros" },
                new Country { Id = 39, Name = "Congo, Democratic Republic of the" },
                new Country { Id = 40, Name = "Congo, Republic of the" },
                new Country { Id = 41, Name = "Costa Rica" },
                new Country { Id = 42, Name = "Croatia" },
                new Country { Id = 43, Name = "Cuba" },
                new Country { Id = 44, Name = "Cyprus" },
                new Country { Id = 45, Name = "Czech Republic" },
                new Country { Id = 46, Name = "Denmark" },
                new Country { Id = 47, Name = "Djibouti" },
                new Country { Id = 48, Name = "Dominica" },
                new Country { Id = 49, Name = "Dominican Republic" },
                new Country { Id = 50, Name = "Ecuador" },
                new Country { Id = 51, Name = "Egypt" },
                new Country { Id = 52, Name = "El Salvador" },
                new Country { Id = 53, Name = "Equatorial Guinea" },
                new Country { Id = 54, Name = "Eritrea" },
                new Country { Id = 55, Name = "Estonia" },
                new Country { Id = 56, Name = "Eswatini" },
                new Country { Id = 57, Name = "Ethiopia" },
                new Country { Id = 58, Name = "Fiji" },
                new Country { Id = 59, Name = "Finland" },
                new Country { Id = 60, Name = "France" },
                new Country { Id = 61, Name = "Gabon" },
                new Country { Id = 62, Name = "Gambia" },
                new Country { Id = 63, Name = "Georgia" },
                new Country { Id = 64, Name = "Germany" },
                new Country { Id = 65, Name = "Ghana" },
                new Country { Id = 66, Name = "Greece" },
                new Country { Id = 67, Name = "Grenada" },
                new Country { Id = 68, Name = "Guatemala" },
                new Country { Id = 69, Name = "Guinea" },
                new Country { Id = 70, Name = "Guinea-Bissau" },
                new Country { Id = 71, Name = "Guyana" },
                new Country { Id = 72, Name = "Haiti" },
                new Country { Id = 73, Name = "Honduras" },
                new Country { Id = 74, Name = "Hungary" },
                new Country { Id = 75, Name = "Iceland" },
                new Country { Id = 76, Name = "India" },
                new Country { Id = 77, Name = "Indonesia" },
                new Country { Id = 78, Name = "Iran" },
                new Country { Id = 79, Name = "Iraq" },
                new Country { Id = 80, Name = "Ireland" },
                new Country { Id = 81, Name = "Israel" },
                new Country { Id = 82, Name = "Italy" },
                new Country { Id = 83, Name = "Jamaica" },
                new Country { Id = 84, Name = "Japan" },
                new Country { Id = 85, Name = "Jordan" },
                new Country { Id = 86, Name = "Kazakhstan" },
                new Country { Id = 87, Name = "Kenya" },
                new Country { Id = 88, Name = "Kiribati" },
                new Country { Id = 89, Name = "Korea, North" },
                new Country { Id = 90, Name = "Korea, South" },
                new Country { Id = 91, Name = "Kosovo" },
                new Country { Id = 92, Name = "Kuwait" },
                new Country { Id = 93, Name = "Kyrgyzstan" },
                new Country { Id = 94, Name = "Laos" },
                new Country { Id = 95, Name = "Latvia" },
                new Country { Id = 96, Name = "Lebanon" },
                new Country { Id = 97, Name = "Lesotho" },
                new Country { Id = 98, Name = "Liberia" },
                new Country { Id = 99, Name = "Libya" },
                new Country { Id = 100, Name = "Liechtenstein" },
                new Country { Id = 101, Name = "Lithuania" },
                new Country { Id = 102, Name = "Luxembourg" },
                new Country { Id = 103, Name = "Madagascar" },
                new Country { Id = 104, Name = "Malawi" },
                new Country { Id = 105, Name = "Malaysia" },
                new Country { Id = 106, Name = "Maldives" },
                new Country { Id = 107, Name = "Mali" },
                new Country { Id = 108, Name = "Malta" },
                new Country { Id = 109, Name = "Marshall Islands" },
                new Country { Id = 110, Name = "Mauritania" },
                new Country { Id = 111, Name = "Mauritius" },
                new Country { Id = 112, Name = "Mexico" },
                new Country { Id = 113, Name = "Micronesia" },
                new Country { Id = 114, Name = "Moldova" },
                new Country { Id = 115, Name = "Monaco" },
                new Country { Id = 116, Name = "Mongolia" },
                new Country { Id = 117, Name = "Montenegro" },
                new Country { Id = 118, Name = "Morocco" },
                new Country { Id = 119, Name = "Mozambique" },
                new Country { Id = 120, Name = "Myanmar (Burma)" },
                new Country { Id = 121, Name = "Namibia" },
                new Country { Id = 122, Name = "Nauru" },
                new Country { Id = 123, Name = "Nepal" },
                new Country { Id = 124, Name = "Netherlands" },
                new Country { Id = 125, Name = "New Zealand" },
                new Country { Id = 126, Name = "Nicaragua" },
                new Country { Id = 127, Name = "Niger" },
                new Country { Id = 128, Name = "Nigeria" },
                new Country { Id = 129, Name = "North Macedonia" },
                new Country { Id = 130, Name = "Norway" },
                new Country { Id = 131, Name = "Oman" },
                new Country { Id = 132, Name = "Pakistan" },
                new Country { Id = 133, Name = "Palau" },
                new Country { Id = 134, Name = "Palestine" },
                new Country { Id = 135, Name = "Panama" },
                new Country { Id = 136, Name = "Papua New Guinea" },
                new Country { Id = 137, Name = "Paraguay" },
                new Country { Id = 138, Name = "Peru" },
                new Country { Id = 139, Name = "Philippines" },
                new Country { Id = 140, Name = "Poland" },
                new Country { Id = 141, Name = "Portugal" },
                new Country { Id = 142, Name = "Qatar" },
                new Country { Id = 143, Name = "Romania" },
                new Country { Id = 144, Name = "Russia" },
                new Country { Id = 145, Name = "Rwanda" },
                new Country { Id = 146, Name = "Saint Kitts and Nevis" },
                new Country { Id = 147, Name = "Saint Lucia" },
                new Country { Id = 148, Name = "Saint Vincent and the Grenadines" },
                new Country { Id = 149, Name = "Samoa" },
                new Country { Id = 150, Name = "San Marino" },
                new Country { Id = 151, Name = "Sao Tome and Principe" },
                new Country { Id = 152, Name = "Saudi Arabia" },
                new Country { Id = 153, Name = "Senegal" },
                new Country { Id = 154, Name = "Serbia" },
                new Country { Id = 155, Name = "Seychelles" },
                new Country { Id = 156, Name = "Sierra Leone" },
                new Country { Id = 157, Name = "Singapore" },
                new Country { Id = 158, Name = "Slovakia" },
                new Country { Id = 159, Name = "Slovenia" },
                new Country { Id = 160, Name = "Solomon Islands" },
                new Country { Id = 161, Name = "Somalia" },
                new Country { Id = 162, Name = "South Africa" },
                new Country { Id = 163, Name = "Spain" },
                new Country { Id = 164, Name = "Sri Lanka" },
                new Country { Id = 165, Name = "Sudan" },
                new Country { Id = 166, Name = "Suriname" },
                new Country { Id = 167, Name = "Sweden" },
                new Country { Id = 168, Name = "Switzerland" },
                new Country { Id = 169, Name = "Syria" },
                new Country { Id = 170, Name = "Taiwan" },
                new Country { Id = 171, Name = "Tajikistan" },
                new Country { Id = 172, Name = "Tanzania" },
                new Country { Id = 173, Name = "Thailand" },
                new Country { Id = 174, Name = "Timor-Leste" },
                new Country { Id = 175, Name = "Togo" },
                new Country { Id = 176, Name = "Tonga" },
                new Country { Id = 177, Name = "Trinidad and Tobago" },
                new Country { Id = 178, Name = "Tunisia" },
                new Country { Id = 179, Name = "Turkey" },
                new Country { Id = 180, Name = "Turkmenistan" },
                new Country { Id = 181, Name = "Tuvalu" },
                new Country { Id = 182, Name = "Uganda" },
                new Country { Id = 183, Name = "Ukraine" },
                new Country { Id = 184, Name = "United Arab Emirates" },
                new Country { Id = 185, Name = "United Kingdom" },
                new Country { Id = 186, Name = "United States" },
                new Country { Id = 187, Name = "Uruguay" },
                new Country { Id = 188, Name = "Uzbekistan" },
                new Country { Id = 189, Name = "Vanuatu" },
                new Country { Id = 190, Name = "Vatican City" },
                new Country { Id = 191, Name = "Venezuela" },
                new Country { Id = 192, Name = "Vietnam" },
                new Country { Id = 193, Name = "Yemen" },
                new Country { Id = 194, Name = "Zambia" },
                new Country { Id = 195, Name = "Zimbabwe" }
            };

            ViewBag.Countries = new SelectList(countries, "Id", "Name");
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TripName,Description,Category,Transport,InicialBudget,FinalBudget,Location,UserFK")] Trips trips, IFormFile? Banner)
        {


            var countries = new List<Country>
            {
               new Country { Id = 1, Name = "Afghanistan" },
                new Country { Id = 2, Name = "Albania" },
                new Country { Id = 3, Name = "Algeria" },
                new Country { Id = 4, Name = "Andorra" },
                new Country { Id = 5, Name = "Angola" },
                new Country { Id = 6, Name = "Antigua and Barbuda" },
                new Country { Id = 7, Name = "Argentina" },
                new Country { Id = 8, Name = "Armenia" },
                new Country { Id = 9, Name = "Australia" },
                new Country { Id = 10, Name = "Austria" },
                new Country { Id = 11, Name = "Azerbaijan" },
                new Country { Id = 12, Name = "Bahamas" },
                new Country { Id = 13, Name = "Bahrain" },
                new Country { Id = 14, Name = "Bangladesh" },
                new Country { Id = 15, Name = "Barbados" },
                new Country { Id = 16, Name = "Belarus" },
                new Country { Id = 17, Name = "Belgium" },
                new Country { Id = 18, Name = "Belize" },
                new Country { Id = 19, Name = "Benin" },
                new Country { Id = 20, Name = "Bhutan" },
                new Country { Id = 21, Name = "Bolivia" },
                new Country { Id = 22, Name = "Bosnia and Herzegovina" },
                new Country { Id = 23, Name = "Botswana" },
                new Country { Id = 24, Name = "Brazil" },
                new Country { Id = 25, Name = "Brunei" },
                new Country { Id = 26, Name = "Bulgaria" },
                new Country { Id = 27, Name = "Burkina Faso" },
                new Country { Id = 28, Name = "Burundi" },
                new Country { Id = 29, Name = "Cabo Verde" },
                new Country { Id = 30, Name = "Cambodia" },
                new Country { Id = 31, Name = "Cameroon" },
                new Country { Id = 32, Name = "Canada" },
                new Country { Id = 33, Name = "Central African Republic" },
                new Country { Id = 34, Name = "Chad" },
                new Country { Id = 35, Name = "Chile" },
                new Country { Id = 36, Name = "China" },
                new Country { Id = 37, Name = "Colombia" },
                new Country { Id = 38, Name = "Comoros" },
                new Country { Id = 39, Name = "Congo, Democratic Republic of the" },
                new Country { Id = 40, Name = "Congo, Republic of the" },
                new Country { Id = 41, Name = "Costa Rica" },
                new Country { Id = 42, Name = "Croatia" },
                new Country { Id = 43, Name = "Cuba" },
                new Country { Id = 44, Name = "Cyprus" },
                new Country { Id = 45, Name = "Czech Republic" },
                new Country { Id = 46, Name = "Denmark" },
                new Country { Id = 47, Name = "Djibouti" },
                new Country { Id = 48, Name = "Dominica" },
                new Country { Id = 49, Name = "Dominican Republic" },
                new Country { Id = 50, Name = "Ecuador" },
                new Country { Id = 51, Name = "Egypt" },
                new Country { Id = 52, Name = "El Salvador" },
                new Country { Id = 53, Name = "Equatorial Guinea" },
                new Country { Id = 54, Name = "Eritrea" },
                new Country { Id = 55, Name = "Estonia" },
                new Country { Id = 56, Name = "Eswatini" },
                new Country { Id = 57, Name = "Ethiopia" },
                new Country { Id = 58, Name = "Fiji" },
                new Country { Id = 59, Name = "Finland" },
                new Country { Id = 60, Name = "France" },
                new Country { Id = 61, Name = "Gabon" },
                new Country { Id = 62, Name = "Gambia" },
                new Country { Id = 63, Name = "Georgia" },
                new Country { Id = 64, Name = "Germany" },
                new Country { Id = 65, Name = "Ghana" },
                new Country { Id = 66, Name = "Greece" },
                new Country { Id = 67, Name = "Grenada" },
                new Country { Id = 68, Name = "Guatemala" },
                new Country { Id = 69, Name = "Guinea" },
                new Country { Id = 70, Name = "Guinea-Bissau" },
                new Country { Id = 71, Name = "Guyana" },
                new Country { Id = 72, Name = "Haiti" },
                new Country { Id = 73, Name = "Honduras" },
                new Country { Id = 74, Name = "Hungary" },
                new Country { Id = 75, Name = "Iceland" },
                new Country { Id = 76, Name = "India" },
                new Country { Id = 77, Name = "Indonesia" },
                new Country { Id = 78, Name = "Iran" },
                new Country { Id = 79, Name = "Iraq" },
                new Country { Id = 80, Name = "Ireland" },
                new Country { Id = 81, Name = "Israel" },
                new Country { Id = 82, Name = "Italy" },
                new Country { Id = 83, Name = "Jamaica" },
                new Country { Id = 84, Name = "Japan" },
                new Country { Id = 85, Name = "Jordan" },
                new Country { Id = 86, Name = "Kazakhstan" },
                new Country { Id = 87, Name = "Kenya" },
                new Country { Id = 88, Name = "Kiribati" },
                new Country { Id = 89, Name = "Korea, North" },
                new Country { Id = 90, Name = "Korea, South" },
                new Country { Id = 91, Name = "Kosovo" },
                new Country { Id = 92, Name = "Kuwait" },
                new Country { Id = 93, Name = "Kyrgyzstan" },
                new Country { Id = 94, Name = "Laos" },
                new Country { Id = 95, Name = "Latvia" },
                new Country { Id = 96, Name = "Lebanon" },
                new Country { Id = 97, Name = "Lesotho" },
                new Country { Id = 98, Name = "Liberia" },
                new Country { Id = 99, Name = "Libya" },
                new Country { Id = 100, Name = "Liechtenstein" },
                new Country { Id = 101, Name = "Lithuania" },
                new Country { Id = 102, Name = "Luxembourg" },
                new Country { Id = 103, Name = "Madagascar" },
                new Country { Id = 104, Name = "Malawi" },
                new Country { Id = 105, Name = "Malaysia" },
                new Country { Id = 106, Name = "Maldives" },
                new Country { Id = 107, Name = "Mali" },
                new Country { Id = 108, Name = "Malta" },
                new Country { Id = 109, Name = "Marshall Islands" },
                new Country { Id = 110, Name = "Mauritania" },
                new Country { Id = 111, Name = "Mauritius" },
                new Country { Id = 112, Name = "Mexico" },
                new Country { Id = 113, Name = "Micronesia" },
                new Country { Id = 114, Name = "Moldova" },
                new Country { Id = 115, Name = "Monaco" },
                new Country { Id = 116, Name = "Mongolia" },
                new Country { Id = 117, Name = "Montenegro" },
                new Country { Id = 118, Name = "Morocco" },
                new Country { Id = 119, Name = "Mozambique" },
                new Country { Id = 120, Name = "Myanmar (Burma)" },
                new Country { Id = 121, Name = "Namibia" },
                new Country { Id = 122, Name = "Nauru" },
                new Country { Id = 123, Name = "Nepal" },
                new Country { Id = 124, Name = "Netherlands" },
                new Country { Id = 125, Name = "New Zealand" },
                new Country { Id = 126, Name = "Nicaragua" },
                new Country { Id = 127, Name = "Niger" },
                new Country { Id = 128, Name = "Nigeria" },
                new Country { Id = 129, Name = "North Macedonia" },
                new Country { Id = 130, Name = "Norway" },
                new Country { Id = 131, Name = "Oman" },
                new Country { Id = 132, Name = "Pakistan" },
                new Country { Id = 133, Name = "Palau" },
                new Country { Id = 134, Name = "Palestine" },
                new Country { Id = 135, Name = "Panama" },
                new Country { Id = 136, Name = "Papua New Guinea" },
                new Country { Id = 137, Name = "Paraguay" },
                new Country { Id = 138, Name = "Peru" },
                new Country { Id = 139, Name = "Philippines" },
                new Country { Id = 140, Name = "Poland" },
                new Country { Id = 141, Name = "Portugal" },
                new Country { Id = 142, Name = "Qatar" },
                new Country { Id = 143, Name = "Romania" },
                new Country { Id = 144, Name = "Russia" },
                new Country { Id = 145, Name = "Rwanda" },
                new Country { Id = 146, Name = "Saint Kitts and Nevis" },
                new Country { Id = 147, Name = "Saint Lucia" },
                new Country { Id = 148, Name = "Saint Vincent and the Grenadines" },
                new Country { Id = 149, Name = "Samoa" },
                new Country { Id = 150, Name = "San Marino" },
                new Country { Id = 151, Name = "Sao Tome and Principe" },
                new Country { Id = 152, Name = "Saudi Arabia" },
                new Country { Id = 153, Name = "Senegal" },
                new Country { Id = 154, Name = "Serbia" },
                new Country { Id = 155, Name = "Seychelles" },
                new Country { Id = 156, Name = "Sierra Leone" },
                new Country { Id = 157, Name = "Singapore" },
                new Country { Id = 158, Name = "Slovakia" },
                new Country { Id = 159, Name = "Slovenia" },
                new Country { Id = 160, Name = "Solomon Islands" },
                new Country { Id = 161, Name = "Somalia" },
                new Country { Id = 162, Name = "South Africa" },
                new Country { Id = 163, Name = "Spain" },
                new Country { Id = 164, Name = "Sri Lanka" },
                new Country { Id = 165, Name = "Sudan" },
                new Country { Id = 166, Name = "Suriname" },
                new Country { Id = 167, Name = "Sweden" },
                new Country { Id = 168, Name = "Switzerland" },
                new Country { Id = 169, Name = "Syria" },
                new Country { Id = 170, Name = "Taiwan" },
                new Country { Id = 171, Name = "Tajikistan" },
                new Country { Id = 172, Name = "Tanzania" },
                new Country { Id = 173, Name = "Thailand" },
                new Country { Id = 174, Name = "Timor-Leste" },
                new Country { Id = 175, Name = "Togo" },
                new Country { Id = 176, Name = "Tonga" },
                new Country { Id = 177, Name = "Trinidad and Tobago" },
                new Country { Id = 178, Name = "Tunisia" },
                new Country { Id = 179, Name = "Turkey" },
                new Country { Id = 180, Name = "Turkmenistan" },
                new Country { Id = 181, Name = "Tuvalu" },
                new Country { Id = 182, Name = "Uganda" },
                new Country { Id = 183, Name = "Ukraine" },
                new Country { Id = 184, Name = "United Arab Emirates" },
                new Country { Id = 185, Name = "United Kingdom" },
                new Country { Id = 186, Name = "United States" },
                new Country { Id = 187, Name = "Uruguay" },
                new Country { Id = 188, Name = "Uzbekistan" },
                new Country { Id = 189, Name = "Vanuatu" },
                new Country { Id = 190, Name = "Vatican City" },
                new Country { Id = 191, Name = "Venezuela" },
                new Country { Id = 192, Name = "Vietnam" },
                new Country { Id = 193, Name = "Yemen" },
                new Country { Id = 194, Name = "Zambia" },
                new Country { Id = 195, Name = "Zimbabwe" }
            };
            ViewBag.Countries = new SelectList(countries, "Id", "Name");
            try
            {
                var userIdLogged = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Get the user from UsersApp table based on UserID
                Users user = await _context.UsersApp.FirstOrDefaultAsync(u => u.UserID == userIdLogged);

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found. Please log in again.");
                    return View(trips);
                }

                if(!ValidateTripName(trips.TripName))
                {
                    ModelState.AddModelError("TripName", "Please insert a trip name with characters between 10 and 45");
                    return View(trips);
                }

                if(!ValidateDescription(trips.Description))
                {
                    ModelState.AddModelError("Description", "Please insert a description with characters between 15 and 300");
                    return View(trips);
                }

                Country country = countries.FirstOrDefault(c => c.Id == int.Parse(trips.Location));

                trips.Location = country.Name;

                // Assign the UserFK based on the found user's Id
                trips.UserFK = user.Id;

                // Create a new Group associated with the Trip
                var newGroup = new Groups
                {
                    GroupId = Guid.NewGuid().ToString(), // Generate a new GUID for GroupId
                    Name = trips.TripName // Set the group name (adjust as needed)
                };

                // Associate the new Group with the Trip
                trips.Group = newGroup;

                // Generate a GUID for the trip
                trips.Id = Guid.NewGuid().ToString();

                // vars auxiliares
                string nomeImagem = "";
                bool haImagem = false;

                if (Banner == null)
                {
                    trips.Banner = "default.webp";
                }
                else
                {
                    // verify MIME types
                    if(!(Banner.ContentType == "image/png" ||
                        Banner.ContentType == "image/jpeg")){
                        trips.Banner = "default.webp";
                    }
                    else
                    {
                        haImagem = true;

                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(Banner.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        trips.Banner = nomeImagem;
                    }
                }



                // Add the trip and the associated group to the context
                _context.Trips.Add(trips);

                //newgroup e  user

                Users_Groups user_groups = new Users_Groups();

                user_groups.UserFK = user.Id;

                user_groups.GroupFK = newGroup.GroupId;

                _context.Add(user_groups);

                // Save changes to the database to ensure trips.Id is generated
                await _context.SaveChangesAsync();


                if (haImagem)
                {
                    // encolher a imagem ao tamanho certo --> fazer pelos alunos
                    // procurar no NuGet

                    // determinar o local de armazenamento da imagem
                    string localizacaoImagem = _webHostEnvironment.WebRootPath;
                    // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens
                    localizacaoImagem = Path.Combine(localizacaoImagem, "images");

                    // será que o local existe?
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }

                    // atribuir ao caminho o nome da imagem
                    localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);

                    // guardar a imagem no Disco Rígido
                    using var stream = new FileStream(
                       localizacaoImagem, FileMode.Create
                       );
                    await Banner.CopyToAsync(stream);
                }

                // Redirect to the index page upon successful save
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle specific database update errors
                ModelState.AddModelError("", $"Database update error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                ModelState.AddModelError("", $"An error occurred while creating the trip: {ex.Message}");
            }

            // If there are validation errors or exceptions, reload necessary data for the view
            return View(trips);
        }
    

    // GET: Trips/Edit/5
    public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips.FindAsync(id);
            if (trips == null)
            {
                return NotFound();
            }

            var countries = new List<Country>
            {
               new Country { Id = 1, Name = "Afghanistan" },
                new Country { Id = 2, Name = "Albania" },
                new Country { Id = 3, Name = "Algeria" },
                new Country { Id = 4, Name = "Andorra" },
                new Country { Id = 5, Name = "Angola" },
                new Country { Id = 6, Name = "Antigua and Barbuda" },
                new Country { Id = 7, Name = "Argentina" },
                new Country { Id = 8, Name = "Armenia" },
                new Country { Id = 9, Name = "Australia" },
                new Country { Id = 10, Name = "Austria" },
                new Country { Id = 11, Name = "Azerbaijan" },
                new Country { Id = 12, Name = "Bahamas" },
                new Country { Id = 13, Name = "Bahrain" },
                new Country { Id = 14, Name = "Bangladesh" },
                new Country { Id = 15, Name = "Barbados" },
                new Country { Id = 16, Name = "Belarus" },
                new Country { Id = 17, Name = "Belgium" },
                new Country { Id = 18, Name = "Belize" },
                new Country { Id = 19, Name = "Benin" },
                new Country { Id = 20, Name = "Bhutan" },
                new Country { Id = 21, Name = "Bolivia" },
                new Country { Id = 22, Name = "Bosnia and Herzegovina" },
                new Country { Id = 23, Name = "Botswana" },
                new Country { Id = 24, Name = "Brazil" },
                new Country { Id = 25, Name = "Brunei" },
                new Country { Id = 26, Name = "Bulgaria" },
                new Country { Id = 27, Name = "Burkina Faso" },
                new Country { Id = 28, Name = "Burundi" },
                new Country { Id = 29, Name = "Cabo Verde" },
                new Country { Id = 30, Name = "Cambodia" },
                new Country { Id = 31, Name = "Cameroon" },
                new Country { Id = 32, Name = "Canada" },
                new Country { Id = 33, Name = "Central African Republic" },
                new Country { Id = 34, Name = "Chad" },
                new Country { Id = 35, Name = "Chile" },
                new Country { Id = 36, Name = "China" },
                new Country { Id = 37, Name = "Colombia" },
                new Country { Id = 38, Name = "Comoros" },
                new Country { Id = 39, Name = "Congo, Democratic Republic of the" },
                new Country { Id = 40, Name = "Congo, Republic of the" },
                new Country { Id = 41, Name = "Costa Rica" },
                new Country { Id = 42, Name = "Croatia" },
                new Country { Id = 43, Name = "Cuba" },
                new Country { Id = 44, Name = "Cyprus" },
                new Country { Id = 45, Name = "Czech Republic" },
                new Country { Id = 46, Name = "Denmark" },
                new Country { Id = 47, Name = "Djibouti" },
                new Country { Id = 48, Name = "Dominica" },
                new Country { Id = 49, Name = "Dominican Republic" },
                new Country { Id = 50, Name = "Ecuador" },
                new Country { Id = 51, Name = "Egypt" },
                new Country { Id = 52, Name = "El Salvador" },
                new Country { Id = 53, Name = "Equatorial Guinea" },
                new Country { Id = 54, Name = "Eritrea" },
                new Country { Id = 55, Name = "Estonia" },
                new Country { Id = 56, Name = "Eswatini" },
                new Country { Id = 57, Name = "Ethiopia" },
                new Country { Id = 58, Name = "Fiji" },
                new Country { Id = 59, Name = "Finland" },
                new Country { Id = 60, Name = "France" },
                new Country { Id = 61, Name = "Gabon" },
                new Country { Id = 62, Name = "Gambia" },
                new Country { Id = 63, Name = "Georgia" },
                new Country { Id = 64, Name = "Germany" },
                new Country { Id = 65, Name = "Ghana" },
                new Country { Id = 66, Name = "Greece" },
                new Country { Id = 67, Name = "Grenada" },
                new Country { Id = 68, Name = "Guatemala" },
                new Country { Id = 69, Name = "Guinea" },
                new Country { Id = 70, Name = "Guinea-Bissau" },
                new Country { Id = 71, Name = "Guyana" },
                new Country { Id = 72, Name = "Haiti" },
                new Country { Id = 73, Name = "Honduras" },
                new Country { Id = 74, Name = "Hungary" },
                new Country { Id = 75, Name = "Iceland" },
                new Country { Id = 76, Name = "India" },
                new Country { Id = 77, Name = "Indonesia" },
                new Country { Id = 78, Name = "Iran" },
                new Country { Id = 79, Name = "Iraq" },
                new Country { Id = 80, Name = "Ireland" },
                new Country { Id = 81, Name = "Israel" },
                new Country { Id = 82, Name = "Italy" },
                new Country { Id = 83, Name = "Jamaica" },
                new Country { Id = 84, Name = "Japan" },
                new Country { Id = 85, Name = "Jordan" },
                new Country { Id = 86, Name = "Kazakhstan" },
                new Country { Id = 87, Name = "Kenya" },
                new Country { Id = 88, Name = "Kiribati" },
                new Country { Id = 89, Name = "Korea, North" },
                new Country { Id = 90, Name = "Korea, South" },
                new Country { Id = 91, Name = "Kosovo" },
                new Country { Id = 92, Name = "Kuwait" },
                new Country { Id = 93, Name = "Kyrgyzstan" },
                new Country { Id = 94, Name = "Laos" },
                new Country { Id = 95, Name = "Latvia" },
                new Country { Id = 96, Name = "Lebanon" },
                new Country { Id = 97, Name = "Lesotho" },
                new Country { Id = 98, Name = "Liberia" },
                new Country { Id = 99, Name = "Libya" },
                new Country { Id = 100, Name = "Liechtenstein" },
                new Country { Id = 101, Name = "Lithuania" },
                new Country { Id = 102, Name = "Luxembourg" },
                new Country { Id = 103, Name = "Madagascar" },
                new Country { Id = 104, Name = "Malawi" },
                new Country { Id = 105, Name = "Malaysia" },
                new Country { Id = 106, Name = "Maldives" },
                new Country { Id = 107, Name = "Mali" },
                new Country { Id = 108, Name = "Malta" },
                new Country { Id = 109, Name = "Marshall Islands" },
                new Country { Id = 110, Name = "Mauritania" },
                new Country { Id = 111, Name = "Mauritius" },
                new Country { Id = 112, Name = "Mexico" },
                new Country { Id = 113, Name = "Micronesia" },
                new Country { Id = 114, Name = "Moldova" },
                new Country { Id = 115, Name = "Monaco" },
                new Country { Id = 116, Name = "Mongolia" },
                new Country { Id = 117, Name = "Montenegro" },
                new Country { Id = 118, Name = "Morocco" },
                new Country { Id = 119, Name = "Mozambique" },
                new Country { Id = 120, Name = "Myanmar (Burma)" },
                new Country { Id = 121, Name = "Namibia" },
                new Country { Id = 122, Name = "Nauru" },
                new Country { Id = 123, Name = "Nepal" },
                new Country { Id = 124, Name = "Netherlands" },
                new Country { Id = 125, Name = "New Zealand" },
                new Country { Id = 126, Name = "Nicaragua" },
                new Country { Id = 127, Name = "Niger" },
                new Country { Id = 128, Name = "Nigeria" },
                new Country { Id = 129, Name = "North Macedonia" },
                new Country { Id = 130, Name = "Norway" },
                new Country { Id = 131, Name = "Oman" },
                new Country { Id = 132, Name = "Pakistan" },
                new Country { Id = 133, Name = "Palau" },
                new Country { Id = 134, Name = "Palestine" },
                new Country { Id = 135, Name = "Panama" },
                new Country { Id = 136, Name = "Papua New Guinea" },
                new Country { Id = 137, Name = "Paraguay" },
                new Country { Id = 138, Name = "Peru" },
                new Country { Id = 139, Name = "Philippines" },
                new Country { Id = 140, Name = "Poland" },
                new Country { Id = 141, Name = "Portugal" },
                new Country { Id = 142, Name = "Qatar" },
                new Country { Id = 143, Name = "Romania" },
                new Country { Id = 144, Name = "Russia" },
                new Country { Id = 145, Name = "Rwanda" },
                new Country { Id = 146, Name = "Saint Kitts and Nevis" },
                new Country { Id = 147, Name = "Saint Lucia" },
                new Country { Id = 148, Name = "Saint Vincent and the Grenadines" },
                new Country { Id = 149, Name = "Samoa" },
                new Country { Id = 150, Name = "San Marino" },
                new Country { Id = 151, Name = "Sao Tome and Principe" },
                new Country { Id = 152, Name = "Saudi Arabia" },
                new Country { Id = 153, Name = "Senegal" },
                new Country { Id = 154, Name = "Serbia" },
                new Country { Id = 155, Name = "Seychelles" },
                new Country { Id = 156, Name = "Sierra Leone" },
                new Country { Id = 157, Name = "Singapore" },
                new Country { Id = 158, Name = "Slovakia" },
                new Country { Id = 159, Name = "Slovenia" },
                new Country { Id = 160, Name = "Solomon Islands" },
                new Country { Id = 161, Name = "Somalia" },
                new Country { Id = 162, Name = "South Africa" },
                new Country { Id = 163, Name = "Spain" },
                new Country { Id = 164, Name = "Sri Lanka" },
                new Country { Id = 165, Name = "Sudan" },
                new Country { Id = 166, Name = "Suriname" },
                new Country { Id = 167, Name = "Sweden" },
                new Country { Id = 168, Name = "Switzerland" },
                new Country { Id = 169, Name = "Syria" },
                new Country { Id = 170, Name = "Taiwan" },
                new Country { Id = 171, Name = "Tajikistan" },
                new Country { Id = 172, Name = "Tanzania" },
                new Country { Id = 173, Name = "Thailand" },
                new Country { Id = 174, Name = "Timor-Leste" },
                new Country { Id = 175, Name = "Togo" },
                new Country { Id = 176, Name = "Tonga" },
                new Country { Id = 177, Name = "Trinidad and Tobago" },
                new Country { Id = 178, Name = "Tunisia" },
                new Country { Id = 179, Name = "Turkey" },
                new Country { Id = 180, Name = "Turkmenistan" },
                new Country { Id = 181, Name = "Tuvalu" },
                new Country { Id = 182, Name = "Uganda" },
                new Country { Id = 183, Name = "Ukraine" },
                new Country { Id = 184, Name = "United Arab Emirates" },
                new Country { Id = 185, Name = "United Kingdom" },
                new Country { Id = 186, Name = "United States" },
                new Country { Id = 187, Name = "Uruguay" },
                new Country { Id = 188, Name = "Uzbekistan" },
                new Country { Id = 189, Name = "Vanuatu" },
                new Country { Id = 190, Name = "Vatican City" },
                new Country { Id = 191, Name = "Venezuela" },
                new Country { Id = 192, Name = "Vietnam" },
                new Country { Id = 193, Name = "Yemen" },
                new Country { Id = 194, Name = "Zambia" },
                new Country { Id = 195, Name = "Zimbabwe" }
            };
            ViewBag.Countries = new SelectList(countries, "Id", "Name");
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", trips.GroupId);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", trips.UserFK);
            return View(trips);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TripName,Description,Category,Transport,InicialBudget,FinalBudget,Closed, Location")] Trips trips, IFormFile? Banner)
        {
            var countries = new List<Country>
            {
               new Country { Id = 1, Name = "Afghanistan" },
                new Country { Id = 2, Name = "Albania" },
                new Country { Id = 3, Name = "Algeria" },
                new Country { Id = 4, Name = "Andorra" },
                new Country { Id = 5, Name = "Angola" },
                new Country { Id = 6, Name = "Antigua and Barbuda" },
                new Country { Id = 7, Name = "Argentina" },
                new Country { Id = 8, Name = "Armenia" },
                new Country { Id = 9, Name = "Australia" },
                new Country { Id = 10, Name = "Austria" },
                new Country { Id = 11, Name = "Azerbaijan" },
                new Country { Id = 12, Name = "Bahamas" },
                new Country { Id = 13, Name = "Bahrain" },
                new Country { Id = 14, Name = "Bangladesh" },
                new Country { Id = 15, Name = "Barbados" },
                new Country { Id = 16, Name = "Belarus" },
                new Country { Id = 17, Name = "Belgium" },
                new Country { Id = 18, Name = "Belize" },
                new Country { Id = 19, Name = "Benin" },
                new Country { Id = 20, Name = "Bhutan" },
                new Country { Id = 21, Name = "Bolivia" },
                new Country { Id = 22, Name = "Bosnia and Herzegovina" },
                new Country { Id = 23, Name = "Botswana" },
                new Country { Id = 24, Name = "Brazil" },
                new Country { Id = 25, Name = "Brunei" },
                new Country { Id = 26, Name = "Bulgaria" },
                new Country { Id = 27, Name = "Burkina Faso" },
                new Country { Id = 28, Name = "Burundi" },
                new Country { Id = 29, Name = "Cabo Verde" },
                new Country { Id = 30, Name = "Cambodia" },
                new Country { Id = 31, Name = "Cameroon" },
                new Country { Id = 32, Name = "Canada" },
                new Country { Id = 33, Name = "Central African Republic" },
                new Country { Id = 34, Name = "Chad" },
                new Country { Id = 35, Name = "Chile" },
                new Country { Id = 36, Name = "China" },
                new Country { Id = 37, Name = "Colombia" },
                new Country { Id = 38, Name = "Comoros" },
                new Country { Id = 39, Name = "Congo, Democratic Republic of the" },
                new Country { Id = 40, Name = "Congo, Republic of the" },
                new Country { Id = 41, Name = "Costa Rica" },
                new Country { Id = 42, Name = "Croatia" },
                new Country { Id = 43, Name = "Cuba" },
                new Country { Id = 44, Name = "Cyprus" },
                new Country { Id = 45, Name = "Czech Republic" },
                new Country { Id = 46, Name = "Denmark" },
                new Country { Id = 47, Name = "Djibouti" },
                new Country { Id = 48, Name = "Dominica" },
                new Country { Id = 49, Name = "Dominican Republic" },
                new Country { Id = 50, Name = "Ecuador" },
                new Country { Id = 51, Name = "Egypt" },
                new Country { Id = 52, Name = "El Salvador" },
                new Country { Id = 53, Name = "Equatorial Guinea" },
                new Country { Id = 54, Name = "Eritrea" },
                new Country { Id = 55, Name = "Estonia" },
                new Country { Id = 56, Name = "Eswatini" },
                new Country { Id = 57, Name = "Ethiopia" },
                new Country { Id = 58, Name = "Fiji" },
                new Country { Id = 59, Name = "Finland" },
                new Country { Id = 60, Name = "France" },
                new Country { Id = 61, Name = "Gabon" },
                new Country { Id = 62, Name = "Gambia" },
                new Country { Id = 63, Name = "Georgia" },
                new Country { Id = 64, Name = "Germany" },
                new Country { Id = 65, Name = "Ghana" },
                new Country { Id = 66, Name = "Greece" },
                new Country { Id = 67, Name = "Grenada" },
                new Country { Id = 68, Name = "Guatemala" },
                new Country { Id = 69, Name = "Guinea" },
                new Country { Id = 70, Name = "Guinea-Bissau" },
                new Country { Id = 71, Name = "Guyana" },
                new Country { Id = 72, Name = "Haiti" },
                new Country { Id = 73, Name = "Honduras" },
                new Country { Id = 74, Name = "Hungary" },
                new Country { Id = 75, Name = "Iceland" },
                new Country { Id = 76, Name = "India" },
                new Country { Id = 77, Name = "Indonesia" },
                new Country { Id = 78, Name = "Iran" },
                new Country { Id = 79, Name = "Iraq" },
                new Country { Id = 80, Name = "Ireland" },
                new Country { Id = 81, Name = "Israel" },
                new Country { Id = 82, Name = "Italy" },
                new Country { Id = 83, Name = "Jamaica" },
                new Country { Id = 84, Name = "Japan" },
                new Country { Id = 85, Name = "Jordan" },
                new Country { Id = 86, Name = "Kazakhstan" },
                new Country { Id = 87, Name = "Kenya" },
                new Country { Id = 88, Name = "Kiribati" },
                new Country { Id = 89, Name = "Korea, North" },
                new Country { Id = 90, Name = "Korea, South" },
                new Country { Id = 91, Name = "Kosovo" },
                new Country { Id = 92, Name = "Kuwait" },
                new Country { Id = 93, Name = "Kyrgyzstan" },
                new Country { Id = 94, Name = "Laos" },
                new Country { Id = 95, Name = "Latvia" },
                new Country { Id = 96, Name = "Lebanon" },
                new Country { Id = 97, Name = "Lesotho" },
                new Country { Id = 98, Name = "Liberia" },
                new Country { Id = 99, Name = "Libya" },
                new Country { Id = 100, Name = "Liechtenstein" },
                new Country { Id = 101, Name = "Lithuania" },
                new Country { Id = 102, Name = "Luxembourg" },
                new Country { Id = 103, Name = "Madagascar" },
                new Country { Id = 104, Name = "Malawi" },
                new Country { Id = 105, Name = "Malaysia" },
                new Country { Id = 106, Name = "Maldives" },
                new Country { Id = 107, Name = "Mali" },
                new Country { Id = 108, Name = "Malta" },
                new Country { Id = 109, Name = "Marshall Islands" },
                new Country { Id = 110, Name = "Mauritania" },
                new Country { Id = 111, Name = "Mauritius" },
                new Country { Id = 112, Name = "Mexico" },
                new Country { Id = 113, Name = "Micronesia" },
                new Country { Id = 114, Name = "Moldova" },
                new Country { Id = 115, Name = "Monaco" },
                new Country { Id = 116, Name = "Mongolia" },
                new Country { Id = 117, Name = "Montenegro" },
                new Country { Id = 118, Name = "Morocco" },
                new Country { Id = 119, Name = "Mozambique" },
                new Country { Id = 120, Name = "Myanmar (Burma)" },
                new Country { Id = 121, Name = "Namibia" },
                new Country { Id = 122, Name = "Nauru" },
                new Country { Id = 123, Name = "Nepal" },
                new Country { Id = 124, Name = "Netherlands" },
                new Country { Id = 125, Name = "New Zealand" },
                new Country { Id = 126, Name = "Nicaragua" },
                new Country { Id = 127, Name = "Niger" },
                new Country { Id = 128, Name = "Nigeria" },
                new Country { Id = 129, Name = "North Macedonia" },
                new Country { Id = 130, Name = "Norway" },
                new Country { Id = 131, Name = "Oman" },
                new Country { Id = 132, Name = "Pakistan" },
                new Country { Id = 133, Name = "Palau" },
                new Country { Id = 134, Name = "Palestine" },
                new Country { Id = 135, Name = "Panama" },
                new Country { Id = 136, Name = "Papua New Guinea" },
                new Country { Id = 137, Name = "Paraguay" },
                new Country { Id = 138, Name = "Peru" },
                new Country { Id = 139, Name = "Philippines" },
                new Country { Id = 140, Name = "Poland" },
                new Country { Id = 141, Name = "Portugal" },
                new Country { Id = 142, Name = "Qatar" },
                new Country { Id = 143, Name = "Romania" },
                new Country { Id = 144, Name = "Russia" },
                new Country { Id = 145, Name = "Rwanda" },
                new Country { Id = 146, Name = "Saint Kitts and Nevis" },
                new Country { Id = 147, Name = "Saint Lucia" },
                new Country { Id = 148, Name = "Saint Vincent and the Grenadines" },
                new Country { Id = 149, Name = "Samoa" },
                new Country { Id = 150, Name = "San Marino" },
                new Country { Id = 151, Name = "Sao Tome and Principe" },
                new Country { Id = 152, Name = "Saudi Arabia" },
                new Country { Id = 153, Name = "Senegal" },
                new Country { Id = 154, Name = "Serbia" },
                new Country { Id = 155, Name = "Seychelles" },
                new Country { Id = 156, Name = "Sierra Leone" },
                new Country { Id = 157, Name = "Singapore" },
                new Country { Id = 158, Name = "Slovakia" },
                new Country { Id = 159, Name = "Slovenia" },
                new Country { Id = 160, Name = "Solomon Islands" },
                new Country { Id = 161, Name = "Somalia" },
                new Country { Id = 162, Name = "South Africa" },
                new Country { Id = 163, Name = "Spain" },
                new Country { Id = 164, Name = "Sri Lanka" },
                new Country { Id = 165, Name = "Sudan" },
                new Country { Id = 166, Name = "Suriname" },
                new Country { Id = 167, Name = "Sweden" },
                new Country { Id = 168, Name = "Switzerland" },
                new Country { Id = 169, Name = "Syria" },
                new Country { Id = 170, Name = "Taiwan" },
                new Country { Id = 171, Name = "Tajikistan" },
                new Country { Id = 172, Name = "Tanzania" },
                new Country { Id = 173, Name = "Thailand" },
                new Country { Id = 174, Name = "Timor-Leste" },
                new Country { Id = 175, Name = "Togo" },
                new Country { Id = 176, Name = "Tonga" },
                new Country { Id = 177, Name = "Trinidad and Tobago" },
                new Country { Id = 178, Name = "Tunisia" },
                new Country { Id = 179, Name = "Turkey" },
                new Country { Id = 180, Name = "Turkmenistan" },
                new Country { Id = 181, Name = "Tuvalu" },
                new Country { Id = 182, Name = "Uganda" },
                new Country { Id = 183, Name = "Ukraine" },
                new Country { Id = 184, Name = "United Arab Emirates" },
                new Country { Id = 185, Name = "United Kingdom" },
                new Country { Id = 186, Name = "United States" },
                new Country { Id = 187, Name = "Uruguay" },
                new Country { Id = 188, Name = "Uzbekistan" },
                new Country { Id = 189, Name = "Vanuatu" },
                new Country { Id = 190, Name = "Vatican City" },
                new Country { Id = 191, Name = "Venezuela" },
                new Country { Id = 192, Name = "Vietnam" },
                new Country { Id = 193, Name = "Yemen" },
                new Country { Id = 194, Name = "Zambia" },
                new Country { Id = 195, Name = "Zimbabwe" }
            };
            ViewBag.Countries = new SelectList(countries, "Id", "Name");

            if (id != trips.Id)
            {
                return NotFound();
            }

            if (!ValidateTripName(trips.TripName))
            {
                ModelState.AddModelError("TripName", "Please insert a trip name with characters between 10 and 45");
                return View(trips);
            }

            if (!ValidateDescription(trips.Description))
            {
                ModelState.AddModelError("Description", "Please insert a description with characters between 15 and 300");
                return View(trips);
            }

            Country country = countries.FirstOrDefault(c => c.Id == int.Parse(trips.Location));

            trips.Location = country.Name;


            // Vars Aux
            string nomeImagem = "";
            bool haImagem = false;

            if (Banner != null)
            {
                Console.WriteLine("2");
                if (!(Banner.ContentType == "image/png" ||
                                        Banner.ContentType == "image/jpeg"))
                {
                    Console.WriteLine("4");
                    trips.Banner = "default.webp";
                }
                else
                {
                    Console.WriteLine("3");
                    haImagem = true;

                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensaoImagem = Path.GetExtension(Banner.FileName).ToLowerInvariant();
                    nomeImagem += extensaoImagem;
                    trips.Banner = nomeImagem;
                }
            }

                try
                {

                var originalTrip = await _context.Trips.AsNoTracking().FirstOrDefaultAsync(x => x.Id == trips.Id);
                if (originalTrip == null)
                {
                    return NotFound();
                }

                if(haImagem == false)
                {
                    trips.Banner = originalTrip.Banner;
                }
                // Preserve non-editable fields
                trips.GroupId = originalTrip.GroupId;
                trips.UserFK = originalTrip.UserFK;


                // Attach the entity and set its state to Modified
                _context.Entry(trips).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                if (haImagem)
                {
                    // encolher a imagem ao tamanho certo --> fazer pelos alunos
                    // procurar no NuGet

                    // determinar o local de armazenamento da imagem
                    string localizacaoImagem = _webHostEnvironment.WebRootPath;
                    // adicionar à raiz da parte web, o nome da pasta onde queremos guardar as imagens
                    localizacaoImagem = Path.Combine(localizacaoImagem, "images");

                    // será que o local existe?
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }

                    // atribuir ao caminho o nome da imagem
                    localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);

                    // guardar a imagem no Disco Rígido
                    using var stream = new FileStream(
                       localizacaoImagem, FileMode.Create
                       );
                    await Banner.CopyToAsync(stream);
                }
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripsExists(trips.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", trips.GroupId);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", trips.UserFK);
            return View(trips);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips
                .Include(t => t.Group)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trips == null)
            {
                return NotFound();
            }

            return View(trips);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trips = await _context.Trips.FindAsync(id);

            if (trips != null)
            {
                _context.Trips.Remove(trips);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> UserTravels(string id)
        {
            int newId = int.Parse(id);

            Users user = _context.UsersApp.FirstOrDefault(u => u.Id == newId);
            ViewData["UserName"] = user.Name;

            var applicationDbContext = _context.Trips.Where(t => t.UserFK == user.Id).Include(t => t.Group).Include(t => t.User);
         
            return View("UserTravels", await applicationDbContext.ToListAsync());
        }

        private bool TripsExists(string id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }

        private bool ValidateTripName(string tripName)
        {
            tripName = tripName.Trim();
            var length = tripName.Length;

            if (length >= 10 && length <= 45)
            {
                return true;
            }

            return false;
        }

        private bool ValidateDescription(string description)
        {
            description = description.Trim();
            var length = description.Length;

            if (length >= 15 && length <= 300)
            {
                return true;
            }

            return false;
        }
    }

    // inner class country
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
