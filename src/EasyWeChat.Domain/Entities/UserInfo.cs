using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeChat.Domain.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(TypeName ="varchar(50)")]
        public string Email { get; set; } = null!;
        /// <summary>
        /// 昵称
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 0：直接加好友 1：同意后加好友
        /// </summary>
        public int JoinType { get; set; }
        /// <summary>
        /// 性别 0：女 1：男
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "varchar(32)")]
        public string Password { get; set; } = null!;
        /// <summary>
        /// 状态 0：启用 1：禁用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }=DateTime.Now;
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string? AreaName { get; set; } = null;
        /// <summary>
        /// 地区代码
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string? AreaCode { get; set; } = null;
        /// <summary>
        /// 最后离开时间
        /// </summary>
        public DateTime? LastOffTime { get; set; }
    }
}
