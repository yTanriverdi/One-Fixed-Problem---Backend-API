using OFP_CORE.Entities.Likes;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces.LikesServices
{
    public interface IAnswerLikeService
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
        Task<AnswerLike> CreateAnswerLikeAsync(AnswerLikeCreateDTO answerLike);

        /// <summary>
        /// AnswerLikeID ile bulunan AnswerLike nesnesini, parametredeki AnswerLike nesnesi ile günceller
        /// </summary>
        /// <param name="answerLike"></param>
        /// <returns></returns>
        Task<AnswerLike> UpdateAnswerLikeAsync(AnswerLikeUpdateDTO answerLike);

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
