namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 申请输入参数
    /// </summary>
    public class ApplyInfoInput
    {
        /// <summary>
        /// 接收人id
        /// </summary>
        public long ReceiveUserId { get; set; }
        /// <summary>
        /// 类型 0：好友 1：群组
        /// </summary>
        public int ContactType { get; set; }
        /// <summary>
        /// 联系人或群组id
        /// </summary>
        public long ContanctId { get; set; }
        /// <summary>
        /// 最后申请时间
        /// </summary>
        public DateTime LastApplyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 申请消息
        /// </summary>
        public string? ApplyMessage { get; set; } = null;
    }
}
