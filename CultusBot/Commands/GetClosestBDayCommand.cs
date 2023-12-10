using CultusBot.Data.Entities;
using CultusBot.Services;
using System.Globalization;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CultusBot.Commands
{
    internal class GetClosestBDayCommand: Command
    {
        public string Name => "/closestbday";

        private readonly IBirthdayService _birthdayService;
        public GetClosestBDayCommand(IBirthdayService birthdayService)
        {
            _birthdayService = birthdayService;
        }
        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var chatUsers = await _birthdayService.GetUsersWithBDayAsync(chatId);
            DateTime? nextUserBDay = null;
            ChatMember member = null;
            foreach (var user in chatUsers)
            {
                ChatMember chatMember = null;
                try
                {
                    chatMember = await botClient.GetChatMemberAsync(chatId, user.UserId);
                }
                catch (Exception ex)
                {
                    continue;
                }

                if(chatMember != null && chatMember.Status != ChatMemberStatus.Left && chatMember.Status != ChatMemberStatus.Kicked)
                {
                    nextUserBDay = user.BirthdayDate;
                    member = chatMember;
                    break;
                }
            }
            if(nextUserBDay != null)
            {
                CultureInfo ci = new CultureInfo("uk-UA");
                var username = $"{member.User.FirstName} {member.User.LastName}".Trim();
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"* Палець проявляється у повітрі та вказує на <b>{username}</b>. \nПісля цього він починає малювати дату у повітрі - звісно ж це \n[<b>{nextUserBDay.Value.ToString("dd MMMM", ci)}</b>] *",
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
