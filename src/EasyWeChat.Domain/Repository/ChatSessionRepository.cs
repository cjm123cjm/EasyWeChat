using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class ChatSessionRepository : RepositoryBase<ChatSession>, IChatSessionRepository
    {
        public ChatSessionRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
