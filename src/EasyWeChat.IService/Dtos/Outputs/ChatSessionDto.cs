namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 会话消息Dto
    /// </summary>
    public class ChatSessionDto
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public string SessionId { get; set; } = null!;
        /// <summary>
        /// 最后接收的消息
        /// </summary>
        public string LastMessage { get; set; } = null!;
        /// <summary>
        /// 最后接收消息时间
        /// </summary>
        public DateTime? LastReceviceTime { get; set; }
    }
}
