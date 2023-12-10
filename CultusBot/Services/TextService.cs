using CultusBot.Data;
using CultusBot.Data.Entities;
using CultusBot.Data.Repositories;
using CultusBot.Data.Static;

namespace CultusBot.Services
{
    public class TextService : ITextService
    {
        private readonly IRepository<ChatMessage> _chatBotMessageRepository;

        public TextService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _chatBotMessageRepository= unitOfWork.GetRepository<ChatMessage>();
        }

        public string GetRandomText(TextType textType)
        {
            var messagesWithTextType = _chatBotMessageRepository.TableNoTracking
                .Where(x => x.Type == textType && x.IsActive)
                .ToList();

            if (!messagesWithTextType.Any())
            {
                return string.Empty; 
            }

            int randomIndex = new Random().Next(0, messagesWithTextType.Count);

            var randomChatBotMessage = messagesWithTextType[randomIndex];

            return randomChatBotMessage.Text?.Replace("\\n", "\n") ?? string.Empty;
        }
    }
}
