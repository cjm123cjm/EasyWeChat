using System.ComponentModel.DataAnnotations;

namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 聊天消息表
    /// </summary>
    public class ChatMessage
    {
        [Key]
        public long MessageId { get; set; }
        /// <summary>
        /// 会话id
        /// </summary>
        [MaxLength(32)]
        public string SessionId { get; set; } = null!;
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string? MessageContent { get; set; }
        /// <summary>
        /// 发送人id
        /// </summary>
        public long SendUserId { get; set; }
        /// <summary>
        /// 发送人昵称
        /// </summary>
        public string SendUserNickName { get; set; } = null!;
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 接收人id
        /// </summary>
        public long ContactId { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人类型 0-好友 1-群组
        /// </summary>
        public int ContactType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public double FileSize { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [MaxLength(200)]
        public string? FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int FileType { get; set; }
        /// <summary>
        /// 状态 0：正在发送 1：已发送
        /// </summary>
        public int Status { get; set; }
    }
}
