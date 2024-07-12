namespace Dw23787.Models.dto
{
    public class CreateMessageDto
    {

        public CreateMessageDto() {
            Time = DateTime.Now;
        }

        public string MessageTitle { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; init; }

        public string? Photo { get; set; }

        public string GroupFK { get; set; }

        public int UserFK { get; set; }
    }
}
