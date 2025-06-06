namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 查询
    /// </summary>
    public class AppUpdateQuery : PageInput
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string? Version { get; set; }
    }
}
