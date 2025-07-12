using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
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
    public class CommentRepository : BaseRepo<Comment>, ICommentRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public CommentRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// CommentID'ye ait olan Comment nesnesinin Like'ını azaltır.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CommentLikesDownAsync(int commentID)
        {
            Comment comment = await _oneFixedProblemContext.Comments.FindAsync(commentID);
            if (comment.Likes == 0) return 0;
            comment.Likes -= 1;
            _oneFixedProblemContext.Update(comment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return comment.Likes;
        }

        /// <summary>
        /// CommentID'ye ait olan Comment nesnesinin Like'ını artırır.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> CommentLikesUpAsync(int commentID)
        {
            Comment comment = await _oneFixedProblemContext.Comments.FindAsync(commentID);
            comment.Likes += 1;
            _oneFixedProblemContext.Update(comment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return comment.Likes;
        }

        /// <summary>
        /// Comment nesnesi ile yeni bir Comment oluşturma.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _oneFixedProblemContext.AddAsync(comment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return comment;
        }

        /// <summary>
        /// CommentID'ye ait Comment nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteCommentAsync(int commentID)
        {
            Comment comment = await _oneFixedProblemContext.Comments.FindAsync(commentID);
            if (comment == null) return false;
            comment.Status = Status.Passive;
            return true;
        }

        /// <summary>
        /// AKTİF OLAN TÜM COMMENTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Comment>> GetAllActiveCommentsAsync()
        {
            return await _oneFixedProblemContext.Comments.Where(x => x.Status == Status.Active || x.Status == Status.Updated).ToListAsync();
        }

        /// <summary>
        /// AnswerID'ye ait olan tüm COMMENTLERİ döndürür
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Comment>> GetAllCommentsByAnswerIdAsync(int answerID)
        {
            return await _oneFixedProblemContext.Comments.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.AnswerId == answerID).ToListAsync();
        }

        /// <summary>
        /// SearchCommentKey'e ait olan içerisinde aranan kelime geçen tüm COMMENTLERİ döndürür
        /// </summary>
        /// <param name="searchCommentKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Comment>> GetAllCommentsBySearchKeyAsync(string searchCommentKey)
        {
            return await _oneFixedProblemContext.Comments.Where(x => x.CommentContent.Replace(" ", "").ToLower().Contains(searchCommentKey.Replace(" ", "").ToLower())).ToListAsync();
        }

        /// <summary>
        /// UserID'ye ait olan tüm COMMENTLERİ DÖNDÜRÜR
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Comment>> GetAllCommentsByUserIdAsync(string userID)
        {
            return await _oneFixedProblemContext.Comments.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.UserId == userID).ToListAsync();
        }

        /// <summary>
        /// CommentID'ye ait olan COMMENT nesnesini döndürür.
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Comment> GetByIdAsync(int commentID)
        {
            return await _oneFixedProblemContext.Comments.FindAsync(commentID);
        }

        /// <summary>
        /// CommentID'ye ait Comment nesnesini bulup güncelleme.
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="updateComment"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Comment> UpdateCommentAsync(int commentID, Comment updateComment)
        {
            Comment comment = await _oneFixedProblemContext.Comments.FindAsync(commentID);
            if (comment == null) return null;
            comment.Status = Status.Updated;
            comment.UpdatedDate = DateTime.UtcNow;
            comment.Likes = updateComment.Likes;
            comment.CommentContent = updateComment.CommentContent;
            _oneFixedProblemContext.Update(comment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return comment;

        }
    }
}
