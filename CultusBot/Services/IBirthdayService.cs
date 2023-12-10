using CultusBot.Data.Entities;
using Newtonsoft.Json;

namespace CultusBot.Services
{
    public interface IBirthdayService
    {
        public Task SaveUserBirthdayAsync(long chatId, long userId, DateTime birthday);

        public Task<DateTime?> GetUserBirthdayAsync(long chatId, long userId);

        public Task<List<ChatUser>> GetUsersWithBDayAsync(long chatId);

        public Task<List<ChatUser>> GetUsersWithBDayAsync(DateTime date);

        public Task DeleteUserBirthdayAsync(long chatId, long userId);
    }
}
