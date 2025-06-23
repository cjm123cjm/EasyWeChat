namespace EasyWeChat.Common.RedisUtil;

/// <summary>
/// 所有开发人员redis使用的Key前缀
/// </summary>
public static class RedisKeyPrefix
{
    /// <summary>
    /// 会话
    /// </summary>
    public const string RedisSession = "RedisSession";
    /// <summary>
    /// 持久化
    /// </summary>
    public const string Permission = "Permission";

    /// <summary>
    /// 缓存及临时数据
    /// </summary>
    public const string RedisTempData = "RedisTempData";

    /// <summary>
    /// 无前缀
    /// </summary>
    public const string Empty = "";

    /// <summary>
    /// 前缀
    /// </summary>
    public const string InstanceName = "eastwechat_";

    /// <summary>
    /// 验证码
    /// </summary>
    public const string VerifyCode = "VerifyCode_";

    /// <summary>
    /// 用户在线
    /// </summary>
    public const string Online = "Online_";

    /// <summary>
    /// 用户心跳检验
    /// </summary>
    public const string Heart = "Heart_";

    /// <summary>
    /// 系统设置
    /// </summary>
    public const string SystemSeting = "SystemSeting";

    /// <summary>
    /// 用户联系人
    /// </summary>

    public const string User_Contact_Ids = "easywechat:ws:contactids:";
}