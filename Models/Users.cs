using System.ComponentModel.DataAnnotations;

namespace Dw23787.Models
{
    public class Users
    {
        [Key] // Identifica que é o atributo será PK
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public DateOnly DataNascimento { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }


        /// <summary>
        /// atributo para funcionar como FK entre a tabela dos Utilizadores
        /// e a tabela da Autenticação
        /// </summary>
        public string UserID { get; set; }


    }
}
