using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<ResponseDto> LoginAsync(LoginInput loginInput);

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registInput"></param>
        /// <returns></returns>
        Task<ResponseDto> RegistAsync(RegistInput registInput);

        /// <summary>
        /// 我的好友
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetMyFirentsAsync();

        /// <summary>
        /// 获取好友详情
        /// </summary>
        /// <param name="contactId">好友id</param>
        /// <returns></returns>
        Task<ResponseDto> GetFirentContactAsync(long contactId);

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetCurrentUserInfoAsync();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> UpdatePasswordAsync(UpdatePasswordInput updatePassword);

        /// <summary>
        /// 修改当前用户信息
        /// </summary>
        /// <param name="userInfoInput"></param>
        /// <returns></returns>
        Task<ResponseDto> UpdateCurrentUserInfoAsync(UserInfoInput userInfoInput);


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> LoginOutAsync();

        /// <summary>
        /// 更新用户最后连接时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseDto> UpdateLastLoginTimeAsync(long userId);

        /// <summary>
        /// 更新用户最后离线时间
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> UpdateLastOffTimeAsync(long userId);
    }
}
