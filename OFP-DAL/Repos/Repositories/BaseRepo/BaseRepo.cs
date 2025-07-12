using Microsoft.EntityFrameworkCore;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Repositories.BaseRepo
{
    public abstract class BaseRepo<TEntity> : IBaseRepo<TEntity> where TEntity : class
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        private readonly DbSet<TEntity> _dbSet;

        protected BaseRepo(OneFixedProblemContext oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
            _dbSet = _oneFixedProblemContext.Set<TEntity>();
        }

        /// <summary>
        /// BASE EKLEME İŞLEMİ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbSet.AddAsync(entity);
            await _oneFixedProblemContext.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// BASE HARD DELETE İŞLEMİ
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _oneFixedProblemContext.SaveChangesAsync();
        }

        /// <summary>
        /// BASE ENUMERABLE LİSTE DÖNER
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllEnumerableAsync()
        {
            return await _dbSet.ToListAsync();
        }


        /// <summary>
        /// BASE QUERYABLE LİSTE DÖNER
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllQueryableAsync()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        /// BASE ID YE GÖRE ENTITY DÖNER
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// BASE VERİTABANI KAYDETME İŞLEMİNİ YAPAR
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _oneFixedProblemContext.SaveChangesAsync();
        }

        /// <summary>
        /// BASE FİLTREYE GÖRE ENUMERABLE ENTITY LİSTESİ DÖNER
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllFilteredEnumerable(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression != null)
                return await _dbSet.Where(expression).ToListAsync();
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// BASE FİLTREYE GÖRE QUERYABLE ENTITY LİSTESİ DÖNER
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetAllFilteredQueryable(Expression<Func<TEntity, bool>>? expression = null)
        {
            var query = _dbSet.AsQueryable();
            if (expression != null)
                query = query.Where(expression);
            return query;
        }
    }
}
