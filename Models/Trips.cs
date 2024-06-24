using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dw23787.Models
{
    public class Trips
    {
        [Key] // Identifica que é o atributo será PK
        public int Id { get; set; }

        public string TripName { get; set; }

        public string Description { get; set; }

        public string Category { get; set;}

        public string Transport { get; set;}

        public string InicialBudget { get; set;}

        public string FinalBudget { get; set;}

        public string? Banner { get; set; } // o '?' vai tornar o atributo em preenchimento facultativo


        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'GroupFK' é uma FK em conjunto
        // com o atributo 'Group'
        [ForeignKey(nameof(Group))]
        public int GroupFK { get; set; } // FK para o Grupo
        public Groups Group { get; set; } // FK para o Grupo


        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'UserFK' é uma FK em conjunto
        // com o atributo 'User'

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
        public Users User { get; set; }

    }
}
