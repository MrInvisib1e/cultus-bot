using CultusBot.Models;
using CultusBot.Models.Static;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CultusBot.Commands
{
    internal class GetRandomFanfic : Command
    {
        public string Name => "/random";
        private readonly string baseUrl = "https://books.ovell.club";
        public async Task Execute(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string apiUrl = "https://api.books.ovell.club/api/books/getRandom";

            string jsonData = "{\"Id\":\"4d298a95-60ac-46b5-5b95-08db780cfc13\"}";

            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    Book book = JsonConvert.DeserializeObject<Book>(jsonResult);
                    if(book != null)
                    {
                        SendBook(botClient, message, book);
                    }
                }
                else
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
            }

        }

        private async Task SendBook(ITelegramBotClient botClient, Message message, Book book)
        {
            var chatId = message.Chat.Id;
            var response = new StringBuilder();
            response.AppendLine("");
            response.AppendLine($"НАЗВА: [{book.Title}]({baseUrl}/book/{book.Id})");
            response.AppendLine($"АВТОР(-КА): [{book.Author.Name}]({baseUrl}/p/{book.Author.Id})");
            response.AppendLine("");
            response.AppendLine($"ПЕРЕКЛАД: {(book.IsTranslation?"Так":"Ні")}");
            if (book.IsTranslation)
            {
                response.AppendLine($"ОРИГІНАЛ: [*тиць сюди*]({book.OriginalLink})");
                response.AppendLine("");
            }
            response.AppendLine($"СТАТУС: {book.BookStatusText}");
            response.AppendLine($"РЕЙТИНГ: {book.RatingText}");
            response.AppendLine($"НАПИСАНО: {book.PagesCount}ст.");
            response.AppendLine($"РОЗМІР: {book.BookSizeText}");
            if (book.ExpectedBookSize.HasValue && book.BookStatus != BookStatus.Completed)
            {
                response.AppendLine($"ОЧІКУЄТЬСЯ: {book.ExpectedBookSizeText}");
            }
            if (book.Pairings.Any())
            {
                response.AppendLine("");
                response.Append("ПЕЙРИНҐИ: ");
                foreach (var pair in book.Pairings.Take(5))
                {
                    response.Append($"{pair.FirstCharacter.FirstName} {pair.FirstCharacter.LastName} - {pair.SecondCharacter.FirstName} {pair.SecondCharacter.LastName}, ");
                }
                response.Remove(response.Length - 2, 2);
                if (book.Pairings.Count > 5)
                    response.Append("...");
                response.Append("\n");
            }
            
            if (book.Tags.Any())
            {
                response.AppendLine("");
                response.Append("ТЕҐИ: ");
                foreach (var tag in book.Tags.Take(5))
                {
                    response.Append($"{tag.Title}, ");
                }
                response.Remove(response.Length - 2, 2);
                if (book.Tags.Count > 5)
                    response.Append("...");
                response.Append("\n");
            }
            response.AppendLine("");
            if (!string.IsNullOrEmpty(book.Description) && book.Description.Length > 300)
            {
                response.Append($"ОПИС:\n{book.Description.Substring(0, Math.Min(book.Description.Length, 300))}...\n");
            }
            else if (!string.IsNullOrEmpty(book.Description) && book.Description.Length <= 300)
            {
                response.Append($"ОПИС:\n{book.Description.Substring(0, Math.Min(book.Description.Length, 300))}\n");
            }
            response.AppendLine("");

            await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: response.ToString(),
                    parseMode: ParseMode.Markdown);
            
            return;
        }

    }
}
