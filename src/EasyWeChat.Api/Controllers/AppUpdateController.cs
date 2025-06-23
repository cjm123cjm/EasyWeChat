using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// app发布服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "IsAdmin")]
    public class AppUpdateController : ControllerBase
    {
        private readonly IAppUpdateService _appUpdateService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="appUpdateService"></param>
        public AppUpdateController(IAppUpdateService appUpdateService)
        {
            _appUpdateService = appUpdateService;
        }

        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="appUpdateInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> AddOrUpdateAppUpdate([FromBody] AppUpdateInput appUpdateInput)
        {
            return await _appUpdateService.AddOrUpdateAppUpdateAsync(appUpdateInput);
        }

        /// <summary>
        /// 获取app发布记录
        /// </summary>
        /// <param name="appUpdateQuery"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetAppUpdates([FromQuery] AppUpdateQuery appUpdateQuery)
        {
            return await _appUpdateService.GetAppUpdatesAsync(appUpdateQuery);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> DeleteAddUpdate([FromBody] long id)
        {
            return await _appUpdateService.DeleteAddUpdateAsync(id);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="publishAppInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> PublishAddUpdate([FromBody] PublishAppInput publishAppInput)
        {
            return await _appUpdateService.PublishAddUpdateAsync(publishAppInput);
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet("{version}")]
        public async Task<ResponseDto> CheckVersion(string version)
        {
            return await _appUpdateService.CheckVersionAsync(version);
        }
    }
}
