namespace Dw23787.Models.dto
{
    public class CreateTripDto
    {
        public string TripName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public string Location { get; set; }
        public string Transport { get; set; }
        public string InicialBudget { get; set; }
        public string FinalBudget { get; set; }

    }
}
