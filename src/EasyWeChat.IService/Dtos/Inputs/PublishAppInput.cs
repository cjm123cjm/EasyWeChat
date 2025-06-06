namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 发布app输入参数
    /// </summary>
    public class PublishAppInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 状态 0-未发布 1-灰度发布 2-全网发布
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 灰度发布
        /// </summary>
        public string? GrayscaleUid { get; set; }
    }
}
