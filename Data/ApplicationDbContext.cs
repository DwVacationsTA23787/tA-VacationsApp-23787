using Dw23787.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Dw23787.Data
{

    /// <summary>
    /// Esta classe representa a BD do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /* ********************************************
        * definir as 'tabelas' da base de dados
        * ******************************************** */

        public DbSet<Users> UsersApp { get; set; }
        public DbSet<Trips> Trips { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Groups> Groups { get; set; }

    }
}
