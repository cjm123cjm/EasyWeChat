namespace EasyWeChat.Common.Captcha
{
    /// <summary>
    /// 验证码信息
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 验证码数据流
        /// </summary>
        public byte[] Image { get; set; }
        /// <summary>
        /// base64
        /// </summary>
        public string Base64Str { get { return Convert.ToBase64String(Image); } }
    }
}
