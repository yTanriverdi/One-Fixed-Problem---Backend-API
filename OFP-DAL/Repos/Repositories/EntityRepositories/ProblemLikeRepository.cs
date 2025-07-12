using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Likes;
using OFP_CORE.Enums;
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
    public class ProblemLikeRepository : BaseRepo<ProblemLike>, IProblemLikeRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        public ProblemLikeRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// PROBLEMLIKE nesnesi ile yeni bir PROBLEMLIKE oluşturma.
        /// </summary>
        /// <param name="problemLike"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProblemLike> CreateProblemLikeAsync(ProblemLike problemLike)
        {
            await _oneFixedProblemContext.AddAsync(problemLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problemLike;
        }

        /// <summary>
        /// ProblemID'ye ait ProblemLike nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="problemLikeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProblemLikeAsync(int problemLikeId)
        {
            ProblemLike problemLike = await _oneFixedProblemContext.ProblemLikes.FindAsync(problemLikeId);
            if (problemLike == null) return false;
            _oneFixedProblemContext.Remove(problemLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// ProblemID'ye ait olan tüm ProblemLike'ları döndürür
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProblemLike>> GetAllProblemLikeByProblemIdAsync(int problemID)
        {
            return await _oneFixedProblemContext.ProblemLikes.Where(x => x.ProblemId == problemID && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync(); 
        }

        /// <summary>
        /// UserID'ye ait olan tüm ProblemLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ProblemLike>> GetAllProblemLikeByUserIdAsync(string userID)
        {
            return await _oneFixedProblemContext.ProblemLikes.Where(x => x.UserId == userID && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
        }

        /// <summary>
        /// ProblemLikeID'ye ait olan ProblemLike nesnesi döndürür
        /// </summary>
        /// <param name="problemLikeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProblemLike> GetProblemLikeByIdAsync(int problemLikeID)
        {
            return await _oneFixedProblemContext.ProblemLikes.FindAsync(problemLikeID);
        }

        /// <summary>
        /// UserID ve ProblemID'ye ait olan ProblemLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="problemID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProblemLike> GetProblemLikeByUserAndProblemIdAsync(string userID, int problemID)
        {
            return await _oneFixedProblemContext.ProblemLikes.Where(x => x.UserId == userID && x.ProblemId == problemID && (x.Status == Status.Active || x.Status == Status.Updated)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// ProblemLikeID'ye ait olan ProblemLike nesnesi döndürür
        /// </summary>
        /// <param name="problemLikeId"></param>
        /// <param name="updateProblemLike"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProblemLike> UpdateProblemLikeAsync(int problemLikeId, ProblemLike updateProblemLike)
        {
            ProblemLike problemLike = await _oneFixedProblemContext.ProblemLikes.FindAsync(problemLikeId);
            problemLike.Status = Status.Updated;
            problemLike.ProblemId = updateProblemLike.ProblemId;
            problemLike.UserId = updateProblemLike.UserId;
            _oneFixedProblemContext.Update(problemLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problemLike;
        }


        /// <summary>
        /// ProblemID'ye ve UserId'ye ait olan bir Problem beğenisi var mı? KONTROL ET
        /// </summary>
        /// <param name="problemLikeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> AnyProblemLikeAsync(int problemId, string userId)
        {
            ProblemLike problemLike = await _oneFixedProblemContext.ProblemLikes.Where(x => x.ProblemId == problemId && x.UserId == userId && (x.Status == Status.Active || x.Status == Status.Updated)).FirstOrDefaultAsync();
            if(problemLike == null) return false;
            return true;
        }
    }
}
