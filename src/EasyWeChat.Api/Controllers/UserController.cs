using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyWeChat.Common.RedisUtil;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Login([FromBody] LoginInput loginInput)
        {
            return await _userService.LoginAsync(loginInput);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> Regist([FromBody] RegistInput registInput)
        {
            return await _userService.RegistAsync(registInput);
        }

        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ResponseDto GetSystemSettings()
        {
            var system = CacheManager.Get<SystemSettingDto>(RedisKeyPrefix.SystemSeting);
            ResponseDto response = new ResponseDto
            {
                Code = 200,
                Result = system
            };
            return response;
        }

        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ResponseDto SaveSystemSettings([FromBody] SystemSettingDto systemSettingDto)
        {
            CacheManager.Set(RedisKeyPrefix.SystemSeting, systemSettingDto);
            ResponseDto response = new ResponseDto
            {
                Code = 200
            };
            return response;
        }
    }
}
