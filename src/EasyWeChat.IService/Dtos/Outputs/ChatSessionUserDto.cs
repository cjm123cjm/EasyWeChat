namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 会话用户Dto
    /// </summary>
    public class ChatSessionUserDto
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
        /// 会话id
        /// </summary>
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// 类型 0-用户 1-群组
        /// </summary>
        public int ContactType { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 最后发送的消息
        /// </summary>

        public string? LastMessage { get; set; } = null;

        /// <summary>
        /// 最后接收消息时间
        /// </summary>
        public DateTime? LastReceviceTime { get; set; }

        /// <summary>
        /// 群组人数
        /// </summary>
        public int MemberCount { get; set; }
    }
}
