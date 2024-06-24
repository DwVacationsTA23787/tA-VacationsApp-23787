using System.ComponentModel.DataAnnotations;

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
    
    }
}
