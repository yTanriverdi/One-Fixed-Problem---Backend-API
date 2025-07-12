using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces
{
    public interface IAnswerService
    {
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>


        /// <summary>
        /// Yeni Answer nesnesi ekleme
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        Task<Answer> CreateAnswerAsync(AnswerCreateDTO answer);

        /// <summary>
        /// AnswerID'ye göre bulunan Answer nesnesini güncelleme.
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        Task<Answer> UpdateAnswerAsync(AnswerUpdateDTO answer);

        /// <summary>
        /// AnswerID'ye göre bulunan Answer nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task<bool> DeleteAnswerAsync(int answerID);



        // GET İŞLEMLERİ >>>>>>
        // GET İŞLEMLERİ >>>>>>
        // GET İŞLEMLERİ >>>>>>
        // GET İŞLEMLERİ >>>>>>
        // GET İŞLEMLERİ >>>>>>

        /// <summary>
        /// AnswerID'ye ait Answer nesnesi döner.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        Task<Answer> GetAnswerByIdAsync(int answerID);

        /// <summary>
        /// ProblemID'ye ait Answer listesi döner. IENUMERABLE
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        Task<IEnumerable<Answer>> GetAnswerByProblemIdEnumerableAsync(int problemID);

        /// <summary>
        /// ProblemID'ye ait Answer listesi döner. IQUERYABLE
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        IQueryable<Answer> GetAnswerByProblemIdQueryable(int problemID);

        /// <summary>
        /// UserID'ye ait olan tüm Answer listesini döner IENUMERABLE
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<Answer>> GetAnswersByUserIdEnumerableAsync(string userID);

        /// <summary>
        /// UserID'ye ait olan tüm Answer listesini döner IQUERYABLE
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IQueryable<Answer> GetAnswersByUserIdQueryable(string userID);

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Answer>> GetAllActiveAnswersEnumerableAsync();

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Answer>> GetAllPassiveAnswersEnumerableAsync();

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        IQueryable<Answer> GetAllActiveAnswersQueryable();

        /// <summary>
        /// Tüm Aktif Answer nesnelerinin listesini döner IQUERYABLE
        /// </summary>
        /// <returns></returns>
        IQueryable<Answer> GetAllPassiveAnswersQueryable();

        /// <summary>
        /// Answer Like sayısını artırma.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public Task<int> AnswerLikesUpAsync(int answerID);

        /// <summary>
        /// Answer Like sayısını azaltma.
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public Task<int> AnswerLikesDownAsync(int answerID);


        /// <summary>
        /// Kullanıcı önceden probleme cevap vermiş mi? Vermişse True döner USERID VE PROBLEMID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public Task<bool> AnyAnswerByProblemAndUserIdAsync(string userId, int problemId);


        /// <summary>
        /// AnswerID'ye ait olan Answer nesnesinin 30 dakika süresi dolduğundan sonra cevap vermesinin engellenmesi
        /// </summary>
        /// <param name="answerId"></param>
        /// <returns></returns>
        public Task<bool> AnswerEndAsync(int answerId);

        /// <summary>
        /// Parametredeki UserId ve ProblemId' ye ait olan Answer nesnesini döner
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public Task<Answer> GetAnswerByUserIdAndProblemIdAsync(string userId, int problemId);
    }
}
