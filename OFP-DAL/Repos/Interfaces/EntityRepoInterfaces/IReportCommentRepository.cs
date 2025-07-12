using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.BaseRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Repos.Interfaces.EntityRepoInterfaces
{
    public interface IReportCommentRepository : IBaseRepo<ReportComment>
    {
        /// <summary>
        /// ReportCommentID'ye ait olan ReportComment nesnesi döner
        /// </summary>
        /// <param name="reportCommentID"></param>
        /// <returns></returns>
        Task<ReportComment> GetReportByIdAsync(int reportCommentID);
        /// <summary>
        /// Tüm ReportCommentsleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportComment>> GetAllReportCommentsAsync();
        /// <summary>
        /// İnceleniyor olan ReportCommentsleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportComment>> GetAllUnderReviewReportCommentsAsync();
        /// <summary>
        /// Çözülmüş olan ReportCommentsleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportComment>> GetAllResolvedReportCommentsAsync();
        /// <summary>
        /// Reddedilmiş olan ReportCommentsleri getirir
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ReportComment>> GetAllRejectedReportCommentsAsync();

        /// <summary>
        /// Yeni bir ReportProblem oluşturma
        /// </summary>
        /// <param name="reportComment"></param>
        /// <returns></returns>
        Task<ReportComment> CreateReportCommentAsync(ReportComment reportComment);

        /// <summary>
        /// ReportCommentID'ye ait olan ReportComment nesnesini parametredeki ReportComment ile güncelleme
        /// </summary>
        /// <param name="reportID"></param>
        /// <param name="reportComment"></param>
        /// <returns></returns>
        Task<ReportComment> UpdateReportCommentAsync(int reportID, ReportComment reportComment);

        /// <summary>
        /// ReportID'ye ait olan ReportComment nesnesini PASİF OLARAK SİLER
        /// </summary>
        /// <param name="reportID"></param>
        /// <returns></returns>
        Task<ReportComment> DeleteReportCommentAsync(int reportID);
    }
}
