using EasyWeChat.Domain.Entities;
using EasyWeChat.Domain.IRepository;

namespace EasyWeChat.Domain.Repository
{
    public class ApplyInfoRepository : RepositoryBase<ApplyInfo>, IApplyInfoRepository
    {
        public ApplyInfoRepository(EasyWeChatDbContext dbContext) : base(dbContext)
        {
        }
    }
}
