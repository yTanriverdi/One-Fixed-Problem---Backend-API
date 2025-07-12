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
    public class AnswerLikeService : IAnswerLikeService
    {
        private readonly IAnswerLikeRepository _answerLikeRepository;
        private readonly IMapper _mapper;

        public AnswerLikeService(IAnswerLikeRepository answerLikeRepository, IMapper mapper)
        {
            _answerLikeRepository = answerLikeRepository;
            _mapper = mapper;
        }
        public async Task<AnswerLike> CreateAnswerLikeAsync(AnswerLikeCreateDTO answerLike)
        {
            AnswerLike newAnswerLike = _mapper.Map<AnswerLike>(answerLike);
            return await _answerLikeRepository.CreateAnswerLikeAsync(newAnswerLike);
        }

        public async Task<bool> DeleteAnswerLikeAsync(int answerLikeID)
        {
            if (answerLikeID == null) return false;
            return await _answerLikeRepository.DeleteAnswerLikeAsync(answerLikeID);
        }

        public async Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByAnswerIdAsync(int answerID)
        {
            if (answerID == null) return null;
            return await _answerLikeRepository.GetAllAnswerLikeByAnswerIdAsync(answerID);
        }

        public async Task<IEnumerable<AnswerLike>> GetAllAnswerLikeByUserIdAsync(string userID)
        {
            if (userID == null) return null;
            return await _answerLikeRepository.GetAllAnswerLikeByUserIdAsync(userID);
        }

        public async Task<AnswerLike> GetAnswerLikeById(int answerLikeID)
        {
            if (answerLikeID == null) return null;
            return await _answerLikeRepository.GetAnswerLikeById(answerLikeID);
        }

        public async Task<AnswerLike> GetAnswerLikeByUserAndAnswerIdAsync(string userID, int answerID)
        {
            if (userID == null || answerID == null) return null;
            return await _answerLikeRepository.GetAnswerLikeByUserAndAnswerIdAsync(userID, answerID);
        }

        public async Task<AnswerLike> UpdateAnswerLikeAsync(AnswerLikeUpdateDTO answerLike)
        {
            AnswerLike updatedAnswerLike = _mapper.Map<AnswerLike>(answerLike);
            return await _answerLikeRepository.UpdateAnswerLikeAsync(updatedAnswerLike.Id, updatedAnswerLike);
        }
    }
}
