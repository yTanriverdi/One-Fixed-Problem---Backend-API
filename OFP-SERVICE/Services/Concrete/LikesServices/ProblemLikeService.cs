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
    public class ProblemLikeService : IProblemLikeService
    {
        private readonly IProblemLikeRepository _problemLikeRepository;
        private readonly IMapper _mapper;

        public ProblemLikeService(IProblemLikeRepository problemLikeRepository, IMapper mapper)
        {
            _problemLikeRepository = problemLikeRepository;
            _mapper = mapper;
        }
        public async Task<ProblemLike> CreateProblemLikeAsync(ProblemLikeCreateDTO problemLike)
        {
            ProblemLike newProblemLike = _mapper.Map<ProblemLike>(problemLike);
            return await _problemLikeRepository.CreateProblemLikeAsync(newProblemLike);
        }

        public async Task<bool> DeleteProblemLikeAsync(int problemLikeId)
        {
            if (problemLikeId == null) return false;
            return await _problemLikeRepository.DeleteProblemLikeAsync(problemLikeId);
        }

        public async Task<IEnumerable<ProblemLike>> GetAllProblemLikeByProblemIdAsync(int problemID)
        {
            if (problemID == null) return null;
            return await _problemLikeRepository.GetAllProblemLikeByProblemIdAsync(problemID);
        }

        public async Task<IEnumerable<ProblemLike>> GetAllProblemLikeByUserIdAsync(string userID)
        {
            if (userID == null) return null;
            return await _problemLikeRepository.GetAllProblemLikeByUserIdAsync(userID);
        }

        public async Task<ProblemLike> GetProblemLikeByIdAsync(int problemLikeID)
        {
            if (problemLikeID == null) return null;
            return await _problemLikeRepository.GetProblemLikeByIdAsync(problemLikeID);
        }

        public async Task<ProblemLike> GetProblemLikeByUserAndProblemIdAsync(string userID, int problemID)
        {
            if(userID == null || problemID == null) return null;
            return await _problemLikeRepository.GetProblemLikeByUserAndProblemIdAsync(userID, problemID);
        }

        public async Task<ProblemLike> UpdateProblemLikeAsync(ProblemLikeUpdateDTO updateProblemLike)
        {
            ProblemLike updatedProblemLike = _mapper.Map<ProblemLike>(updateProblemLike);
            return await _problemLikeRepository.UpdateProblemLikeAsync(updatedProblemLike.Id, updatedProblemLike);
        }

        public async Task<bool> AnyProblemLikeAsync(int problemId, string userId)
        {
            return await _problemLikeRepository.AnyProblemLikeAsync(problemId, userId);
        }
    }
}
