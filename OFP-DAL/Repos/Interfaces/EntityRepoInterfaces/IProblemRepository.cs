using OFP_CORE.Entities;
using OFP_CORE.EntitiesInterfaces;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface IProblemRepository : IBaseRepo<Problem>
    {
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>


        /// <summary>
        /// PROBLEM nesnesi ile yeni bir PROBLEM oluşturma.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        Task<Problem> CreateProblemAsync(Problem problem);

        /// <summary>
        /// ProblemID'ye ait Problem nesnesini bulup güncelleme.
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="updateProblem"></param>
        /// <returns></returns>
        Task<Problem> UpdateProblemAsync(int problemId, Problem updateProblem);

        /// <summary>
        /// ProblemID'ye ait Problem nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        Task<bool> DeleteProblemAsync(int problemId);
        

        // GET İSTEKLERİ >>>>>>
        // GET İSTEKLERİ >>>>>>
        // GET İSTEKLERİ >>>>>>
        // GET İSTEKLERİ >>>>>>
        // GET İSTEKLERİ >>>>>>
        Task<Problem> GetProblemAsync(int problemId);
        Task<Problem> GetProblemByDateAsync(DateTime dateTime);
        Task<Problem> GetProblemQuestionDateAsync();



        /// <summary>
        /// Like'ı en fazladan düşüğe olacak şekilde PROBLEM listesi döner. ENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Problem>> GetProblemByLikesEnumerableAsync();

        /// <summary>
        /// Like'ı en fazladan düşüğe olacak şekilde PROBLEM listesi döner. QUERYABLE
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<Problem>> GetProblemByLikesQueryableAsync();


        /// <summary>
        /// Aktif olan tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Problem>> GetAllActiveProblemsEnumerableAsync();
        
        
        /// <summary>
        /// Aktif olan tüm Problemlerin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<Problem>> GetAllActiveProblemsQueryableAsync();


        /// <summary>
        /// Pasif olan tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Problem>> GetAllPassiveProblemsEnumerableAsync();

        /// <summary>
        /// Pasif olan tüm Problemlerin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<Problem>> GetAllPassiveProblemsQueryableAsync();



        // EK İŞLEMLER >>>>>>
        // EK İŞLEMLER >>>>>>
        // EK İŞLEMLER >>>>>>
        // EK İŞLEMLER >>>>>>
        // EK İŞLEMLER >>>>>>

        /// <summary>
        /// Problem Like sayısını artırma.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public Task<int> ProblemLikesUpAsync(int problemID);

        /// <summary>
        /// Problem Like sayısını azaltma.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public Task<int> ProblemLikesDownAsync(int problemID);

        /// <summary>
        /// Problem cevaplanma sayısını artırma
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public Task<int> ProblemAnswerUpAsync(int problemID);

        /// <summary>
        /// Problem nesnesinin beğenmesini Duruma göre azaltır veya artırır.
        /// </summary>
        /// <param name="problemID"></param>
        /// <param name="isLike"></param>
        /// <returns></returns>
        public Task<int> ProblemLikeChangeAsync(int problemID, bool isLike);
        /// <summary>
        /// Problem cevaplanma sayısını azaltma.
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        public Task<int> ProblemAnswerDownAsync(int problemID);

        /// <summary>
        /// Aranan kelimeye göre IEnumerable<Problem> Listesi döner
        /// </summary>
        /// <param name="filterStr"></param>
        /// <returns></returns>
        public Task<IEnumerable<Problem>> GetProblemsByFilteredAsync(string filterStr);

        /// <summary>
        /// Geçmiş tüm Problemlerin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Problem>> GetAllPastProblemsAsync();
    }
}
       