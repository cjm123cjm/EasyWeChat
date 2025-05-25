using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EasyWeChat.Domain.IRepository
{
    public interface IRepositoryBase<T> where T : class, new()
    {
        EasyWeChatDbContext DbContext { get; set; }
        DbSet<T> Table { get; set; }

        /// <summary>
        /// 查询所有(不跟踪)
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> FindByIdAsync(long id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> AddAsync(params T[] entities);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(params T[] entities);

        /// <summary>
        /// 删除(真删除)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(params T[] entities);

    }
}
