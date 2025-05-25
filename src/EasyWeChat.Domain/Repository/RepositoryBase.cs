using EasyWeChat.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EasyWeChat.Domain.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        public EasyWeChatDbContext DbContext { get; set; }
        public DbSet<T> Table { get; set; }
        public RepositoryBase(EasyWeChatDbContext dbContext)
        {
            DbContext = dbContext;
            Table = dbContext.Set<T>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(params T[] entities)
        {
            await Table.AddRangeAsync(entities);

            return await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 查询所有(不跟踪)
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> All()
        {
            return Table.AsNoTracking();
        }

        /// <summary>
        /// 删除(真删除)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(params T[] entities)
        {
            Table.RemoveRange(entities);

            return await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> FindByIdAsync(long id)
        {
            return await Table.FindAsync(id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(params T[] entities)
        {
            Table.UpdateRange(entities);

            return await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }
    }
}
