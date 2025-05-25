using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EasyWeChat.Domain.Repository
{
    public class UserInfoRepository : RepositoryBase<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 根据邮箱获取用户
        /// </summary>
        /// <param name="emial"></param>
        /// <returns></returns>
        public async Task<UserInfo?> GetByEmailAsync(string emial)
        {
            return await Table.FirstOrDefaultAsync(t => t.Email == emial);
        }
    }
}
