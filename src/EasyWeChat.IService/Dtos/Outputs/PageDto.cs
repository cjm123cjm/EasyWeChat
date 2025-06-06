namespace EasyWeChat.IService.Dtos.Outputs
{
    /// <summary>
    /// 分页-输出对象
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class PageDto<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<T> Data { get; set; }
    }
}
