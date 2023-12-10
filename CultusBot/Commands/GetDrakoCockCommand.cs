using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CultusBot.Commands
{
    internal class GetDrakoCockCommand : Command
    {
        public string Name => "/dc";

        public GetDrakoCockCommand()
        {
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string text = message.Text;
            string[] parts = text.Split(' ');
            

            if(parts.Length<2 || !int.TryParse(parts[1], out int heigthInt))
            {
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "* Введіть коректний зріст у сантиметрах або виправте команду. Для прикладу: /dc 170 *",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }

            var chatId = message.Chat.Id;
            var drakoCock = (heigthInt / 150f).ToString("0.##");
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"* Це буде приблизно {drakoCock} Дракочлена *",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

        }
    }
}
