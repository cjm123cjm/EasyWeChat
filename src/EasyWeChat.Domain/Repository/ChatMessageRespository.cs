using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class ChatMessageRespository : RepositoryBase<ChatMessage>, IChatMessageRespository
    {
        public ChatMessageRespository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
