using CultusBot.Data.Static;

namespace CultusBot.Data.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string Text { get; set; }
        public TextType Type { get; set; }
        public bool IsActive { get; set; }
    }
}
