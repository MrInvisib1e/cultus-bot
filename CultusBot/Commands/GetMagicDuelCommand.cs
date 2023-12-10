using CultusBot.Data.Static;
using CultusBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace CultusBot.Commands
{
    internal class GetMagicDuelCommand : Command
    {
        public string Name => "/duel";

        private readonly ITextService _textService;
        public GetMagicDuelCommand(ITextService textService)
        {
            _textService = textService;
        }
        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            var chat = await botClient.GetChatAsync(chatId);
            var user1Id = message.From.Id;
            if(message.Entities == null || message.Entities.Length == 0)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    replyToMessageId: message.MessageId,
                    text: $"Ви не вказали суперника!",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }
            if(message.Entities.Length > 1)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    replyToMessageId: message.MessageId,
                    text: $"Ви вказали забагато суперників!",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }

            var userEntity = message.Entities.FirstOrDefault(e => 
                e.Type == MessageEntityType.TextMention)?.User;
            if(userEntity?.IsBot??true)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    replyToMessageId: message.MessageId,
                    text: $"Ви не можете воювати з ботами, вони не живі!",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }

            var user2Id = userEntity.Id;

            var fullUser2 = await botClient.GetChatMemberAsync(chatId, user2Id);
            if(fullUser2 == null) {
                await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        replyToMessageId: message.MessageId,
                        text: $"На жаль ця людина не в чаті зараз",
                        parseMode: ParseMode.Html,
                        cancellationToken: cancellationToken);
                return;
            }

            var username1 = $"{message.From.FirstName} {message.From.LastName}".Trim();
            var mentionOfUser1 = $"<a href='tg://user?id={user1Id}'>{username1}</a>";

            var username2 = $"{fullUser2.User.FirstName} {fullUser2.User.LastName}".Trim();
            var mentionOfUser2 = $"<a href='tg://user?id={user2Id}'>{username2}</a>";
            InlineKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Так", callbackData: "DuelYes"),
                        InlineKeyboardButton.WithCallbackData(text: "Ні", callbackData: "DuelYes"),
                    }
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"* Кам'яний помост виростає із землі. Розступіться усі - це дуель між {mentionOfUser1} та {mentionOfUser2}.\nУ повітрі появились надпис буквами, що світяться: \n\"Чи погоджується {mentionOfUser2} на дуель з цією людиною?\" *",
                parseMode: ParseMode.Html,
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
