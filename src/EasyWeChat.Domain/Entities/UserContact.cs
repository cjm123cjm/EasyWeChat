namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 联系人
    /// </summary>
    public class UserContact
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 联系人或群组id
        /// </summary>
        public long ContactId { get; set; }
        /// <summary>
        /// 联系类型 0：好友 1：群组
        /// </summary>
        public int ContanctType { get; set; }
        /// <summary>
        /// 状态 0：非好友 1：好友 2：已删除好友 3：被好友删除 4：已拉黑好友 5：被好友拉黑
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }
    }
}
