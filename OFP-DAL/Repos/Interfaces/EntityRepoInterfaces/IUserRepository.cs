using OFP_CORE.Entities;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Tüm Kullanıcıları döndürür.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<BaseUser>> GetAllUsersAsync();

        /// <summary>
        /// UserId'ye ait BaseUser döndürür.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseUser> GetUserByIdAsync(string userId);

        /// <summary>
        /// UserID' ye ait olan tüm Answerları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IEnumerable<Answer>> GetAllAnswersByUserAsync(string userID);

        /// <summary>
        /// Region'a göre ülkelere göre BaseUser listesi döner
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseUser>> GetAllUserByRegion(string region);

        /// <summary>
        /// Region'a göre hangi ülkeden kaç kişi kayıtlı onu bulmak
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Task<int> GetAllCountUserByRegion(string region);

        /// <summary>
        /// Gender'a göre hangi cinsiyetten kaç kişi kullanıyor onu döner.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        Task<int> GetAllCountUserByGender(string gender);

        /// <summary>
        /// UserID'ye ait olan BaseUser'in Likes'ını azaltır.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> UserLikesDownAsync(string userId);
        
        /// <summary>
        /// UserID'ye ait olan BaseUser'in Likes'ını artırır.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> UserLikesUpAsync(string userId);

        /// <summary>
        /// Kullanıcı için Email onaylaması yapar.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UserEmailConfirmAsync(string userId);
    }
}