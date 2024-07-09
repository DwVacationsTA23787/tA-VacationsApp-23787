using Dw23787.Data;
using Dw23787.Models;
using Dw23787.Models.dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using static Dw23787.Models.Trips;

namespace Dw23787.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1 : ControllerBase
    {
        public ApplicationDbContext _Context;

        public UserManager<IdentityUser> _userManager;

        public SignInManager<IdentityUser> _signInManager;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public V1(ApplicationDbContext applicationDbContext, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _Context = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }


        //Creates

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> CreateUser([FromForm] Users user, IFormFile profilePicture)
        {
            if (user == null)
            {
                return BadRequest("Please insert a user");
            }

            try
            {
                IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest("Email already has an account");
                }

                IdentityUser newUser = new IdentityUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    Id = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Create the application user
                Users userApp = new Users
                {
                    Email = user.Email,
                    DataNascimento = user.DataNascimento,
                    Name = user.Name,
                    Gender = user.Gender,
                    Phone = user.Phone,
                    UserID = newUser.Id,
                    Nationality = user.Nationality,
                    Password = newUser.PasswordHash,
                    Quote = user.Quote
                };

                // Calculate age
                DateTime today = DateTime.Today;
                int age = today.Year - user.DataNascimento.Year;
                userApp.Age = age;

                string nomeImagem = "";
                bool haImagem = false;

                //add image

                if (profilePicture != null)
                {
                    if (!(profilePicture.ContentType == "image/png" ||
                                          profilePicture.ContentType == "image/jpeg"))
                    {
                        userApp.ProfilePicture = "default.webp"; // set default user logo.
                    }
                    else
                    {
                        haImagem = true;

                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        userApp.ProfilePicture = nomeImagem;
                    }
                }


                _Context.UsersApp.Add(userApp);

                await _Context.SaveChangesAsync(); // Completes the transaction

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
                    await profilePicture.CopyToAsync(stream);
                }

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateTrip")]
        public async Task<ActionResult> CreateTrip([FromQuery] int id, [FromForm] CreateTripDto tripDto, IFormFile banner)
        {
            try
            {
                // Get the user from UsersApp table based on UserID
                Users user = await _Context.UsersApp.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return BadRequest("Please insert a valid user");
                }

                var trip = new Trips
                {
                    TripName = tripDto.TripName,
                    Description = tripDto.Description,
                    Location = tripDto.Location,
                    Category = Enum.Parse<TripCategory>(tripDto.Category),
                    Transport = Enum.Parse<TripTransport>(tripDto.Transport),
                    InicialBudget = tripDto.InicialBudget,
                    FinalBudget = tripDto.FinalBudget,
                    UserFK = user.Id
                };

                // Assign the UserFK based on the found user's Id
                trip.UserFK = user.Id;

                // Create a new Group associated with the Trip
                var newGroup = new Groups
                {
                    GroupId = Guid.NewGuid().ToString(), // Generate a new GUID for GroupId
                    Name = trip.TripName // Set the group name (adjust as needed)
                };

                // Associate the new Group with the Trip
                trip.Group = newGroup;

                // Generate a GUID for the trip
                trip.Id = Guid.NewGuid().ToString();

                // vars aux
                string nomeImagem = "";
                bool haImagem = false;

                // Handle banner file
                if (banner == null || banner.Length == 0)
                {
                    return BadRequest("Please insert a Banner");
                }else
                {
                    // verify MIME types
                    if (!(banner.ContentType == "image/png" ||
                        banner.ContentType == "image/jpeg"))
                    {
                        trip.Banner = "default.webp";
                    }
                    else
                    {
                        haImagem = true;

                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(banner.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        trip.Banner = nomeImagem;
                    }
                }

             

                // Add the trip and the associated group to the context
                _Context.Trips.Add(trip);

                //newgroup e  user

                Users_Groups user_groups = new Users_Groups();

                user_groups.UserFK = user.Id;

                user_groups.GroupFK = newGroup.GroupId;

                _Context.Add(user_groups);

                // Save changes to the database to ensure trips.Id is generated
                await _Context.SaveChangesAsync();

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
                    await banner.CopyToAsync(stream);
                }

                return Ok("Trip created successfully");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Database update error occurred");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while creating the trip");
            }
        }


        [HttpGet]
        [Route("LogIn")]
        public async Task<ActionResult> LogInUser([FromQuery] string email, [FromQuery] string password, [FromQuery] bool remainder)
        {
            try
            {

                IdentityUser resultUser = await _userManager.FindByEmailAsync(email);

                if (resultUser != null)
                {

                    PasswordVerificationResult passWorks = new PasswordHasher<IdentityUser>().VerifyHashedPassword(resultUser, resultUser.PasswordHash, password);

                    if (passWorks == PasswordVerificationResult.Success)
                    {

                        await _signInManager.SignInAsync(resultUser, remainder); // 'remainder' determines if the user should be remembered for 14 days.


                        Users user = _Context.UsersApp.FirstOrDefault(u => u.UserID == resultUser.Id);

                        if (user != null)
                        {
                            // user to DTO
                            Usersdto userdto = new Usersdto
                            {
                                Id = user.Id,
                                Age = user.Age,
                                Email = email,
                                Gender = user.Gender,
                                DataNascimento = user.DataNascimento,
                                Phone = user.Phone,
                                Name = user.Name,
                                ProfilePicture = user.ProfilePicture,
                                Quote = user.Quote,
                            };

                            return Ok(userdto);
                        }
                        else
                        {
                            return BadRequest("User details not found");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid password");
                    }
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return exception message
            }
        }



        [HttpGet]
        [Route("numUsers")]
        public ActionResult NumberUsers()
        {
            // Podemos utilizar esta tabela como a do ASP.netUsers pois ambas devem ter a mesma quantidade de utilizadores.
            //var num = _Context.Users.Count();
            var num = _Context.UsersApp.Count();
            return Ok(num);
        }


        [HttpGet]
        [Route("numTrips")]
        public ActionResult NumberTrips()
        {
            var num = _Context.Trips.Count();
            return Ok(num);
        }


        [HttpGet]
        [Route("TravelCards")]
        public ActionResult GetTravelCards([FromQuery] int id, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string category, [FromQuery] string? search = null)
        {
            int offset = (page - 1) * pageSize;

            // Base query
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
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.TripName.Contains(search) || t.Description.Contains(search));
            }

            // Filter out trips where UserFK matches the provided id
            query = query.Where(t => t.UserFK != id);

            // Get the total items
            int totalItems = query.Count();

            // Apply pagination
            var travelCards = query
                .OrderBy(t => t.Id) // Order by ID for consistency
                .Skip(offset)
                .Take(pageSize)
                .Select(t => new TravelCardViewModel
                {
                    Id = t.Id,
                    Name = t.TripName,
                    Description = t.Description,
                    Banner = t.Banner ?? ""
                })
                .ToList();

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Prepare the response
            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                TravelCards = travelCards
            };

            return Ok(response);
        }





        [HttpGet]
        [Route("TripDetail")]
        public async Task<ActionResult<TripDto>> TripDetailsAsync([FromQuery] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _Context.Trips
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trip == null)
            {
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

            return Ok(tripDto);
        }


        [HttpGet]
        [Route("UserTrips")]
        public async Task<ActionResult> GetUserTrips([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _Context.UsersApp.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // user to DTO
            var userDto = new Usersdto
            {
                Id = user.Id,
                Age = user.Age,
                Email = user.Email,
                Gender = user.Gender,
                DataNascimento = user.DataNascimento,
                Phone = user.Phone,
                Name = user.Name,
                ProfilePicture = user.ProfilePicture,
                Quote = user.Quote,
            };

            var tripsList = await _Context.Trips
                               .Where(t => t.UserFK == id)
                               .ToListAsync();

            var result = new
            {
                user = userDto,
                trips = tripsList,
            };

            return Ok(result);
        }


        [HttpGet]
        [Route("NumberUserTrips")]
        public async Task<ActionResult<int>> GetNumberUserTrips([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

                var tripsCount = await _Context.Trips
                                   .Where(t => t.UserFK == id)
                                   .CountAsync();

            return Ok(tripsCount);
        }


        [HttpGet]
        [Route("GetUser")]
        public ActionResult<int> GetUser([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            Users user = _Context.UsersApp.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                // user to DTO
                Usersdto userdto = new Usersdto
                {
                    Id = user.Id,
                    Age = user.Age,
                    Email = user.Email,
                    Gender = user.Gender,
                    DataNascimento = user.DataNascimento,
                    Phone = user.Phone,
                    Name = user.Name,
                    ProfilePicture = user.ProfilePicture,
                    Quote = user.Quote,
                };

                return Ok(userdto);
            }

            return BadRequest("User not found");

        }

        [HttpPost]
        [Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] Usersdto updatedUserDto)
        {
            try
            {
                if (id <= 0 || updatedUserDto == null)
                {
                    return BadRequest("Invalid user ID or data.");
                }

                Users user = _Context.UsersApp.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Update user entity with updated data
                user.Age = updatedUserDto.Age;
                user.Email = updatedUserDto.Email;
                user.Gender = updatedUserDto.Gender;
                user.DataNascimento = updatedUserDto.DataNascimento;
                user.Phone = updatedUserDto.Phone;
                user.Name = updatedUserDto.Name;
                user.ProfilePicture = updatedUserDto.ProfilePicture;
                user.Quote = updatedUserDto.Quote;

                _Context.UsersApp.Update(user);
                _Context.SaveChanges();

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetAllTripsForUser")]
        public async Task<ActionResult> GetAllTripsForUser([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _Context.UsersApp.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }


            if(user.isAdmin == false)
            {

               var tripsList = await _Context.Trips
                               .Where(t => t.UserFK == id)
                               .ToListAsync();
                return Ok(tripsList);

            }
            else
            {
                var tripsList = await _Context.Trips
                              .ToListAsync();

                return Ok(tripsList);
            }
        }


        [HttpGet]
        [Route("GetAllGroupsForUser")]
        public async Task<ActionResult> GetAllGroupsForUser([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _Context.UsersApp.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.isAdmin == false)
            {

            // Retrieve all groups where the user is an admin
            var adminGroups = await _Context.GroupAdmins
                .Where(ug => ug.UserFK == id && ug.UserAdmin)
                .Select(ug => ug.GroupFK)
                .ToListAsync();

            // Query to get groups with the count of users in each group
            var groupsWithUserCount = await _Context.Groups
                .Where(g => adminGroups.Contains(g.GroupId))
                .Select(g => new
                {
                    GroupId = g.GroupId,
                    GroupName = g.Name,
                    UserCount = _Context.GroupAdmins.Count(ug => ug.GroupFK == g.GroupId)
                })
                .ToListAsync();

                return Ok(groupsWithUserCount);
            }
            else
            {
                var groups = await _Context.Groups
                .Select(g => new
                {
                    GroupId = g.GroupId,
                    GroupName = g.Name,
                    UserCount = _Context.GroupAdmins.Count(ug => ug.GroupFK == g.GroupId)
                })
                .ToListAsync();

                return Ok(groups);
            }
        }


        [HttpPost]
        [Route("AddUserToGroup")]
        public IActionResult AddUserToGroup([FromQuery] string groupId, [FromQuery] int userId)
        {
            if (string.IsNullOrEmpty(groupId) || userId <= 0)
            {
                return BadRequest("Invalid groupId or userId");
            }

            try
            {
                // Check group and user
                var group = _Context.Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (group == null)
                {
                    return NotFound("Group not found");
                }

                var user = _Context.UsersApp.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                //check if user is already in the group
                var existingUserGroup = _Context.GroupAdmins
                    .FirstOrDefault(ug => ug.UserFK == userId && ug.GroupFK == groupId);

                if (existingUserGroup != null)
                {
                    return Ok("User already belongs to this group"); // acho que nao quero conflict
                }

                
                var newUserGroup = new Users_Groups
                {
                    UserFK = userId,
                    GroupFK = groupId
                };

                _Context.GroupAdmins.Add(newUserGroup);
                _Context.SaveChanges();

                return Ok("User successfully added to group");


            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


            return Ok("");
        }

        // Aux functions

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
