namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 好友申请
    /// </summary>
    public class ApplyInfoDto
    {
        /// <summary>
        /// 申请id
        /// </summary>
        public long ApplyId { get; set; }
        /// <summary>
        /// 申请人id
        /// </summary>
        public long ApplyUserId { get; set; }
        /// <summary>
        /// 申请人/群组名称
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人或群组id
        /// </summary>
        public long ContanctId { get; set; }
        /// <summary>
        /// 类型 0：好友 1：群组
        /// </summary>
        public int ContactType { get; set; }
        /// <summary>
        /// 最后申请时间
        /// </summary>
        public DateTime LastApplyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 状态 0：待处理 1：已同意 2：已拒绝 3：已拉黑
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 申请消息
        /// </summary>
        public string? ApplyMessage { get; set; } = null;
        /// <summary>
        /// 头像
        /// </summary>
        public string? Thumbnail { get; set; } = null;
    }
}
