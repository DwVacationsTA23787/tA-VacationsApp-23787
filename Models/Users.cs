using System.ComponentModel.DataAnnotations;

namespace Dw23787.Models
{
    public class Users
    {

        public Users()
        {
            TripList = new HashSet<Trips>();
            MessagesList = new HashSet<Messages>();
            isAdmin = false;
        }

        [Key] // Identifica que é o atributo será PK
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public DateOnly DataNascimento { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Quote { get; set; }

        public string Nationality { get; set; }

        public string? ProfilePicture { get; set; }

        public Boolean isAdmin { get; set; }




        /// <summary>
        /// atributo para funcionar como FK entre a tabela dos Utilizadores
        /// e a tabela da Autenticação
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Este atributo vai servir para a parte do professor joao
        /// </summary>
        public string Password { get; set; }

        public ICollection<Trips> TripList { get; set; }

        public ICollection<Messages> MessagesList { get; set; }
    }
}
