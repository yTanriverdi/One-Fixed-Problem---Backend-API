using AutoMapper;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<int> CommentLikesDownAsync(int commentID)
        {
            return await _commentRepository.CommentLikesDownAsync(commentID);
        }

        public async Task<int> CommentLikesUpAsync(int commentID)
        {
            return await _commentRepository.CommentLikesUpAsync(commentID);
        }

        public async Task<Comment> CreateCommentAsync(CommentCreateDTO comment)
        {
            Comment newComment = _mapper.Map<Comment>(comment);
            return await _commentRepository.CreateCommentAsync(newComment);
        }

        public async Task<bool> DeleteCommentAsync(int commentID)
        {
            if(commentID == null) return false;
            return await _commentRepository.DeleteCommentAsync(commentID);
        }

        public async Task<IEnumerable<Comment>> GetAllActiveCommentsAsync()
        {
            return await _commentRepository.GetAllActiveCommentsAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByAnswerIdAsync(int answerID)
        {
            return await _commentRepository.GetAllCommentsByAnswerIdAsync(answerID);
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsBySearchKeyAsync(string searchCommentKey)
        {
            if (searchCommentKey == null) return null;
            return await _commentRepository.GetAllCommentsBySearchKeyAsync(searchCommentKey);
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByUserIdAsync(string userID)
        {
            if (userID == null) return null;
            return await _commentRepository.GetAllCommentsByUserIdAsync(userID);
        }

        public async Task<Comment> GetByIdAsync(int commentID)
        {
            if (commentID == null) return null;
            return await _commentRepository.GetByIdAsync(commentID);
        }

        public async Task<Comment> UpdateCommentAsync(CommentUpdateDTO updateComment)
        {
            Comment oldComment = _mapper.Map<Comment>(updateComment);
            return await _commentRepository.UpdateCommentAsync(updateComment.Id, oldComment);
        }
    }
}
