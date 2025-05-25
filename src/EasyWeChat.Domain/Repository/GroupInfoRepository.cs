using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class GroupInfoRepository : RepositoryBase<GroupInfo>, IGroupInfoRepository
    {
        public GroupInfoRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
