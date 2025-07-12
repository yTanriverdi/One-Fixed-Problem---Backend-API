using Microsoft.EntityFrameworkCore;
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
    public class CommentLikeRepository : BaseRepo<CommentLike>, ICommentLikeRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public CommentLikeRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// CommentLike nesnesi ile yeni bir COMMENTLIKE oluşturma.
        /// </summary>
        /// <param name="commentLike"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommentLike> CreateCommentLikeAsync(CommentLike commentLike)
        {
            await _oneFixedProblemContext.CommentLikes.AddAsync(commentLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return commentLike;
        }

        /// <summary>
        /// CommentLikeId'ye ait CommentLike nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteCommentLikeAsync(int commentLikeId)
        {
            CommentLike commentLike = await _oneFixedProblemContext.CommentLikes.FindAsync(commentLikeId);
            if (commentLike == null) return false;
            _oneFixedProblemContext.Remove(commentLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// CommentID'ye ait olan tüm CommentLike'ları döndürür
        /// </summary>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<CommentLike>> GetAllCommentLikeByCommentIdAsync(int commentID)
        {
            return await _oneFixedProblemContext.CommentLikes.Where(x => x.CommentId == commentID && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
        }

        /// <summary>
        /// UserID'ye ait olan tüm CommentLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CommentLike>> GetAllCommentLikeByUserIdAsync(string userID)
        {
            return await _oneFixedProblemContext.CommentLikes.Where(x => x.UserId == userID && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
        }

        /// <summary>
        /// CommentLikeID'ye ait olan CommentLike nesnesi döndürür
        /// </summary>
        /// <param name="commentLikeID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommentLike> GetCommentLikeByIdAsync(int commentLikeID)
        {
            return await _oneFixedProblemContext.CommentLikes.FindAsync(commentLikeID);
        }

        /// <summary>
        /// UserID ve commentID'ye ait olan CommentLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="commentID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CommentLike> GetCommentLikeByUserAndCommentIdAsync(string userID, int commentID)
        {
            return await _oneFixedProblemContext.CommentLikes.Where(x => x.UserId == userID && x.CommentId == commentID && (x.Status == Status.Active || x.Status == Status.Updated)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// CommentLikeId'ye ait CommentLike nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="commentLikeId"></param>
        /// <param name="updateCommentLike"></param>
        /// <returns></returns>
        public async Task<CommentLike> UpdateCommentLikeAsync(int commentLikeId, CommentLike updateCommentLike)
        {
            CommentLike commentLike = await _oneFixedProblemContext.CommentLikes.FindAsync(commentLikeId);
            commentLike.Status = Status.Updated;
            commentLike.CommentId = updateCommentLike.CommentId;
            commentLike.UserId = updateCommentLike.UserId;
            _oneFixedProblemContext.Update(commentLike);
            await _oneFixedProblemContext.SaveChangesAsync();
            return commentLike;
        }
    }
}
