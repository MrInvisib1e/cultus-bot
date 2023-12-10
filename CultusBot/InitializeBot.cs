using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using CultusBot.Commands;
using CultusBot.Services;

namespace CultusBot
{
    public static class InitializeBot
    {
        private static TelegramBotClient botClient;

        private static ICommandManager _commandManager;
        private static IScheduleManager _scheduleManager;
        private static IUserService _userService;
        public static void Init(ICommandManager commandManager, IScheduleManager scheduleManager, IUserService userService, CancellationTokenSource cts, string botId)
        {
            botClient = new(botId);
            
            _commandManager = commandManager;
            _scheduleManager = scheduleManager;
            _userService = userService;

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            Console.WriteLine("-- Bot started --");

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            ScheduleDailyReminder(cts);
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if(update.Type == UpdateType.CallbackQuery)
                return;
            
            if (update.Message is not { } message)
                return;

            if (message.Type == MessageType.ChatMembersAdded && message.Chat.Id == -1001509704290)
            {
                await botClient.SendTextMessageAsync(
                    replyToMessageId: message.MessageId,
                    chatId: message.Chat.Id,
                    text: $"* \nТекст на дверях написано кров'ю: \n\n\"ТАЄМНУ КІМНАТУ ВІДЧИНЕНО\nНе забудь глянути у правила, щоб не наробити біди\"\n<a href=\"https://t.me/VidlunniaGroup/2291\">https://t.me/VidlunniaGroup/2291</a>\n*",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
                return;
            }

            if (message.Text is not { } messageText)
                return;

            try
            { 
                await _userService.CheckUserInfoAsync(message.Chat.Id, message.From.Id, message.From.Username);
                await _commandManager.ExecuteCommand(botClient, message, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(messageText);
                Console.WriteLine(ex.ToString());
                await botClient.SendTextMessageAsync(message.Chat.Id, " Ерррр-оооор[ Помииииилка ]. Копніть Рейдо нехай гляне що поламалось.");
            }
        }

        static async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Помилка у Telegram API... Копніть Reydo:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return;
        }

        private static void ScheduleDailyReminder(CancellationTokenSource cts)
        {
            var sendTime = DateTime.Today.AddHours(9).AddMinutes(34);

            var delay = sendTime > DateTime.Now ? sendTime - DateTime.Now : TimeSpan.FromHours(24) - (DateTime.Now - sendTime);

            Task.Delay(delay).ContinueWith(async _ =>
            {
                await _scheduleManager.SendDailyBirthdayRemainderAsync(botClient, cts);
                ScheduleDailyReminder(cts);
            });
        }
    }
}
