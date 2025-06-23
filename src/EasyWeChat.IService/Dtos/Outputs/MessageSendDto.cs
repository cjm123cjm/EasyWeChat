using EasyWeChat.IService.Enums;

namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 消息发送Dto
    /// </summary>
    public class MessageSendDto<T> where T : class 
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public long MessageId { get; set; }
        /// <summary>
        /// 会话id
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 发送人id
        /// </summary>
        public long SendUserId { get; set; }
        /// <summary>
        /// 发送人昵称
        /// </summary>
        public string SendUserNikcName { get; set; }
        /// <summary>
        /// 联系人id
        /// </summary>
        public long ContactId { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }
        /// <summary>
        /// 最后的消息
        /// </summary>
        public string LastMessage { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageTypeEnum MessageType { get; set; }
        /// <summary>
        /// 发送消息时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 联系人类型
        /// </summary>
        public int ContactType { get; set; }
        /// <summary>
        /// 消息状态：0-发送中 1-已发送
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 扩展消息
        /// </summary>
        public T? ExtendData { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public double FileSize { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// 群员
        /// </summary>
        public int MemberCount { get; set; }
    }
}
