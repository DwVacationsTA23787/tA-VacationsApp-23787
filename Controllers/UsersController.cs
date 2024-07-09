using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dw23787.Data;
using Dw23787.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Dw23787.Controllers
{

    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Users user = _context.UsersApp.FirstOrDefault(u => u.UserID == userId);

            if (user.isAdmin == false)
            {
                return View(await _context.UsersApp.Where(u => u.Id == user.Id).ToListAsync());
            }

             return View(await _context.UsersApp.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.UsersApp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,DataNascimento,Gender,Phone,Nationality,Quote,Password")] Users users, IFormFile ProfilePicture)
        {
            try
            {
                IdentityUser existingUser = await _userManager.FindByEmailAsync(users.Email);

                if (existingUser != null)
                {
                    return BadRequest("Email already has an account");
                }

                IdentityUser newUser = new IdentityUser
                {
                    UserName = users.Email,
                    Email = users.Email,
                    Id = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, users.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // Create the application user
                Users userApp = new Users
                {
                    Email = users.Email,
                    DataNascimento = users.DataNascimento,
                    Name = users.Name,
                    Gender = users.Gender,
                    Phone = users.Phone,
                    UserID = newUser.Id,
                    Nationality = users.Nationality,
                    Password = newUser.PasswordHash,
                    Quote = users.Quote
                };

                // Calculate age
                DateTime today = DateTime.Today;
                int age = today.Year - users.DataNascimento.Year;
                userApp.Age = age;

                string nomeImagem = "";
                bool haImagem = false;

                //add image

                if (ProfilePicture != null)
                {
                    if (!(ProfilePicture.ContentType == "image/png" ||
                                          ProfilePicture.ContentType == "image/jpeg"))
                    {
                        userApp.ProfilePicture = "default.webp"; // set default user logo.
                    }
                    else
                    {
                        haImagem = true;

                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(ProfilePicture.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        userApp.ProfilePicture = nomeImagem;
                    }
                }


                _context.UsersApp.Add(userApp);

                await _context.SaveChangesAsync(); // Completes the transaction

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
                    await ProfilePicture.CopyToAsync(stream);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
 
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.UsersApp.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,DataNascimento,Age,Gender,Phone,UserID,Password,Nationality,Quote,isAdmin")] Users users, IFormFile ProfilePicture)
        {
            if (id != users.Id)
            {
                return NotFound();
            }

            // Vars Aux
            string nomeImagem = "";
            bool haImagem = false;

            if (ProfilePicture != null)
            {
                Console.WriteLine("2");
                if (!(ProfilePicture.ContentType == "image/png" ||
                                        ProfilePicture.ContentType == "image/jpeg"))
                {
                    Console.WriteLine("4");
                    users.ProfilePicture = "default.webp"; // set default user logo.
                }
                else
                {
                    Console.WriteLine("3");
                    haImagem = true;

                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensaoImagem = Path.GetExtension(ProfilePicture.FileName).ToLowerInvariant();
                    nomeImagem += extensaoImagem;
                    users.ProfilePicture = nomeImagem;
                }
            }

                try
                {
                    _context.Update(users);
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
                        await ProfilePicture.CopyToAsync(stream);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.UsersApp
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Users userLogged = _context.UsersApp.FirstOrDefault(u => u.UserID == userId);

            if (userLogged.Id == id)
            {
                ModelState.AddModelError(string.Empty, "You Cant delete yourself!");
                return View(userLogged);
            }
            var users = await _context.UsersApp.FindAsync(id);
            var userNet = await _userManager.FindByIdAsync(users.UserID);




            if (users != null)
            {
                if (userNet != null)
                {
                    var result = await _userManager.DeleteAsync(userNet);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "User not deleted!");
                        return View(users);
                    }

                    _context.UsersApp.Remove(users);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }



            _context.UsersApp.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.UsersApp.Any(e => e.Id == id);
        }
    }
}
