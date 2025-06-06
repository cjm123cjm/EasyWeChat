namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UpdatePasswordInput
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; } = null!;
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; } = null!;
    }
}
