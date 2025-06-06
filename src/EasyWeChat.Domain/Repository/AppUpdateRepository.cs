using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class AppUpdateRepository : RepositoryBase<AppUpdate>, IAppUpdateRepository
    {
        public AppUpdateRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
