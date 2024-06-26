using Dw23787.Data;
using Dw23787.Models;
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
        public ActionResult CreateUser([FromBody] Users user)
        {
            IdentityUser resultUser;
            if (user == null)
            {
                return BadRequest("Please insert a user");
            }

            try
            {
                resultUser = _userManager.FindByEmailAsync(user.Email).Result;

                if (resultUser != null)
                {
                    return BadRequest("Email already have an account");
                }

                resultUser = new IdentityUser();
                resultUser.Email = user.Email;
                resultUser.PasswordHash = null;
                resultUser.Id = Guid.NewGuid().ToString();
                Console.WriteLine(resultUser.Id);
                Console.WriteLine(resultUser.Email);
                return Ok("Ola");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("LogIn")]
        public ActionResult LogInUser()
        {
            return Ok("Ola");
        }
    }
}
