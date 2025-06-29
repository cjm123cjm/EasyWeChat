using EasyWeChat.IService.Dtos;
using EasyWeChat.IService.Dtos.Inputs;
using EasyWeChat.IService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 联系人
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserContactController : ControllerBase
    {
        private readonly IUserContactService _userContactService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userContactService"></param>
        public UserContactController(IUserContactService userContactService)
        {
            _userContactService = userContactService;
        }

        /// <summary>
        /// 搜索联系人/群组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseDto> GetSeachContacts([FromBody] SearchContactQueryInput queryInput)
        {
            return await _userContactService.GetSeachContactsAsync(queryInput);
        }
    }
}
