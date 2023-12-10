using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CultusBot.Commands
{
    internal class GetDictionaryCommand: Command
    {
        public string Name => "/dictionary";

        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            await botClient.SendDocumentAsync(
                chatId: chatId,
                document: InputFile.FromUri("https://cdn.discordapp.com/attachments/385499161186926595/1154019630365540372/Harry_potter_terminology_dictionary.pdf"),
                caption: $"Ось словник на основі оригінального перекладу видавництва А-ба-ба-га-ла-ма та Віктора Морозова.\n\nНижче ви можете знайти посилання на канал GSF, де є його остання редакція ^^",
                parseMode: ParseMode.Html,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithUrl(
                        text: "GSF",
                        url: "https://t.me/gingersnapefan/253")),
                cancellationToken: cancellationToken);
        }
    }
}
