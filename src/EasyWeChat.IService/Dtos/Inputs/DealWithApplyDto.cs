namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 处理申请请求
    /// </summary>
    public class DealWithApplyDto
    {
        /// <summary>
        /// 申请id
        /// </summary>
        public long ApplyId { get; set; }

        /// <summary>
        /// 状态 1：同意 2：拒绝 3：拉黑
        /// </summary>
        public int Status { get; set; }
    }
}
