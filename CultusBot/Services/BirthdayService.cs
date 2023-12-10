using CultusBot.Data;
using CultusBot.Data.Entities;
using CultusBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CultusBot.Services
{
    public class BirthdayService : IBirthdayService
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly IRepository<ChatUser> _chatUserRepository;

        public BirthdayService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _chatUserRepository = unitOfWork.GetRepository<ChatUser>();
        }

        public async Task SaveUserBirthdayAsync(long chatId, long userId, DateTime birthday)
        {
            // Check if the entity is already in the context's change tracker.
            var chatUser = await _chatUserRepository.Table
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);

            if (chatUser != null)
            {
                // Update the entity detached state.
                _unitOfWork.Context.Entry(chatUser).State = EntityState.Detached;

                // Update the entity properties and mark it as Modified.
                chatUser.BirthdayDate = birthday;
                _unitOfWork.Context.Entry(chatUser).State = EntityState.Modified;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUserBirthdayAsync(long chatId, long userId)
        {
            var chatUser = await _chatUserRepository.Table.FirstOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);
            if (chatUser != null)
            {
                chatUser.BirthdayDate = null;
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<DateTime?> GetUserBirthdayAsync(long chatId, long userId)
        {
            var chatUser = await _chatUserRepository.TableNoTracking.FirstOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);
            return chatUser?.BirthdayDate;
        }

        public async Task<List<ChatUser>> GetUsersWithBDayAsync(long chatId)
        {
            DateTime currentDate = DateTime.Today;
            var chatUsers = await _chatUserRepository.TableNoTracking
                .Where(x => x.ChatId == chatId && x.BirthdayDate != null && x.IsActive)
                .OrderBy(x => x.BirthdayDate.Value.Month * 100 + x.BirthdayDate.Value.Day >= currentDate.Month * 100 + currentDate.Day
                        ? x.BirthdayDate.Value.Month * 100 + x.BirthdayDate.Value.Day
                        : x.BirthdayDate.Value.Month * 100 + x.BirthdayDate.Value.Day + 1200)
                .ToListAsync();

            return chatUsers;
        }

        public async Task<List<ChatUser>> GetUsersWithBDayAsync(DateTime date)
        {
            var chatUsers = await _chatUserRepository.TableNoTracking
                .Where(x => x.BirthdayDate != null && x.BirthdayDate.Value.Month == date.Month && x.BirthdayDate.Value.Day == date.Day)
                .ToListAsync();

            return chatUsers;
        }
    }
}
