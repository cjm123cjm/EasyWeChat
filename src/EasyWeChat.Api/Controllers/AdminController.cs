using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyWeChat.IService.Interfaces;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.IService.Consts;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using AutoMapper;
using System.Net.Http;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 管理员接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Policy = "IsAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="adminUserService"></param>
        /// <param name="hostEnvironment"></param>
        public AdminController(
            IAdminService adminUserService,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper)
        {
            _adminService = adminUserService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetUserInfos([FromQuery] UserQueryInput userQueryInput)
        {
            return await _adminService.GetUserInfosAsync(userQueryInput);
        }

        /// <summary>
        /// 修改用户状态，状态 0：启用 1：禁用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status">状态 0：启用 1：禁用</param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public async Task<ResponseDto> UpdateUserState(long userId, [FromBody] int status)
        {
            return await _adminService.UpdateUserStateAsync(userId, status);
        }

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> ForceOffline(long userId)
        {
            return await _adminService.ForceOfflineAsync(userId);
        }

        /// <summary>
        /// 获取所有群组
        /// </summary>
        /// <param name="groupInfoQueryInput"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetGroupInfos([FromBody] GroupInfoQueryInput groupInfoQueryInput)
        {
            return await _adminService.GetGroupInfosAsync(groupInfoQueryInput);
        }

        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
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
        [HttpPost]
        public ResponseDto SaveSystemSettings([FromBody] SystemSettingInput systemSettingDto)
        {
            var systemDto = _mapper.Map<SystemSettingDto>(systemSettingDto);
            if (systemSettingDto.RobotFile != null)
            {
                // 获取文件后缀名
                var extension = Path.GetExtension(systemSettingDto.RobotFile.FileName);
                // 为文件重命名，防止文件重名
                var fileName = EasyWeChatConst.RobotOriginal + "." + extension;
                using FileStream fileStream = new FileStream(
                // 拼接上传路径(upload文件夹必须事先存在)
                    Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                    FileMode.Create, FileAccess.Write);
                systemSettingDto.RobotFile.CopyTo(fileStream);
            }
            if (systemSettingDto.RobotConverFile != null)
            {
                // 获取文件后缀名
                var extension = Path.GetExtension(systemSettingDto.RobotConverFile.FileName);
                // 为文件重命名，防止文件重名
                var fileName = EasyWeChatConst.RobotThumbnail + "." + extension;
                using FileStream fileStream = new FileStream(
                // 拼接上传路径(upload文件夹必须事先存在)
                    Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                    FileMode.Create, FileAccess.Write);
                systemSettingDto.RobotConverFile.CopyTo(fileStream);

                string ServerUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                systemDto.RobotConver = ServerUrl + "/upload/" + fileName;
            }

            CacheManager.Set(RedisKeyPrefix.SystemSeting, systemDto);

            ResponseDto response = new ResponseDto
            {
                Code = 200
            };
            return response;
        }
    }
}
