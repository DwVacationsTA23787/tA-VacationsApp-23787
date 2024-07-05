namespace Dw23787.Models.dto
{
    public class TripDto
    {
        public string Id { get; set; }
        public string TripName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Transport { get; set; }
        public string InicialBudget { get; set; }
        public string FinalBudget { get; set; }
        public string Banner { get; set; }
        public bool Closed { get; set; }
        public string GroupId { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public DateOnly DataNascimento { get; set; }

        public string Quote { get; set; }
    }
}
