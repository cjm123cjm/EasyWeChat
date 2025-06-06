namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 关系
    /// </summary>
    public class UserContactDto
    {
        /// <summary>
        /// 联系人或群组id
        /// </summary>
        public long ContactId { get; set; }
        /// <summary>
        /// 联系类型 0：好友 1：群组
        /// </summary>
        public int ContanctType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string ContanctName { get; set; } = null!;
        /// <summary>
        /// 头像
        /// </summary>
        public string? Thumbnail { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string? AreaName { get; set; }

        /// <summary>
        /// 是否是好友
        /// </summary>
        public bool IsFriends { get; set; }
    }
}
