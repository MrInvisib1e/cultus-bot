using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultusBot.Services
{
    public interface IUserService
    {
        public Task CheckUserInfoAsync(long chatId, long userId, string username);
    }
}
