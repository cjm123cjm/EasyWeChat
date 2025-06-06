using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 管理员用户管理
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetUserInfosAsync(UserQueryInput userQueryInput);

        /// <summary>
        /// 修改用户状态，状态 0：启用 1：禁用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status">状态 0：启用 1：禁用</param>
        /// <returns></returns>
        Task<ResponseDto> UpdateUserStateAsync(long userId, int status);

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseDto> ForceOfflineAsync(long userId);

        /// <summary>
        /// 获取所有群组
        /// </summary>
        /// <param name="groupInfoQueryInput"></param>
        /// <returns></returns>
        Task<ResponseDto> GetGroupInfosAsync(GroupInfoQueryInput groupInfoQueryInput);

        
    }
}
