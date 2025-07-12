using AutoMapper;
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
    public class ProblemService : IProblemService
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IMapper _mapper;
        public ProblemService(IProblemRepository problemRepository, IMapper mapper)
        {
            _problemRepository = problemRepository;
            _mapper = mapper;
        }

        public async Task<Problem> CreateProblemAsync(ProblemCreateDTO problemCreateDTO)
        {
            var problem = _mapper.Map<Problem>(problemCreateDTO);
            await _problemRepository.CreateProblemAsync(problem);
            return problem;
        }

        public async Task<bool> DeleteProblemAsync(int problemId)
        {
            return await _problemRepository.DeleteProblemAsync(problemId);
        }

        public async Task<IEnumerable<Problem>> GetAllActiveProblemsEnumerableAsync()
        {
            return await _problemRepository.GetAllActiveProblemsEnumerableAsync();
        }

        public async Task<IQueryable<Problem>> GetAllActiveProblemsQueryableAsync()
        {
            return await _problemRepository.GetAllActiveProblemsQueryableAsync();
        }

        public async Task<IEnumerable<Problem>> GetAllPassiveProblemsEnumerableAsync()
        {
            return await _problemRepository.GetAllPassiveProblemsEnumerableAsync();
        }

        public async Task<IQueryable<Problem>> GetAllPassiveProblemsQueryableAsync()
        {
            return await _problemRepository.GetAllPassiveProblemsQueryableAsync();
        }

        public async Task<IEnumerable<Problem>> GetAllPastProblemsAsync()
        {
            return await _problemRepository.GetAllPastProblemsAsync();
        }

        public async Task<Problem> GetProblemAsync(int problemId)
        {
            return await _problemRepository.GetProblemAsync(problemId);
        }

        public async Task<Problem> GetProblemByDateAsync(DateTime dateTime)
        {
            return await _problemRepository.GetProblemByDateAsync(dateTime);
        }

        public async Task<IEnumerable<Problem>> GetProblemByLikesEnumerableAsync()
        {
            return await _problemRepository.GetProblemByLikesEnumerableAsync();
        }

        public async Task<IQueryable<Problem>> GetProblemByLikesQueryableAsync()
        {
            return await _problemRepository.GetProblemByLikesQueryableAsync();
        }

        public async Task<Problem> GetProblemQuestionDateAsync()
        {
            return await _problemRepository.GetProblemQuestionDateAsync();
        }

        public async Task<IEnumerable<Problem>> GetProblemsByFilteredAsync(string filterStr)
        {
            return await _problemRepository.GetProblemsByFilteredAsync(filterStr);
        }

        public async Task<int> ProblemAnswerDownAsync(int problemID)
        {
            return await _problemRepository.ProblemAnswerDownAsync(problemID);
        }

        public async Task<int> ProblemAnswerUpAsync(int problemID)
        {
            return await _problemRepository.ProblemAnswerUpAsync(problemID);
        }

        public async Task<int> ProblemLikesDownAsync(int problemID)
        {
            return await _problemRepository.ProblemLikesDownAsync(problemID);
        }

        public async Task<int> ProblemLikesUpAsync(int problemID)
        {
            return await _problemRepository.ProblemLikesUpAsync(problemID);
        }

        public async Task<int> ProblemLikeChangeAsync(int problemID, bool isLike)
        {
            return await _problemRepository.ProblemLikeChangeAsync(problemID, isLike);
        }

        public async Task<Problem> UpdateProblemAsync(ProblemUpdateDTO updateProblem)
        {
            Problem problem = _mapper.Map<Problem>(updateProblem);
            await _problemRepository.UpdateProblemAsync(updateProblem.Id, problem);
            return problem;
        }
    }
}
