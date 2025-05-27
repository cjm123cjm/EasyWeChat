using EasyWeChat.IService.Dtos.Outputs;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 生成token
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        string GenerateToken(UserInfoDto userDto);
    }
}
