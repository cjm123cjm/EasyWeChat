namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 查询好友/群组 参数
    /// </summary>
    public class SearchContactQueryInput
    {
        /// <summary>
        /// 用户id 或 群组id
        /// </summary>
        public long ContactId { get; set; }
        /// <summary>
        /// 0-好友 1-群组
        /// </summary>
        public int ContactType { get; set; }
    }
}
