using System.ComponentModel.DataAnnotations;

namespace Dw23787.Models
{
    public class Messages
    {

        [Key] // Identifica que é o atributo será PK
        public int MessageId { get; set; }

        public string Message { get; set; }



    }
}
