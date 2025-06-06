using EasyWeChat.IService.Dtos;

namespace EasyWeChat.IService.Interfaces
{
    /// <summary>
    /// 联系人
    /// </summary>
    public interface IUserContactService
    {
        /// <summary>
        /// 搜索联系人(用户id或群组id)0-用户 1-群组
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto> GetSeachContactsAsync(long contactId, int type);
    }
}
