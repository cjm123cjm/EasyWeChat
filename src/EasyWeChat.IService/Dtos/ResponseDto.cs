namespace EasyWeChat.IService.Dtos
{
    /// <summary>
    /// 统一返回结果
    /// </summary>
    public class ResponseDto
    {
        /// <summary>
        /// 结果
        /// </summary>
        public object? Result { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public int Code { get; set; } = 200;
        /// <summary>
        /// 输出消息
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
