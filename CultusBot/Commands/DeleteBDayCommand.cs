using CultusBot.Services;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CultusBot.Commands
{
    internal class DeleteBDayCommand : Command
    {
        public string Name => "/deletebday";

        private readonly IBirthdayService _birthdayService;
        public DeleteBDayCommand(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            await _birthdayService.DeleteUserBirthdayAsync(chatId, message.From.Id);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "* Спочатку проявився ілюзорний пергамент, після чого він стрімко почав горіти, поки від нього нічого не залишилось. \nНапевно, це означає що день народження було забуто *",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}
