using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CultusBot.Commands
{
    public interface IScheduleManager
    {
        public Task SendDailyBirthdayRemainderAsync(ITelegramBotClient botClient, CancellationTokenSource cts);
    }
}
