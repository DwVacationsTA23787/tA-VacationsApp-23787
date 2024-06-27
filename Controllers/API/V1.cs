using Dw23787.Data;
using Dw23787.Models;
using Dw23787.Models.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dw23787.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1 : ControllerBase
    {
        public ApplicationDbContext _Context;

        public UserManager<IdentityUser> _userManager;

        public SignInManager<IdentityUser> _signInManager;


        public V1(ApplicationDbContext applicationDbContext, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
            _Context = applicationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> CreateUser([FromBody] Users user)
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
                    ProfilePicture = user.ProfilePicture,
                    UserID = newUser.Id,
                    Password = newUser.PasswordHash
                };

                // Calculate age
                DateTime today = DateTime.Today;
                int age = today.Year - user.DataNascimento.Year;
                userApp.Age = age;

                _Context.UsersApp.Add(userApp);

                await _Context.SaveChangesAsync(); // Completes the transaction

                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                                Age = user.Age,
                                Email = email,
                                Gender = user.Gender,
                                DataNascimento = user.DataNascimento,
                                Phone = user.Phone,
                                Name = user.Name,
                                ProfilePicture = user.ProfilePicture
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
    }
}
