using AutoMapper;
using OFP_CORE.Entities;
using OFP_CORE.Enums;
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
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnswerEndAsync(int answerId)
        {
            return await _answerRepository.AnswerEndAsync(answerId);
        }

        public async Task<int> AnswerLikesDownAsync(int answerID)
        {
            return await _answerRepository.AnswerLikesDownAsync(answerID);
        }

        public async Task<int> AnswerLikesUpAsync(int answerID)
        {
            return await _answerRepository.AnswerLikesUpAsync(answerID);
        }

        public async Task<bool> AnyAnswerByProblemAndUserIdAsync(string userId, int problemId)
        {
            return await _answerRepository.AnyAnswerByProblemAndUserIdAsync(userId, problemId);
        }

        public async Task<Answer> CreateAnswerAsync(AnswerCreateDTO answer)
        {
            Answer newAnswer = _mapper.Map<Answer>(answer);
            await _answerRepository.CreateAnswerAsync(newAnswer);
            return newAnswer;
        }

        public async Task<bool> DeleteAnswerAsync(int answerID)
        {
            return await _answerRepository.DeleteAnswerAsync(answerID);
        }

        public async Task<IEnumerable<Answer>> GetAllActiveAnswersEnumerableAsync()
        {
            return await _answerRepository.GetAllActiveAnswersEnumerableAsync();
        }

        public IQueryable<Answer> GetAllActiveAnswersQueryable()
        {
            return _answerRepository.GetAllActiveAnswersQueryable();
        }

        public async Task<IEnumerable<Answer>> GetAllPassiveAnswersEnumerableAsync()
        {
            return await _answerRepository.GetAllPassiveAnswersEnumerableAsync();
        }

        public IQueryable<Answer> GetAllPassiveAnswersQueryable()
        {
            return _answerRepository.GetAllPassiveAnswersQueryable();
        }

        public async Task<Answer> GetAnswerByIdAsync(int answerID)
        {
            return await _answerRepository.GetAnswerByIdAsync(answerID);
        }

        public async Task<IEnumerable<Answer>> GetAnswerByProblemIdEnumerableAsync(int problemID)
        {
            return await _answerRepository.GetAnswerByProblemIdEnumerableAsync(problemID);
        }

        public IQueryable<Answer> GetAnswerByProblemIdQueryable(int problemID)
        {
            return _answerRepository.GetAnswerByProblemIdQueryable(problemID);
        }

        public async Task<IEnumerable<Answer>> GetAnswersByUserIdEnumerableAsync(string userID)
        {
            return await _answerRepository.GetAnswersByUserIdEnumerableAsync(userID);
        }

        public IQueryable<Answer> GetAnswersByUserIdQueryable(string userID)
        {
            return _answerRepository.GetAnswersByUserIdQueryable(userID);
        }

        public async Task<Answer> UpdateAnswerAsync(AnswerUpdateDTO answer)
        {
            Answer oldAnswer = _mapper.Map<Answer>(answer);
            oldAnswer.Status = Status.Updated;
            oldAnswer.UpdatedDate = DateTime.UtcNow;
            return await _answerRepository.UpdateAnswerAsync(answer.Id, oldAnswer);
        }


        public async Task<Answer> GetAnswerByUserIdAndProblemIdAsync(string userId, int problemId)
        {
            return await _answerRepository.GetAnswerByUserIdAndProblemIdAsync(userId, problemId);
        }
    }
}
