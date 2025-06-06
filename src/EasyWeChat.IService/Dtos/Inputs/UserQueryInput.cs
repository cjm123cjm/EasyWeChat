namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 用户查询参数
    /// </summary>
    public class UserQueryInput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? UserId { get; set; } = null;
        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; } = null;
    }
}
