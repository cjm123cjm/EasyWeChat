using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class ChatSessionUserRepository : RepositoryBase<ChatSessionUser>, IChatSessionUserRepository
    {
        public ChatSessionUserRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
