using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface IReportAnswerRepository : IBaseRepo<ReportAnswer>
    {
        /// <summary>
        /// ReportAnswerID'ye ait olan ReportAnswer nesnesi döner
        /// </summary>
        /// <param name="reportAnswerID"></param>
        /// <returns></returns>
        Task<ReportAnswer> GetReportByIdAsync(int reportAnswerID);
        /// <summary>
        /// Tüm ReportAnswerleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportAnswer>> GetAllReportAnswersAsync();
        /// <summary>
        /// İnceleniyor olan ReportAnswerleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportAnswer>> GetAllUnderReviewReportAnswersAsync();
        /// <summary>
        /// Çözülmüş olan ReportAnswerleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportAnswer>> GetAllResolvedReportAnswersAsync();
        /// <summary>
        /// Reddedilmiş olan ReportAnswerleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportAnswer>> GetAllRejectedReportAnswersAsync();

        /// <summary>
        /// Yeni bir ReportAnswer oluşturma
        /// </summary>
        /// <param name="reportAnswer"></param>
        /// <returns></returns>
        Task<ReportAnswer> CreateReportAnswerAsync(ReportAnswer reportAnswer);

        /// <summary>
        /// ReportAnswerID'ye ait olan ReportAnswer nesnesini parametredeki ReportAnswer ile güncelleme
        /// </summary>
        /// <param name="reportID"></param>
        /// <param name="reportAnswer"></param>
        /// <returns></returns>
        Task<ReportAnswer> UpdateReportAnswerAsync(int reportID, ReportAnswer reportAnswer);

        /// <summary>
        /// ReportID'ye ait olan ReportAnswer nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        Task<ReportAnswer> DeleteReportAnswerAsync(int reportID);
    }
}
