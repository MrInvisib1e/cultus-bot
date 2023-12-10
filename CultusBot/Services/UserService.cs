using CultusBot.Data;
using CultusBot.Data.Entities;
using CultusBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CultusBot.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<ChatUser> _chatUsersRepository;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public UserService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _chatUsersRepository = unitOfWork.GetRepository<ChatUser>();
            _unitOfWork = unitOfWork;
        }

        public async Task CheckUserInfoAsync(long chatId, long userId, string username)
        {
            var existUser = await _chatUsersRepository.Table.FirstOrDefaultAsync(u => u.UserId ==userId && u.ChatId == chatId);
            if(existUser == null)
            {
                existUser = new ChatUser
                {
                    UserId = userId,
                    ChatId = chatId,
                    UserName = username,
                    IsActive = true
                };

                _chatUsersRepository.Insert(existUser);
                await _unitOfWork.SaveAsync();
            }
            else if(existUser.UserName != username)
            {
                _unitOfWork.Context.Entry(existUser).State = EntityState.Detached;
                existUser.UserName = username;
                _unitOfWork.Context.Entry(existUser).State = EntityState.Modified;
                await _unitOfWork.SaveAsync();
            }

            return;
        }
    }
}
