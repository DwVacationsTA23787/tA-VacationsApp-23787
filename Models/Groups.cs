using System.ComponentModel.DataAnnotations;

namespace Dw23787.Models
{
    public class Groups
    {

        [Key] // Identifica que é o atributo será PK
        public int GroupId { get; set; }

        public string Name { get; set; }

    }
}
