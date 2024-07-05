namespace Dw23787.Models.dto
{
    public class Usersdto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public DateOnly DataNascimento { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string Phone { get; set; }

        public string? ProfilePicture { get; set; }

        public string Quote { get; set; }
    }
}
