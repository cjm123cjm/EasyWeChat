namespace EasyWeChat.IService.Enums
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageTypeEnum
    {
        /// <summary>
        /// 连接ws获取消息
        /// </summary>
        INIT,

        /// <summary>
        /// 添加好友打招呼消息
        /// </summary>
        ADD_FRIEND,

        /// <summary>
        /// 普通聊天
        /// </summary>
        CHAT,

        /// <summary>
        /// 群组创建成功，可以聊天了
        /// </summary>
        GROUP_CREATE,

        /// <summary>
        /// 好友申请
        /// </summary>
        CONTACT_APPLY,

        /// <summary>
        /// 媒体文件
        /// </summary>
        MEDIA_CHAT,

        /// <summary>
        /// 文件上传
        /// </summary>
        FILE_UPLOAD,

        /// <summary>
        /// 强制下线
        /// </summary>
        FORCE_OFF_LINE,

        /// <summary>
        /// 解散群聊
        /// </summary>
        DISSOLUTION_GROUP,

        /// <summary>
        /// 加入群组
        /// </summary>
        ADD_GROUP,

        /// <summary>
        /// 更改群组名称
        /// </summary>
        GROUP_NAME_UPDATE,

        /// <summary>
        /// 退出群组
        /// </summary>
        LEAVE_GROUP,

        /// <summary>
        /// 被管理员移除群聊
        /// </summary>
        REMOVE_GROUP
    }
}
