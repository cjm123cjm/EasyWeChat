using System.ComponentModel.DataAnnotations;

namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// app发布
    /// </summary>
    public class AppUpdate
    {
        public long Id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [MaxLength(200)]
        public string Version { get; set; } = null!;

        /// <summary>
        /// 更新说明
        /// </summary>
        [MaxLength(200)]
        public string UpdateDesc { get; set; } = null!;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 状态 0-未发布 1-灰度发布 2-全网发布
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 灰度id
        /// </summary>
        [MaxLength(200)]
        public string? GrayscaleUid { get; set; }
        /// <summary>
        /// 文件类型 0-本地文件 1-外链
        /// </summary>
        public int FileType { get; set; }
        /// <summary>
        /// 外链地址
        /// </summary>
        [MaxLength(200)]
        public string? OuterLink { get; set; }
    }
}
