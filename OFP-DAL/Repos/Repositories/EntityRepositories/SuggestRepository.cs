using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_DAL.Repos.Repositories.BaseRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Repositories.EntityRepositories
{
    public class SuggestRepository : BaseRepo<Suggest>, ISuggestRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        public SuggestRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// Suggest nesnesi ile Suggest oluşturur.
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Suggest> CreateSuggestAsync(Suggest suggest)
        {
            await _oneFixedProblemContext.AddAsync(suggest);
            await _oneFixedProblemContext.SaveChangesAsync();
            return suggest;
        }

        /// <summary>
        /// Tüm SUGGESTLERİ DÖNDÜRÜR IENUMERABLE
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Suggest>> GetAllSuggestsAsync()
        {
            return await _oneFixedProblemContext.Suggests.ToListAsync();
        }


        /// <summary>
        /// Tüm ÖNERİ OLAN SUGGESTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Suggest>> GetAllSuggestsIsSuggestAsync()
        {
            return await _oneFixedProblemContext.Suggests.Where(x => x.IsSuggest == true).ToListAsync();
        }

        /// <summary>
        /// Tüm ŞİKAYET OLAN SUGGESTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Suggest>> GetAllSuggestsNoSuggestAsync()
        {
            return await _oneFixedProblemContext.Suggests.Where(x => x.IsSuggest == false).ToListAsync();
        }


        /// <summary>
        /// SuggestID'ye ait olan Suggest nesnesini getirir.
        /// </summary>
        /// <param name="suggestID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Suggest> GetSuggestAsync(int suggestID)
        {
            return await _oneFixedProblemContext.Suggests.FindAsync(suggestID);
        }
    }
}
