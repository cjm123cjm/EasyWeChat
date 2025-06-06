using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace EasyWeChat.Domain.Repository
{
    internal class UserContactRepository : RepositoryBase<UserContact>, IUserContactRepository
    {
        public UserContactRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="applyUsertId">申请人id</param>
        /// <param name="receiveUserId">接收人id</param>
        /// <param name="contanctId">群组id</param>
        /// <param name="contactType">0-好友，1-群组</param>
        /// <returns></returns>
        public async Task AddContact(long applyUsertId, long receiveUserId, long contanctId, int contactType)
        {
            //群聊
            if (contanctId == 1)
            {
                UserContact userContact = new UserContact
                {
                    UserId = applyUsertId,
                    ContactId = contanctId,
                    ContanctType = contactType,
                    Status = 1,
                    LastUpdatedTime = DateTime.Now
                };

                DbContext.UserContacts.Add(userContact);

                await DbContext.SaveChangesAsync();
            }
            else
            {
                //接收人添加申请人
                UserContact userContact2 = new UserContact
                {
                    UserId = receiveUserId,
                    ContactId = applyUsertId,
                    ContanctType = 0,
                    Status = 1,
                    LastUpdatedTime = DateTime.Now
                };
                DbContext.UserContacts.Add(userContact2);

                //申请人添加接收人
                UserContact userContact1 = new UserContact
                {
                    UserId = applyUsertId,
                    ContactId = receiveUserId,
                    ContanctType = 0,
                    Status = 1,
                    LastUpdatedTime = DateTime.Now
                };
                DbContext.UserContacts.Add(userContact1);

                await DbContext.SaveChangesAsync();
            }

            //TODO 创建缓存，将好友信息添加到缓存里
            //TODO 创建会话
        }

        /// <summary>
        /// 删除或拉黑联系人
        /// </summary>
        /// <param name="userId">当前用户id</param>
        /// <param name="contanctId">好友或群组id</param>
        /// <param name="contactType">0-好友，1-群组</param>
        /// <param name="status">0-删除，1-拉黑</param>
        /// <returns></returns>
        public async Task DeleteOrBlockingContact(long userId, long contanctId, int contactType, int status)
        {
            var userContact1 = await DbContext.UserContacts.FirstOrDefaultAsync(t => t.UserId == userId && t.ContactId == contanctId && t.ContanctType == contactType);
            //删除好友
            if (userContact1 != null)
            {
                if (status == 0)
                {
                    userContact1.Status = 2;
                    userContact1.LastUpdatedTime = DateTime.Now;
                }
                else
                {
                    userContact1.Status = 4;
                    userContact1.LastUpdatedTime = DateTime.Now;
                }
            }
            else
            {
                //拉黑
                if (status == 1)
                {
                    UserContact blockingContact1 = new UserContact
                    {
                        UserId = userId,
                        ContactId = contanctId,
                        ContanctType = contactType,
                        Status = 4,
                        LastUpdatedTime = DateTime.Now
                    };
                    DbContext.UserContacts.Add(blockingContact1);


                }
            }
            //被好友删除
            var userContact2 = await DbContext.UserContacts.FirstOrDefaultAsync(t => t.UserId == contanctId && t.ContactId == userId && t.ContanctType == contactType);
            if (userContact2 != null)
            {
                if (status == 0)
                {
                    userContact2.Status = 3;
                    userContact2.LastUpdatedTime = DateTime.Now;
                }
                else
                {
                    userContact2.Status = 5;
                    userContact2.LastUpdatedTime = DateTime.Now;
                }
            }
            else
            {
                //拉黑
                if (status == 1)
                {
                    UserContact blockingContact2 = new UserContact
                    {
                        UserId = contanctId,
                        ContactId = userId,
                        ContanctType = contactType,
                        Status = 5,
                        LastUpdatedTime = DateTime.Now
                    };
                    DbContext.UserContacts.Add(blockingContact2);
                }
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
