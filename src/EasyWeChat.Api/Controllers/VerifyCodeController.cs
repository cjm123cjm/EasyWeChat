using EasyWeChat.Common.Captcha;
using EasyWeChat.Common.RedisUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyWeChat.Api.Controllers
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VerifyCodeController : ControllerBase
    {
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="codeType">验证码类型 0：纯数字 1：数字+字母 2：数字运算 默认1</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{codeType}/{email}")]
        public VerifyCode Captcha(string codeType, string email)
        {
            VerifyCode codeInfo = new VerifyCode();
            if (codeType == "0")
            {
                codeInfo = CreateCaptcha.CreateVerifyCode(4, VerifyCodeType.NUM);
            }
            else if (codeType == "1")
            {
                codeInfo = CreateCaptcha.CreateVerifyCode(4, VerifyCodeType.CHAR);
            }
            else if (codeType == "2")
            {
                codeInfo = CreateCaptcha.CreateVerifyCode(4, VerifyCodeType.ARITH);
            }

            //保存到redis中
            CacheManager.Set(RedisKeyPrefix.VerifyCode + email, codeInfo, TimeSpan.FromMinutes(5));

            return codeInfo;
        }
    }
}
