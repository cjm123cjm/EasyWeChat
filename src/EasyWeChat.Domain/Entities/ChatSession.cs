using System.ComponentModel.DataAnnotations;

namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 会话信息
    /// </summary>
    public class ChatSession
    {
        /// <summary>
        /// 会话id
        /// </summary>
        [Key]
        [MaxLength(100)]
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
