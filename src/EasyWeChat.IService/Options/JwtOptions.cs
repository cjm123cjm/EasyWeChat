namespace EasyWeChat.IService.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Secret { get; set; } = null!;
        /// <summary>
        /// 过期时间/min
        /// </summary>
        public double Expires { get; set; }

    }
}
