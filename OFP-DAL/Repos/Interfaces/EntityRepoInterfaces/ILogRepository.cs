using OFP_CORE.Entities.Log;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface ILogRepository : IBaseRepo<Log>
    {
        /// <summary>
        /// TÜM LOGLARI DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetAllLogsAsync();

        /// <summary>
        /// TÜM HATA LOGLARINI DÖNDÜRÜR YENİDEN ESKİYE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetAllErrorLogsAsync();

        /// <summary>
        /// TÜM BİLGİ LOGLARINI DÖNDÜRÜR YENİDEN ESKİYE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetAllInformationLogsAsync();

        /// <summary>
        /// LogID listesine göre Logları siler
        /// </summary>
        /// <param name="logIds"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeLogsAsync(List<int> logIds);

        /// <summary>
        /// LogId'ye ait olan LOG nesnesini siler
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        Task<bool> DeleteLogAsync(int logId);

        /// <summary>
        /// YENİ LOG EKLEME
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task<Log> CreateLogAsync(Log log);

        /// <summary>
        /// ControllerName'e ait olan tüm LOGLARI DÖNDÜRÜR
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetAllLogsByControllerNameAsync(string controllerName);

        /// <summary>
        /// UserId'ye ait olan tüm LOGLARI DÖNDÜRÜR
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetAllLogsByUserIdAsync(string userId);
    }
}
