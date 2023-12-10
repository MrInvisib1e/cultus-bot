using Telegram.Bot;
using Telegram.Bot.Types;

namespace CultusBot.Commands
{
    public interface Command
    {
        public abstract string Name { get; }
        public abstract Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
    }
}
