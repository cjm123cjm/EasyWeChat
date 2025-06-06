using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using Microsoft.AspNetCore.Mvc;
using EasyWeChat.IService.Interfaces;
using EasyWeChat.IService.Consts;
using Microsoft.AspNetCore.Authorization;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 群组服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupInfoService _groupInfoService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAdminService _adminService;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="groupInfoService"></param>
        /// <param name="hostEnvironment"></param>
        public GroupController(
            IGroupInfoService groupInfoService,
            IWebHostEnvironment hostEnvironment,
            IAdminService adminService)
        {
            _groupInfoService = groupInfoService;
            _hostEnvironment = hostEnvironment;
            _adminService = adminService;
        }

        /// <summary>
        /// 创建/修改群组
        /// </summary>
        /// <param name="groupInfoInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> CreateGroupInfo([FromBody] GroupInfoInput groupInfoInput)
        {
            ResponseDto response = await _groupInfoService.CreateGroupInfoAsync(groupInfoInput);

            if (response.Result != null)
            {
                long groupId = (long)response.Result;

                if (groupInfoInput.avatarFile != null)
                {
                    // 获取文件后缀名
                    var extension = Path.GetExtension(groupInfoInput.avatarFile.FileName);
                    // 为文件重命名，防止文件重名
                    var fileName = EasyWeChatConst.GroupOriginal + groupId + "." + extension;

                    using FileStream fileStream = new FileStream(
                        // 拼接上传路径(upload文件夹必须事先存在)
                        Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                        FileMode.Create, FileAccess.Write);
                    groupInfoInput.avatarFile.CopyTo(fileStream);
                }

                if (groupInfoInput.avatarCover != null)
                {
                    // 获取文件后缀名
                    var extension = Path.GetExtension(groupInfoInput.avatarCover.FileName);
                    // 为文件重命名，防止文件重名
                    var fileName = EasyWeChatConst.GroupThumbnail + groupId + "." + extension;

                    using FileStream fileStream = new FileStream(
                        // 拼接上传路径(upload文件夹必须事先存在)
                        Path.Combine(_hostEnvironment.ContentRootPath, "upload", fileName),
                        FileMode.Create, FileAccess.Write);

                    groupInfoInput.avatarCover.CopyTo(fileStream);
                }
            }

            return response;
        }

        /// <summary>
        /// 获取某个群组详情
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet("{groupId}")]
        public async Task<ResponseDto> GetGroupInfo(long groupId)
        {
            return await _groupInfoService.GetGroupInfoAsync(groupId);
        }

        /// <summary>
        /// 我的群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetMyGroupInfos()
        {
            return await _groupInfoService.GetMyGroupInfosAsync();
        }

        /// <summary>
        /// 我的加入群组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> GetMyJoinGroupInfos()
        {
            return await _groupInfoService.GetMyGroupInfosAsync();
        }

        /// <summary>
        /// 解散群聊
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> DissolutionGroup(long groupId)
        {
            return await _groupInfoService.DissolutionGroupAsync(groupId);
        }
    }
}
