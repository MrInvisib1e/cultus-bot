namespace CultusBot.Data.Entities
{
    public class ChatUser : BaseEntity
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public int PersonalScore { get; set; }
        public long ChatId { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public bool IsActive { get; set; }
    }
}
