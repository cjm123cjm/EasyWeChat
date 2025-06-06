namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 分页输入
    /// </summary>
    public class PageInput
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int PageSize { get; set; } = 30;
    }
}
