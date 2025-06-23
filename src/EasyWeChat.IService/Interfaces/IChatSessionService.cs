using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 会话服务
    /// </summary>
    public interface IChatSessionService
    {
        /// <summary>
        /// 获取用户初始化消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebSocketInitDataDto> GetUserWebSockerInitDataAsync(long userId);
    }
}
