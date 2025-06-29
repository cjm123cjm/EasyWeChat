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
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatSessionUser> ChatSessionUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserContact>().HasKey(t => new { t.UserId, t.ContactId });
            modelBuilder.Entity<ChatSessionUser>().HasKey(t => new { t.UserId, t.ContactId, t.ContactType });
        }
    }
}
