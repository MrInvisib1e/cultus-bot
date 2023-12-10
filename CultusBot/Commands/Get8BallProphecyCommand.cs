using CultusBot.Data.Static;
using CultusBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CultusBot.Commands
{
    internal class Get8BallProphecyCommand : Command
    {
        public string Name => "/prophecy";

        private readonly ITextService _textService;
        public Get8BallProphecyCommand(ITextService textService)
        {
            _textService = textService;
        }
        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
           var text = _textService.GetRandomText(TextType.Prophecy8Ball);
           var chatId = message.Chat.Id;

           await botClient.SendTextMessageAsync(
                chatId: chatId,
                replyToMessageId: message.MessageId,
                text: text,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);

        }
    }
}
