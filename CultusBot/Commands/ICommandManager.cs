using CultusBot.Commands;
using CultusBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

public interface ICommandManager
{
    public void RegisterCommand(Command command, params string[] aliases);

    public Task ExecuteCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken);
}