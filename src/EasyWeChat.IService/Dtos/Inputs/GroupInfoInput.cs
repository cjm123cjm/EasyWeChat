using Microsoft.AspNetCore.Http;

namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 群组输入参数
    /// </summary>
    public class GroupInfoInput
    {
        /// <summary>
        /// 0-添加 1-修改
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 群公告
        /// </summary>
        public string? GroupNotice { get; set; } = null;

        /// <summary>
        /// 加入方式 0：直接加入 1：管理员同意后加入
        /// </summary>
        public int JoinType { get; set; }

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
