using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 申请服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApplyInfoController : ControllerBase
    {
        private readonly IApplyInfoService _applyInfoService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="applyInfoService"></param>
        public ApplyInfoController(IApplyInfoService applyInfoService)
        {
            _applyInfoService = applyInfoService;
        }

        /// <summary>
        /// 申请添加好友或群组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> ApplyAdd([FromBody] ApplyInfoInput applyInfoInput)
        {
            return await _applyInfoService.ApplyAddAsync(applyInfoInput);
        }

        /// <summary>
        /// 申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseDto> ApplyList()
        {
            return await _applyInfoService.ApplyListAsync();
        }

        /// <summary>
        /// 处理好友申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> DealWithApply([FromBody] DealWithApplyDto dealWithApplyDto)
        {
            return await _applyInfoService.DealWithApplyAsync(dealWithApplyDto);
        }
    }
}
