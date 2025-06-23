namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 用户登录初始化消息
    /// </summary>
    public class WebSocketInitDataDto
    {
        /// <summary>
        /// 会话消息Dto
        /// </summary>
        public List<ChatSessionDto> ChatSessionDtos { get; set; } = new();

        /// <summary>
        /// 会话用户Dto
        /// </summary>
        public List<ChatSessionUserDto> ChatSessionUserDtos { get; set; } = new();

        /// <summary>
        /// 好友申请条数
        /// </summary>
        public int ApplyCount { get; set; }
    }
}
