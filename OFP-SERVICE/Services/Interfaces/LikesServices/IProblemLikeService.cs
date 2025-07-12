using OFP_CORE.Entities.Likes;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces.LikesServices
{
    public interface IProblemLikeService
    {
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>
        // CRUD İŞLEMLERİ >>>>>>


        /// <summary>
        /// PROBLEMLIKECREATEDTO nesnesi ile yeni bir PROBLEMLIKE oluşturma.
        /// </summary>
        /// <param name="problemLike"></param>
        /// <returns></returns>
        Task<ProblemLike> CreateProblemLikeAsync(ProblemLikeCreateDTO problemLike);

        /// <summary>
        /// ProblemID'ye ait ProblemLike nesnesini bulup güncelleme.
        /// </summary>
        /// <param name="updateProblemLike"></param>
        /// <returns></returns>
        Task<ProblemLike> UpdateProblemLikeAsync(ProblemLikeUpdateDTO updateProblemLike);

        /// <summary>
        /// ProblemLikeId'ye ait ProblemLike nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="problemLikeId"></param>
        /// <returns></returns>
        Task<bool> DeleteProblemLikeAsync(int problemLikeId);



        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>
        // GET İŞLEMLERİ >>>>>

        /// <summary>
        /// UserID'ye ait olan tüm ProblemLike'ları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<ProblemLike>> GetAllProblemLikeByUserIdAsync(string userID);

        /// <summary>
        /// ProblemID'ye ait olan tüm ProblemLike'ları döndürür
        /// </summary>
        /// <param name="problemLikeID"></param>
        /// <returns></returns>
        Task<IEnumerable<ProblemLike>> GetAllProblemLikeByProblemIdAsync(int problemID);

        /// <summary>
        /// UserID ve ProblemID'ye ait olan ProblemLike nesnesi döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="problemID"></param>
        /// <returns></returns>
        Task<ProblemLike> GetProblemLikeByUserAndProblemIdAsync(string userID, int problemID);

        /// <summary>
        /// ProblemLikeID'ye ait olan ProblemLike nesnesi döndürür
        /// </summary>
        /// <param name="problemLikeID"></param>
        /// <returns></returns>
        Task<ProblemLike> GetProblemLikeByIdAsync(int problemLikeID);


        /// <summary>
        /// ProblemID'ye ve UserId'ye ait olan bir Problem beğenisi var mı? KONTROL ET
        /// </summary>
        /// <param name="problemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> AnyProblemLikeAsync(int problemId, string userId);
    }
}
