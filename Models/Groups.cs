using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dw23787.Models
{
    public class Groups
    {
        public Groups()
        {
            MessagesList = new HashSet<Messages>();
        }

        [Key] // Identifica que é o atributo será PK
        public int GroupId { get; set; }

        public string Name { get; set; }


        // relacionamento 1-N

        // esta anotação informa a EF
        // que o atributo 'TripFK' é uma FK em conjunto
        // com o atributo 'Trip'

        [ForeignKey(nameof(Trip))]
        public int TripFk { get; set; }
        public Trips Trip { get; set; }
        public ICollection<Messages> MessagesList { get; set; }

    }
}
