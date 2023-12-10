using Telegram.Bot.Types;
using Telegram.Bot;
using CultusBot.Services;
using Telegram.Bot.Types.Enums;
using System.Threading;
using CultusBot.Data.Static;
using System.Text;

namespace CultusBot.Commands
{
    public class ScheduleManager : IScheduleManager
    {
        private readonly IBirthdayService _birthdayService;
        private readonly ITextService _textService;

        public ScheduleManager(IBirthdayService birthdayService, ITextService textService)
        {
            _birthdayService = birthdayService;
            _textService = textService;
        }
        public async Task SendDailyBirthdayRemainderAsync(ITelegramBotClient botClient, CancellationTokenSource cancellationToken)
        {
            var users = await _birthdayService.GetUsersWithBDayAsync(DateTime.Today);
            if(users == null && !users.Any()) {
                return;
            }
            
            foreach (var groupedChat in users.GroupBy(u=>u.ChatId))
            {
                var membersWithBDay = new List<ChatMember>();
                var chatId = groupedChat.Key;
                foreach (var user in groupedChat.Select(u=>u))
                {
                    var member = await botClient.GetChatMemberAsync(chatId, user.UserId);
                    if (member != null && member.Status != ChatMemberStatus.Kicked && member.Status != ChatMemberStatus.Left) {
                        membersWithBDay.Add(member);
                    }
                }
                if (!membersWithBDay.Any())
                {
                    continue;
                }

                string mention = string.Empty;
                foreach (var member in membersWithBDay)
                {
                    var username = $"{member.User.FirstName} {member.User.LastName}".Trim();
                    mention += $"<a href='tg://user?id={member.User.Id}'>{username}</a>\n";
                }

                var textType = membersWithBDay.Count > 1 ? TextType.ScheduledMultiBirthdays : TextType.ScheduledOneBirthday;

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: new StringBuilder(_textService.GetRandomText(textType).Replace("{mention}", mention)).ToString(),
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken.Token);
            }
        }
    }
}
