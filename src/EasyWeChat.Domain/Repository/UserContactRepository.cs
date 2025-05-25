using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    internal class UserContactRepository : RepositoryBase<UserContact>, IUserContactRepository
    {
        public UserContactRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
