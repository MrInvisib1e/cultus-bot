using CultusBot.Data.Entities;
using CultusBot.Services;
using System.Globalization;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CultusBot.Commands
{
    internal class GetAllBDayCommand : Command
    {
        public string Name => "/allbdays";

        private readonly IBirthdayService _birthdayService;
        public GetAllBDayCommand(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }
        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var chatUsers = await _birthdayService.GetUsersWithBDayAsync(chatId);
            List<ChatMember> members = new List<ChatMember>();
            List<ChatUser> failedUsers = new List<ChatUser>();

            foreach (var user in chatUsers)
            {
                ChatMember chatMember = null;
                try
                {
                    chatMember = await botClient.GetChatMemberAsync(chatId, user.UserId);
                }
                catch
                {
                    failedUsers.Add(user);
                }

                if(chatMember != null && chatMember.Status != ChatMemberStatus.Left && chatMember.Status != ChatMemberStatus.Kicked)
                {
                    members.Add(chatMember);
                }
            }
            if(members.Any())
            {
                CultureInfo ci = new CultureInfo("uk-UA");
                var mention = string.Empty;
                foreach (var member in members)
                {
                    var user = chatUsers.FirstOrDefault(x => x.UserId == member.User.Id);
                    var username = $"{member.User.FirstName} {member.User.LastName}".Trim();
                    mention += $"[{user.BirthdayDate.Value.ToString("dd MMMM", ci)}] - {username} \n";
                }

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"* Урочиста музика заграла та у повітрі появився пергамент з іменами: *\n\n{mention}",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "* З'явився малюнок плечей, що знизуть. Напевно це означає, що закляття нічого не дало і не можливо знайти найближчий день народження зараз *",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

        }
    }
}
