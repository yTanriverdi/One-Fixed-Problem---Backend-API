using AutoMapper;
using OFP_CORE.Entities.Likes;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs.Likes;
using OFP_SERVICE.Services.Interfaces.LikesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete.LikesServices
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly ICommentLikeRepository _commentLikeRepository;
        private readonly IMapper _mapper;

        public CommentLikeService(ICommentLikeRepository commentLikeRepository, IMapper mapper)
        {
            _commentLikeRepository = commentLikeRepository;
            _mapper = mapper;
        }

        public async Task<CommentLike> CreateCommentLikeAsync(CommentLikeCreateDTO commentLike)
        {
            CommentLike newCommentLike = _mapper.Map<CommentLike>(commentLike);
            return await _commentLikeRepository.CreateCommentLikeAsync(newCommentLike);
        }

        public async Task<bool> DeleteCommentLikeAsync(int commentLikeId)
        {
            if (commentLikeId <= 0) return false;
            return await _commentLikeRepository.DeleteCommentLikeAsync(commentLikeId);
        }

        public async Task<IEnumerable<CommentLike>> GetAllCommentLikeByCommentIdAsync(int commentID)
        {
            if (commentID <= 0) return null;
            return await _commentLikeRepository.GetAllCommentLikeByCommentIdAsync(commentID);
        }

        public async Task<IEnumerable<CommentLike>> GetAllCommentLikeByUserIdAsync(string userID)
        {
            if (userID == null) return null;
            return await _commentLikeRepository.GetAllCommentLikeByUserIdAsync(userID);
        }

        public async Task<CommentLike> GetCommentLikeByIdAsync(int commentLikeID)
        {
            if (commentLikeID == null) return null;
            return await _commentLikeRepository.GetCommentLikeByIdAsync(commentLikeID);
        }

        public async Task<CommentLike> GetCommentLikeByUserAndCommentIdAsync(string userID, int commentID)
        {
            if (userID == null || commentID == null) return null;
            return await _commentLikeRepository.GetCommentLikeByUserAndCommentIdAsync(userID, commentID);
        }

        public async Task<CommentLike> UpdateCommentLikeAsync(CommentLikeUpdateDTO updateCommentLike)
        {
            CommentLike updatedCommentLike = _mapper.Map<CommentLike>(updateCommentLike);
            return await _commentLikeRepository.UpdateCommentLikeAsync(updateCommentLike.Id, updatedCommentLike);
        }
    }
}
