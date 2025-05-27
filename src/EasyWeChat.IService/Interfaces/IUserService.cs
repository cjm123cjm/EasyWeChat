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
    }
}
