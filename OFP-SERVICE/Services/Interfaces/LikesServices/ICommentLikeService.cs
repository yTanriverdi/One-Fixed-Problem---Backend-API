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
    public interface ICommentLikeService
    {
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>


        /// <summary>
        /// CommentLike nesnesi ile yeni bir COMMENTLIKE oluşturma.
        /// </summary>
        /// <param name="commentLike"></param>
        /// <returns></returns>
        Task<CommentLike> CreateCommentLikeAsync(CommentLikeCreateDTO commentLike);

        /// <summary>
        /// CommentLikeId'ye ait CommentLike nesnesini bulup güncelleme.
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <param name="updateCommentLike"></param>
        /// <returns></returns>
        Task<CommentLike> UpdateCommentLikeAsync(CommentLikeUpdateDTO updateCommentLike);

        /// <summary>
        /// CommentLikeId'ye ait CommentLike nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentLikeAsync(int commentLikeId);



        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>

        /// <summary>
        /// UserID'ye ait olan tüm CommentLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<CommentLike>> GetAllCommentLikeByUserIdAsync(string userID);

        /// <summary>
        /// CommentID'ye ait olan tüm CommentLike'ları döndürür
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        Task<IEnumerable<CommentLike>> GetAllCommentLikeByCommentIdAsync(int commentID);

        /// <summary>
        /// UserID ve commentID'ye ait olan CommentLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="commentID"></param>
        /// <returns></returns>
        Task<CommentLike> GetCommentLikeByUserAndCommentIdAsync(string userID, int commentID);

        /// <summary>
        /// CommentLikeID'ye ait olan CommentLike nesnesi döndürür
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns></returns>
        Task<CommentLike> GetCommentLikeByIdAsync(int commentLikeID);
    }
}
