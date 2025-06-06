namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 用户Dto
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 0：直接加好友 1：同意后加好友
        /// </summary>
        public int JoinType { get; set; }
        /// <summary>
        /// 性别 0：女 1：男
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string? AreaName { get; set; } = null;
        /// <summary>
        /// 地区代码
        /// </summary>
        public string? AreaCode { get; set; } = null;
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Thumbnail { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string? PicExtension { get; set; }
    }
}
