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
    public class AnswerLikeRepository : BaseRepo<AnswerLike>, IAnswerLikeRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public AnswerLikeRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// AnswerLike nesnesi ile AnswerLike oluşturma
        /// </summary>
        /// <param name="answerLike"></param>
        /// <returns></returns>
        public async Task<AnswerLike> CreateAnswerLikeAsync(AnswerLike answerLike)
        {
            await _oneFixedProblemContext.AnswerLikes.AddAsync(answerLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return answerLike;
        }

        /// <summary>
        /// AnswerLikeID'ye ait olan AnswerLike nesnesini PASİF OLARAK siler
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAnswerLikeAsync(int answerLikeID)
        {
            AnswerLike answerLike = await _oneFixedProblemContext.AnswerLikes.FindAsync(answerLikeID);
            if(answerLike == null) return false;
            _oneFixedProblemContext.Remove(answerLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// AnswerID'ye ait olan tüm AnswerLike'ları döndürür
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByAnswerIdAsync(int answerID)
        {
            return await _oneFixedProblemContext.AnswerLikes.Where(x => x.AnswerId == answerID).ToListAsync();
        }

        /// <summary>
        /// UserID'ye ait olan tüm AnswerLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByUserIdAsync(string userID)
        {
            return await _oneFixedProblemContext.AnswerLikes.Where(x => x.UserId == userID).ToListAsync();
        }

        /// <summary>
        /// AnswerLikeID'ye ait olan AnswerLike nesnesini döner.
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AnswerLike> GetAnswerLikeById(int answerLikeID)
        {
            return await _oneFixedProblemContext.AnswerLikes.FindAsync(answerLikeID);
        }

        /// <summary>
        /// UserID ve AnswerID'ye ait olan AnswerLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AnswerLike> GetAnswerLikeByUserAndAnswerIdAsync(string userID, int answerID)
        {
            return await _oneFixedProblemContext.AnswerLikes.Where(x => x.UserId == userID && x.AnswerId == answerID && (x.Status == Status.Active)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// AnswerLikeID ile bulunan AnswerLike nesnesini, parametredeki AnswerLike nesnesi ile günceller
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <param name="answerLike"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<AnswerLike> UpdateAnswerLikeAsync(int answerLikeID, AnswerLike answerLike)
        {
            AnswerLike oldAnswerLike = await _oneFixedProblemContext.AnswerLikes.FindAsync(answerLikeID);
            if(oldAnswerLike == null) return null;
            oldAnswerLike.AnswerId = answerLike.AnswerId;
            oldAnswerLike.UserId = answerLike.UserId;
            oldAnswerLike.Status = Status.Updated;
            _oneFixedProblemContext.Update(oldAnswerLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return oldAnswerLike;
        }
    }
}
