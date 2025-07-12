using System.Linq.Expressions;

namespace OFP_DAL.Repos.Interfaces.BaseRepoInterface
{
    public interface IBaseRepo<TEntity> where TEntity : class
    {
        /// <summary>
        /// EKLEME İŞLEMİ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);
        /// <summary>
        /// HARD DELETE
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Delete(TEntity entity);

        /// <summary>
        /// ENUMERABLE LİSTESİ DÖNER 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllEnumerableAsync();

        /// <summary>
        /// FILTER ENUMERABLE LİSTESİ DÖNER
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllFilteredEnumerable(Expression<Func<TEntity, bool>>? expression = null);

        /// <summary>
        /// FILTER QUERYABLE LİSTESİ DÖNER
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetAllFilteredQueryable(Expression<Func<TEntity, bool>>? expression = null);
        /// <summary>
        /// QUERYABLE LİSTESİ DÖNER
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllQueryableAsync();
        /// <summary>
        /// ID YE AİT ENTITY DÖNER
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetById(int id);

        /// <summary>
        /// KAYDETME İŞLEMİ
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}