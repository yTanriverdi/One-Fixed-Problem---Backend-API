using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
using OFP_CORE.Enums;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_DAL.Repos.Repositories.BaseRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Repositories.EntityRepositories
{
    public class AnswerRepository : BaseRepo<Answer>, IAnswerRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public AnswerRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }


        /// <summary>
        /// AnswerID'ye ait olan Answer nesnesinin 30 dakika süresi dolduğundan sonra cevap vermesinin engellenmesi
        /// </summary>
        /// <param name="answerId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AnswerEndAsync(int answerId)
        {
            Answer answer = await _oneFixedProblemContext.Answers.FindAsync(answerId);
            if (answer == null) return false;
            answer.UpdatedDate = DateTime.UtcNow;
            answer.Status = Status.Updated;
            answer.EndAnswer = true;
            _oneFixedProblemContext.Update(answer);
            int result = await _oneFixedProblemContext.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        /// <summary>
        /// Answer Like sayısını azaltma.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public async Task<int> AnswerLikesDownAsync(int answerID)
        {
            Answer answer = await _oneFixedProblemContext.Answers.FindAsync(answerID);
            if (answer.Likes == 0) return 0;
            answer.Likes -= 1;
            _oneFixedProblemContext.Update(answer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return answer.Likes;
        }

        /// <summary>
        /// Answer Like sayısını artırma.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public async Task<int> AnswerLikesUpAsync(int answerID)
        {
            Answer answer = await _oneFixedProblemContext.Answers.FindAsync(answerID);
            answer.Likes += 1;
            _oneFixedProblemContext.Update(answer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return answer.Likes;
        }


        /// <summary>
        /// Kullanıcı önceden probleme cevap vermiş mi? Vermişse True döner USERID VE PROBLEMID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AnyAnswerByProblemAndUserIdAsync(string userId, int problemId)
        {
            Answer answer = await _oneFixedProblemContext.Answers.Where(x => x.UserId == userId && x.ProblemId == problemId && (x.Status == Status.Active || x.Status == Status.Updated) && x.EndAnswer == false).FirstOrDefaultAsync();
            if(answer == null || answer.EndAnswer == true) return false;
            return true;
        }

        /// <summary>
        /// Yeni Answer nesnesi ekleme.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task<Answer> CreateAnswerAsync(Answer answer)
        {
            await _oneFixedProblemContext.Answers.AddAsync(answer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return answer;
        }


        /// <summary>
        /// AnswerID'ye ait olan Answer nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteAnswerAsync(int answerID)
        {
            Answer answer = await _oneFixedProblemContext.Answers.FindAsync(answerID);
            if (answer is null) return false;
            answer.Status = Status.Passive;
            answer.DeletedTime = DateTime.UtcNow;
            _oneFixedProblemContext.Update(answer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Answer>> GetAllActiveAnswersEnumerableAsync()
        {
            return await _oneFixedProblemContext.Answers.Where(x => x.Status == Status.Active || x.Status == Status.Updated).ToListAsync();
        }

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<Answer> GetAllActiveAnswersQueryable()
        {
            return _oneFixedProblemContext.Answers.Where(x => x.Status == Status.Active || x.Status == Status.Updated);
        }

        /// <summary>
        /// Tüm Pasif Answer nesnelerinin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Answer>> GetAllPassiveAnswersEnumerableAsync()
        {
            return await _oneFixedProblemContext.Answers.Where(x => x.Status == Status.Passive || x.Status == Status.Updated).ToListAsync();
        }

        /// <summary>
        /// Tüm Pasif Answer nesnelerinin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        public IQueryable<Answer> GetAllPassiveAnswersQueryable()
        {
            return _oneFixedProblemContext.Answers.Where(x => x.Status == Status.Passive || x.Status == Status.Updated);
        }


        /// <summary>
        /// AnswerID'ye ait Answer nesnesi döner.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Answer> GetAnswerByIdAsync(int answerID)
        {
            return await _oneFixedProblemContext.Answers.FindAsync(answerID);
        }

        /// <summary>
        /// ProblemID'ye ait Answer listesi döner. IENUMERABLE
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Answer>> GetAnswerByProblemIdEnumerableAsync(int problemID)
        {
            IEnumerable<Answer> list = await GetAllActiveAnswersEnumerableAsync();
            IEnumerable<Answer> newList = list.Where(x => x.ProblemId == problemID && (x.Status == Status.Active || x.Status == Status.Updated)
            );
            return newList;
        }

        /// <summary>
        /// ProblemID'ye ait Answer listesi döner. IQUERYABLE
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<Answer> GetAnswerByProblemIdQueryable(int problemID)
        {
            return _oneFixedProblemContext.Answers.Where(x => x.ProblemId == problemID);
        }

        /// <summary>
        /// UserID'ye ait olan tüm Answer listesini döner IENUMERABLE
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Answer>> GetAnswersByUserIdEnumerableAsync(string userID)
        {
            var list = await GetAllActiveAnswersEnumerableAsync();
            var newList = list.Where(x => x.UserId == userID);
            return newList;
        }

        /// <summary>
        /// UserID'ye ait olan tüm Answer listesini döner IQUERYABLE
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<Answer> GetAnswersByUserIdQueryable(string userID)
        {
            var list = GetAllActiveAnswersQueryable();
            var newList = list.Where(x => x.UserId == userID);
            return newList;
        }

        /// <summary>
        /// AnswerID'ye göre bulunan Answer nesnesini güncelleme.
        /// </summary>
        /// <param name="answerID"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Answer> UpdateAnswerAsync(int answerID, Answer answer)
        {
            Answer oldAnswer = await _oneFixedProblemContext.Answers.FindAsync(answerID);
            oldAnswer.AnswerContent = answer.AnswerContent;
            oldAnswer.Status = Status.Updated;
            oldAnswer.UpdatedDate = DateTime.UtcNow;
            _oneFixedProblemContext.Update(oldAnswer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return oldAnswer;
        }


        /// <summary>
        /// Parametredeki UserId ve ProblemId' ye ait olan Answer nesnesini döner
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public async Task<Answer> GetAnswerByUserIdAndProblemIdAsync(string userId, int problemId)
        {
            Answer answer = await _oneFixedProblemContext.Answers.Where(x => x.UserId == userId && x.ProblemId == problemId && (x.Status == Status.Active || x.Status == Status.Updated)).FirstOrDefaultAsync();
            if (answer == null) return null;
            else return answer;
        }
    }
}
