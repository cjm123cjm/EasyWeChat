using Microsoft.AspNetCore.Http;

namespace EasyWeChat.IService.Dtos.Inputs
{
    /// <summary>
    /// 保存系统设置输入参数
    /// </summary>
    public class SystemSettingInput
    {
        /// <summary>
        /// 最大群组数
        /// </summary>
        public int MaxGroupCount { get; set; }
        /// <summary>
        /// 群组最大成员数
        /// </summary>
        public int MaxGroupMemberCount { get; set; }
        /// <summary>
        /// 最大照片内存
        /// </summary>
        public int MaxImageSize { get; set; }
        /// <summary>
        /// 最大视频内存
        /// </summary>
        public int MaxVideoSize { get; set; }
        /// <summary>
        /// 最大文件大小
        /// </summary>
        public int MaxFileSize { get; set; }
        /// <summary>
        /// 机器人id
        /// </summary>
        public int RobotUid { get; set; }
        /// <summary>
        /// 机器人昵称
        /// </summary>
        public string? RobotNickName { get; set; }
        /// <summary>
        /// 机器人欢迎语
        /// </summary>
        public string? RobotWelcome { get; set; }
        /// <summary>
        /// 机器人头像(保存的是服务器地址)
        /// </summary>
        public string? RobotIcon { get; set; }
        /// <summary>
        /// 机器人图片
        /// </summary>
        public IFormFile? RobotFile { get; set; } = null;
        /// <summary>
        /// 机器人缩略图
        /// </summary>
        public IFormFile? RobotConver { get; set; } = null;
    }
}
