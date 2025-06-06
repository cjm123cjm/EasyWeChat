using EasyWeChat.Domain.Entities;

namespace EasyWeChat.Domain.IRepository
{
    public interface IUserContactRepository : IRepositoryBase<UserContact>
    {
        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="applyUsertId">申请人id</param>
        /// <param name="receiveUserId">接收人id</param>
        /// <param name="contanctId">群组id</param>
        /// <param name="contactType">0-好友，1-群组</param>
        /// <param name="status">状态:1：好友 2：已删除好友 3：被好友删除 4：已拉黑好友 5：被好友拉黑</param>
        /// <returns></returns>
        Task AddContact(long applyUsertId, long receiveUserId, long contanctId, int contactType);

        /// <summary>
        /// 删除或拉黑联系人
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="contanctId">好友或群组id</param>
        /// <param name="contactType">0-好友，1-群组</param>
        /// <param name="status">0-删除，1-拉黑</param>
        /// <returns></returns>
        Task DeleteOrBlockingContact(long userId, long contanctId, int contactType, int status);
    }
}
