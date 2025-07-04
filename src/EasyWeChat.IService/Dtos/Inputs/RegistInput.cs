﻿namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 注册输入参数
    /// </summary>
    public class RegistInput
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; } = null!;
        /// <summary>
        /// 验证码key
        /// </summary>
        public string CodeKey { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
    }
}
