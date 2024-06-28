using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dw23787.Models
{
    public class Messages
    {

        [Key] // Identifica que é o atributo será PK
        public int MessageId { get; set; }
        
        public string MessageTitle { get; set; }

        public string Description { get; init; }

        public string? Photo { get; init; } // o '?' vai tornar o atributo em preenchimento facultativo



        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'GroupFK' é uma FK em conjunto
        // com o atributo 'Group'
        [ForeignKey(nameof(Group))]
        public string GroupFK { get; set; } // FK para o Grupo
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
