using EasyWeChat.Domain.Entities;

namespace EasyWeChat.Domain.IRepository
{
    public interface IUserInfoRepository : IRepositoryBase<UserInfo>
    {
        /// <summary>
        /// 根据邮箱获取用户
        /// </summary>
        /// <param name="emial"></param>
        /// <returns></returns>
        Task<UserInfo?> GetByEmailAsync(string emial);
    }
}
