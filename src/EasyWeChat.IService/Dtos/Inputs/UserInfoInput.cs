using Microsoft.AspNetCore.Http;

namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 修改用户信息
    /// </summary>
    public class UserInfoInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = null!;
        /// <summary>
        /// 0：直接加好友 1：同意后加好友
        /// </summary>
        public int JoinType { get; set; } = 1;
        /// <summary>
        /// 性别 0：女 1：男
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string? AreaName { get; set; } = null;
        /// <summary>
        /// 地区代码
        /// </summary>
        public string? AreaCode { get; set; } = null;

        /// <summary>
        /// 图片
        /// </summary>
        public IFormFile? avatarFile { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public IFormFile? avatarCover { get; set; }
    }
}
