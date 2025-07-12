using OFP_CORE.Entities.Likes;
using OFP_CORE.EntitiesInterfaces;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface IAnswerLikeRepository : IBaseRepo<AnswerLike>
    {
        /// <summary>
        /// UserID'ye ait olan tüm AnswerLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByUserIdAsync(string userID);

        /// <summary>
        /// AnswerID'ye ait olan tüm AnswerLike'ları döndürür
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByAnswerIdAsync(int answerID);

        /// <summary>
        /// UserID ve AnswerID'ye ait olan AnswerLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task<AnswerLike> GetAnswerLikeByUserAndAnswerIdAsync(string userID, int answerID);

        /// <summary>
        /// AnswerLike nesnesi ile AnswerLike oluşturma
        /// </summary>
        /// <param name="answerLike"></param>
        /// <returns></returns>
        Task<AnswerLike> CreateAnswerLikeAsync(AnswerLike answerLike);

        /// <summary>
        /// AnswerLikeID ile bulunan AnswerLike nesnesini, parametredeki AnswerLike nesnesi ile günceller
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <param name="answerLike"></param>
        /// <returns></returns>
        Task<AnswerLike> UpdateAnswerLikeAsync(int answerLikeID, AnswerLike answerLike);

        /// <summary>
        /// AnswerLikeID'ye ait olan AnswerLike nesnesini siler
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <returns></returns>
        Task<bool> DeleteAnswerLikeAsync(int answerLikeID);

        /// <summary>
        /// AnswerLikeID'ye ait olan AnswerLike nesnesini döner.
        /// </summary>
        /// <param name="answerLikeID"></param>
        /// <returns></returns>
        Task<AnswerLike> GetAnswerLikeById(int answerLikeID);
    }
}
