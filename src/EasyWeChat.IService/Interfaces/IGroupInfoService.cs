using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 群组服务
    /// </summary>
    public interface IGroupInfoService
    {
        /// <summary>
        /// 创建群组
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> CreateGroupInfoAsync(GroupInfoInput groupInfoInput);

        /// <summary>
        /// 获取某个群组详情
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<ResponseDto> GetGroupInfoAsync(long groupId);

        /// <summary>
        /// 我的群组
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetMyGroupInfosAsync();

        /// <summary>
        /// 我的加入群组
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetMyJoinGroupInfosAsync();

        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<ResponseDto> DissolutionGroupAsync(long groupId);
    }
}
