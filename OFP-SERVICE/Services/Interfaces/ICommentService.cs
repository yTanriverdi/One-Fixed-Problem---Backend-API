using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Comment nesnesi ile yeni bir Comment oluşturma.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<Comment> CreateCommentAsync(CommentCreateDTO comment);

        /// <summary>
        /// CommentID'ye ait Comment nesnesini bulup güncelleme.
        /// </summary>
        /// <param name="updateComment"></param>
        /// <returns></returns>
        Task<Comment> UpdateCommentAsync(CommentUpdateDTO updateComment);

        /// <summary>
        /// CommentID'ye ait Comment nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        Task<bool> DeleteCommentAsync(int commentID);

        /// <summary>
        /// CommentID'ye ait olan COMMENT nesnesini döndürür.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Comment> GetByIdAsync(int commentID);

        /// <summary>
        /// AnswerID'ye ait olan tüm COMMENTLERİ döndürür
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllCommentsByAnswerIdAsync(int answerID);

        /// <summary>
        /// UserID'ye ait olan tüm COMMENTLERİ DÖNDÜRÜR
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllCommentsByUserIdAsync(string userID);

        /// <summary>
        /// AKTİF OLAN TÜM COMMENTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllActiveCommentsAsync();

        /// <summary>
        /// SearchCommentKey'e ait olan içerisinde aranan kelime geçen tüm COMMENTLERİ döndürür
        /// </summary>
        /// <param name="searchCommentKey"></param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetAllCommentsBySearchKeyAsync(string searchCommentKey);

        /// <summary>
        /// CommentID'ye ait olan Comment nesnesinin Like'ını artırır.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        Task<int> CommentLikesUpAsync(int commentID);

        /// <summary>
        /// CommentID'ye ait olan Comment nesnesinin Like'ını azaltır.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        Task<int> CommentLikesDownAsync(int commentID);
    }
}
