using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class ProblemRepository : BaseRepo<Problem>, IProblemRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        public ProblemRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        /// <summary>
        /// Yeni problem oluşturma işlemi yapar.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<Problem> CreateProblemAsync(Problem problem)
        {
            Problem lastProblem = await _oneFixedProblemContext.Problems.Where(x => x.Status == Status.Active || x.Status == Status.Updated).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                problem.QuestionDate = lastProblem.QuestionDate.AddDays(1);
                problem.CreatedDate = DateTime.UtcNow;
            await _oneFixedProblemContext.Problems.AddAsync(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problem;
        }


        /// <summary>
        /// Problem silme işlemi yapar.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProblemAsync(int problemId)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemId);
            if(problem is null) return false;
            problem.DeletedTime = DateTime.UtcNow;
            problem.Status = Status.Passive;
            _oneFixedProblemContext.Update(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// ID'ye ait olan PROBLEM'i döndürür.
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public async Task<Problem> GetProblemAsync(int problemId)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemId);
            if (problem is null) return null;
            return problem;
        }


        /// <summary>
        /// DATETIME'a ait olan PROBLEM'i döndürür.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<Problem> GetProblemByDateAsync(DateTime dateTime)
        {
            DateTime dateOnly = dateTime.Date;
            Problem problem = await _oneFixedProblemContext.Problems.Where(x => x.QuestionDate.Date == dateOnly).FirstOrDefaultAsync();
            if (problem is null) return null;
            return problem;
        }


        /// <summary>
        /// Like'ı en fazladan düşüğe olacak şekilde PROBLEM listesi döner. ENUMERABLE
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Problem>> GetProblemByLikesEnumerableAsync()
        {
            IEnumerable<Problem> enumerableProblems = await _oneFixedProblemContext.Problems.OrderByDescending(x => x.Likes).ToListAsync();
            return enumerableProblems;
        }


        /// <summary>
        /// Like'ı en fazladan düşüğe olacak şekilde PROBLEM listesi döner. QUERYABLE
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IQueryable<Problem>> GetProblemByLikesQueryableAsync()
        {
            IQueryable<Problem> queryableProblems = _oneFixedProblemContext.Problems.OrderByDescending(x => x.Likes);
            return queryableProblems;
        }


        /// <summary>
        /// Soruyu sorulacağı güne ait olanı getirir.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Problem> GetProblemQuestionDateAsync()
        {
            DateTime dateTime = DateTime.UtcNow;
            Problem problem = await _oneFixedProblemContext.Problems.FirstOrDefaultAsync(x => x.QuestionDate.Date == dateTime.Date);
            if (problem is null) return null;
            if (!problem.Viewed)
            {
                problem.Viewed = true;
                _oneFixedProblemContext.Update(problem);
                await _oneFixedProblemContext.SaveChangesAsync();
            }
            return problem;
        }


        /// <summary>
        /// PROBLEM güncelleme işlemi yapar.
        /// </summary>
        /// <param name="problemID">Güncellenecek olan problemID</param>
        /// <param name="problem">Güncellenecek olan Problem nesnesi</param>
        /// <returns></returns>
        public async Task<Problem> UpdateProblemAsync(int problemID, Problem problem)
        {
            Problem oldProblem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            oldProblem.Status = Status.Updated;
            oldProblem.Likes = problem.Likes;
            oldProblem.Content = problem.Content;
            oldProblem.UpdatedDate = DateTime.UtcNow;
            oldProblem.AnswerCount = problem.AnswerCount;
            _oneFixedProblemContext.Update(oldProblem);
            await _oneFixedProblemContext.SaveChangesAsync(); 
            return oldProblem;
        }


        /// <summary>
        /// PROBLEM için beğenme sayısını artırır.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public async Task<int> ProblemLikesUpAsync(int problemID)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            problem.Likes += 1;
            _oneFixedProblemContext.Update(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problem.Likes;
        }
        
        
        /// <summary>
        /// PROBLEM için beğenme sayısını artırır.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public async Task<int> ProblemLikesDownAsync(int problemID)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            if (problem.Likes == 0) return 0;
            problem.Likes -= 1;
            _oneFixedProblemContext.Update(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problem.Likes;
        }


        /// <summary>
        /// Problem nesnesinin beğenmesini Duruma göre azaltır veya artırır.
        /// </summary>
        /// <param name="problemID"></param>
        /// <param name="isLike"></param>
        /// <returns></returns>
        public async Task<int> ProblemLikeChangeAsync(int problemID, bool isLike)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            if (!isLike)
            {
                if (problem.Likes == 0) return 0;
                problem.Likes -= 1;
                _oneFixedProblemContext.Update(problem);
                await _oneFixedProblemContext.SaveChangesAsync();
                return problem.Likes;
            }
            else
            {
                problem.Likes += 1;
                _oneFixedProblemContext.Update(problem);
                await _oneFixedProblemContext.SaveChangesAsync();
                return problem.Likes;
            }
        }


        /// <summary>
        /// Problem cevaplanma sayısını artırma
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public async Task<int> ProblemAnswerUpAsync(int problemID)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            problem.AnswerCount += 1;
            _oneFixedProblemContext.Update(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problem.AnswerCount;
        }

        /// <summary>
        /// Aranan kelimeye göre IEnumerable<Problem> Listesi döner
        /// </summary>
        /// <param name="filterStr"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Problem>> GetProblemsByFilteredAsync(string filterStr)
        {
            IEnumerable<Problem> filteredProblems = await _oneFixedProblemContext.Problems.Where(x => x.Content.Replace(" ", "").ToLower().Contains(filterStr.Replace(" ", "").ToLower()) && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
            if (filteredProblems is null) return null;
            return filteredProblems;
        }



        /// <summary>
        /// Aktif olan tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Problem>> GetAllActiveProblemsEnumerableAsync()
        {
            return await _oneFixedProblemContext.Problems.Where(x => x.Status == Status.Active || x.Status == Status.Updated).ToListAsync();
        }


        /// <summary>
        /// Aktif olan tüm Problemlerin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<Problem>> GetAllActiveProblemsQueryableAsync()
        {
            return _oneFixedProblemContext.Problems.Where(x => x.Status == Status.Active || x.Status == Status.Updated);
        }


        /// <summary>
        /// Aktif olan tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Problem>> GetAllPassiveProblemsEnumerableAsync()
        {
            return await _oneFixedProblemContext.Problems.Where(x => x.Status == Status.Passive).ToListAsync();
        }


        /// <summary>
        /// Pasif olan tüm Problemlerin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<Problem>> GetAllPassiveProblemsQueryableAsync()
        {
            return _oneFixedProblemContext.Problems.Where(x => x.Status == Status.Passive);
        }


        /// <summary>
        /// Geçmiş tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Problem>> GetAllPastProblemsAsync()
        {
            return await _oneFixedProblemContext.Problems.Where(x => x.Viewed == true && x.QuestionDate.Date <= DateTime.Now.Date).ToListAsync();
        }

        /// <summary>
        /// Problem cevaplanma sayısını azaltma.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public async Task<int> ProblemAnswerDownAsync(int problemID)
        {
            Problem problem = await _oneFixedProblemContext.Problems.FindAsync(problemID);
            if (problem.AnswerCount == 0) return 0;
            problem.AnswerCount -= 1;
            _oneFixedProblemContext.Update(problem);
            await _oneFixedProblemContext.SaveChangesAsync();
            return problem.AnswerCount;
        }
    }
}
