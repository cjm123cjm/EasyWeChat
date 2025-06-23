namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 会话用户
    /// </summary>
    public class ChatSessionUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public long ContactId { get; set; }

        /// <summary>
        /// 类型 0-用户 1-群组
        /// </summary>
        public int ContactType { get; set; }

        /// <summary>
        /// 会话id
        /// </summary>
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 最后发送消息时间
        /// </summary>
        public DateTime? LastReceiveTime { get; set; }
    }
}
