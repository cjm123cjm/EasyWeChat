namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 获取群组/好友
    /// </summary>
    public class GroupInfoDto
    {
        /// <summary>
        /// 群组id
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string GroupName { get; set; } = null!;
        /// <summary>
        /// 群主id
        /// </summary>
        public long GroupOwnerId { get; set; }
        /// <summary>
        /// 群主名称
        /// </summary>
        public string? GroupOwnerName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 群公告
        /// </summary>
        public string? GroupNotice { get; set; } = null;
        /// <summary>
        /// 加入方式 0：直接加入 1：管理员同意后加入
        /// </summary>
        public int JoinType { get; set; }
        /// <summary>
        /// 状态 0：解散 1：正常
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 有多少人
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string? Thumbnail { get; set; }
        /// <summary>
        /// 图片后缀名
        /// </summary>
        public string? PicExtension { get; set; }
    }
}
