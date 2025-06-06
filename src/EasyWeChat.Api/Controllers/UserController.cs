using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.IService.Consts;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

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
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="hostEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public UserController(
            IUserService userService,
            IWebHostEnvironment hostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
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
        /// 我的好友
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> GetMyFirent()
        {
            return await _userService.GetMyFirentsAsync();
        }

        /// <summary>
        /// 获取好友详情
        /// </summary>
        /// <param name="contactId">好友id</param>
        /// <returns></returns>
        [HttpGet("{contactId}")]
        [Authorize]
        public async Task<ResponseDto> GetFirentContact(long contactId)
        {
            return await _userService.GetFirentContactAsync(contactId);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> GetCurrentUserInfo()
        {
            return await _userService.GetCurrentUserInfoAsync();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> UpdatePassword([FromBody] UpdatePasswordInput updatePassword)
        {
            return await _userService.UpdatePasswordAsync(updatePassword);
        }

        /// <summary>
        /// 修改当前用户信息
        /// </summary>
        /// <param name="userInfoInput"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> UpdateCurrentUserInfo([FromBody] UserInfoInput userInfoInput)
        {
            var userId = Convert.ToInt64(_httpContextAccessor!.HttpContext!.User.Claims.First(t => t.Type == "UserId").Value);
            //保存图片
            if (userInfoInput.avatarFile != null)
            {
                // 获取文件后缀名
                var extension = Path.GetExtension(userInfoInput.avatarFile.FileName);
                // 为文件重命名，防止文件重名
                var fileName = EasyWeChatConst.UserOriginal + userId + "." + extension;

                using FileStream fileStream = new FileStream(
                    // 拼接上传路径(upload文件夹必须事先存在)
                    Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                    FileMode.Create, FileAccess.Write);
                userInfoInput.avatarFile.CopyTo(fileStream);
            }

            if (userInfoInput.avatarCover != null)
            {
                // 获取文件后缀名
                var extension = Path.GetExtension(userInfoInput.avatarCover.FileName);
                // 为文件重命名，防止文件重名
                var fileName = EasyWeChatConst.UserThumbnail + userId + "." + extension;

                using FileStream fileStream = new FileStream(
                    // 拼接上传路径(upload文件夹必须事先存在)
                    Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                    FileMode.Create, FileAccess.Write);

                userInfoInput.avatarCover.CopyTo(fileStream);
            }
            return await _userService.UpdateCurrentUserInfoAsync(userInfoInput);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ResponseDto> LoginOut()
        {
            return await _userService.LoginOutAsync();
        }
    }
}
