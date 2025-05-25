using EasyWeChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyWeChat.Domain
{
    public class EasyWeChatDbContext : DbContext
    {
        public EasyWeChatDbContext(DbContextOptions<EasyWeChatDbContext> options) : base(options)
        {

        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<GroupInfo> GroupInfos { get; set; }
        public DbSet<ApplyInfo> ApplyInfos { get; set; }
    }
}
