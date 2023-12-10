using CultusBot.Commands;
using CultusBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class CommandManager : ICommandManager
{
    private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();
    private readonly Dictionary<string, string> commandSynonyms = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public CommandManager(IBirthdayService birthdayService, ITextService textService)
    {
        RegisterCommand(new GetDictionaryCommand(), "акціо словник", "киньте словником", "культус словник");
        RegisterCommand(new SaveBirthdayCommand(birthdayService));
        RegisterCommand(new GetClosestBDayCommand(birthdayService));
        RegisterCommand(new DeleteBDayCommand(birthdayService));
        RegisterCommand(new Get8BallProphecyCommand(textService), "культус скажи");
        RegisterCommand(new GetAllBDayCommand(birthdayService));
        RegisterCommand(new GetDrakoCockCommand());
        RegisterCommand(new GetRandomFanfic());
        //RegisterCommand(new GetMagicDuelCommand(textService), "культус дуель", "культус дуель з");
    }

    public void RegisterCommand(Command command, params string[] aliases)
    {
        commands[command.Name] = command;
        foreach (var alias in aliases)
        {
            commandSynonyms[alias] = command.Name;
        }
    }

    public async Task ExecuteCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        string text = message.Text;
        if (string.IsNullOrEmpty(text))
            return;

        string[] parts = text.Split(' ');
        string commandName = text.StartsWith("/") ? parts[0].ToLower() : text.ToLower();
        
        if (message.Entities != null)
        {
            foreach (MessageEntity entity in message.Entities)
            {
                if (entity.Type == MessageEntityType.BotCommand)
                {
                    var bot = await botClient.GetMeAsync();
                    commandName = commandName.Replace("@" + bot.Username, "").Trim();
                    break;
                }
            }
        }
        string matchingCommand = commandSynonyms.Keys.FirstOrDefault(key => commandName.StartsWith(key));

        if (matchingCommand != null && commandSynonyms.TryGetValue(matchingCommand, out string primaryCommand))
        {
            if (commands.TryGetValue(primaryCommand, out Command command))
            {
                await command.Execute(botClient, message, cancellationToken);
            }
        }
        else if (commands.TryGetValue(commandName, out Command command))
        {
            await command.Execute(botClient, message, cancellationToken);
        }
    }
}