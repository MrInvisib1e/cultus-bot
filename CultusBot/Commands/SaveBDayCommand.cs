using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CultusBot.Services;
using System.Globalization;

namespace CultusBot.Commands
{
    internal class SaveBirthdayCommand : Command
    {
        public string Name => "/savebday";

        private readonly IBirthdayService _birthdayService;
        public SaveBirthdayCommand(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var fromUserId = message.From.Id;
            string mentionedUser = string.Empty;

            if (string.IsNullOrEmpty(mentionedUser))
            {
                var username = $"{message.From.FirstName} {message.From.LastName}".Trim();
                mentionedUser = $"<a href='tg://user?id={fromUserId}'>{username}</a>";
            }

            byte datePossition = (byte)1;

            string[] parts = message.Text.Split(' ');
            if (parts.Length >= datePossition+1 && DateTime.TryParseExact(parts[datePossition], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthday))
            {
                await _birthdayService.SaveUserBirthdayAsync(chatId, fromUserId, birthday);

                string savedMessage = $"* Невідомий голос прошепотів: \n\"Тепер, {mentionedUser}, я тебе точно запам'ятаю. \nАдже дата твого народження, {birthday.ToString("dd.MM.yyyy")}, дуже цікава для тих, хто вміє бачити зірки.\" *";
                await botClient.SendTextMessageAsync(chatId, savedMessage, parseMode: ParseMode.Html);
                return;
            }

            await botClient.SendTextMessageAsync(chatId, " * Текст проявився у повітрі:\n\"Пам'ять підводить уже мене. \nЩоб я краще запям'ятав день твого народження, запиши його правильно: /savebday 30.10.1881\" *");
        }
    }
}
