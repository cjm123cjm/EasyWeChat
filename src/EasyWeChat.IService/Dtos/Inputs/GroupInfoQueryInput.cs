namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 群组查询输入参数
    /// </summary>
    public class GroupInfoQueryInput
    {
        /// <summary>
        /// 群id
        /// </summary>
        public long? GroupId { get; set; }
        /// <summary>
        /// 群名称
        /// </summary>
        public string? GroupName { get; set; }
        /// <summary>
        /// 群主id
        /// </summary>
        public long? GroupOwnerId { get; set; }
    }
}
