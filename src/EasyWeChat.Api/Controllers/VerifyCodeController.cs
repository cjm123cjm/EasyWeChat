using EasyWeChat.Common.Captcha;
using EasyWeChat.Common.RedisUtil;
using EasyWeChat.IService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DotNetty.Codecs.Http.HttpContentEncoder;

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
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ResponseDto Captcha()
        {
            //验证码类型 0：纯数字 1：数字+字母 2：数字运算 默认1
            string codeType = "2";
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
            CacheManager.Set(RedisKeyPrefix.VerifyCode + codeInfo.CodeKey, codeInfo, TimeSpan.FromMinutes(5));

            ResponseDto dto = new ResponseDto()
            {
                Code = 200,
                Result = new { codeInfo.Image, codeInfo.CodeKey }
            };

            return dto;
        }
    }
}
