// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Dw23787.Data;
using Dw23787.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dw23787.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext applicationDbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Birthdate is required")]
            [DataType(DataType.Date)]
            [Display(Name = "Birthdate")]
            public DateOnly DataNascimento { get; set; }

            [Required(ErrorMessage = "Phone is required")]
            [Display(Name = "Phone")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Gender is required")]
            [Display(Name = "Gender")]
            public string Gender { get; set; }


            [Required(ErrorMessage = "Nacionality is required")]
            [Display(Name = "Nacionality")]
            public string Nacionality { get; set; }

            [Required(ErrorMessage = "Please Write a little quote about yourself")]
            [Display(Name = "Quote")]
            public string Quote { get; set; }

            [Display(Name = "ProfilePicture")]
            public IFormFile profilePicture { get; set; }


        }


        public async Task OnGetAsync(string returnUrl = null)
        {

            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = CreateUser(); // Ensure CreateUser() creates a new user instance

                // Set user email and username
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                // Create user with password
                var result = await _userManager.CreateAsync(user, Input.Password);

                // vars aux
                string nomeImagem = "";
                bool haImagem = false;

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    try
                    {
                        Users userApp = new Users
                        {
                            Email = Input.Email,
                            DataNascimento = Input.DataNascimento,
                            Name = Input.Name,
                            Gender = Input.Gender,
                            Phone = Input.Phone,
                            ProfilePicture = "",
                            UserID = user.Id,
                            Password = user.PasswordHash,
                            Nationality = Input.Nacionality,
                            Quote = Input.Quote,
                        };

                        Console.WriteLine(Input.profilePicture);

                        DateTime today = DateTime.Today;
                        int age = today.Year - Input.DataNascimento.Year;
                        userApp.Age = age;

                        //Image addition
                        if (Input.profilePicture != null)
                        {
                            if (!(Input.profilePicture.ContentType == "image/png" ||
                                                  Input.profilePicture.ContentType == "image/jpeg"))
                            {
                                userApp.ProfilePicture = "default.webp"; // set default user logo.
                            }
                            else
                            {
                                haImagem = true;

                                Guid g = Guid.NewGuid();
                                nomeImagem = g.ToString();
                                string extensaoImagem = Path.GetExtension(Input.profilePicture.FileName).ToLowerInvariant();
                                nomeImagem += extensaoImagem;
                                userApp.ProfilePicture = nomeImagem;
                            }
                        }


                        _context.Add(userApp);
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
                            await Input.profilePicture.CopyToAsync(stream);
                        }

                    }
                    catch (Exception ex)
                    {
                        // Handle database saving errors
                        _logger.LogError(ex, "Error saving user data.");
                        ModelState.AddModelError(string.Empty, "Error saving user data. Please try again later.");
                        return Page();
                    }

                    // Email confirmation logic
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                // If creation fails, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If ModelState is invalid, redisplay the form with errors
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
