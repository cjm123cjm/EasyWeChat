using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 申请表
    /// </summary>
    public class ApplyInfo
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
        /// 接收人id
        /// </summary>
        public long ReceiveUserId { get; set; }
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
        [Column(TypeName = "varchar(50)")]
        public string? ApplyMessage { get; set; } = null;
    }

}
