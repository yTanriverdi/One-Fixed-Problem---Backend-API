using Microsoft.EntityFrameworkCore;
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
    public class ReportCommentRepository : BaseRepo<ReportComment>, IReportCommentRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        public ReportCommentRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext) 
        {
             _oneFixedProblemContext = oneFixedProblemContext;
        }


        public async Task<ReportComment> CreateReportCommentAsync(ReportComment reportComment)
        {
            await _oneFixedProblemContext.AddAsync(reportComment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return reportComment;
        }

        public async Task<ReportComment> DeleteReportCommentAsync(int reportID)
        {
            ReportComment reportComment = await _oneFixedProblemContext.ReportComments.FindAsync(reportID);
            reportComment.Status = Status.Passive;
            reportComment.DeletedTime = DateTime.UtcNow;
            _oneFixedProblemContext.Update(reportComment);
            await _oneFixedProblemContext.SaveChangesAsync();
            return reportComment;
        }

        public async Task<IEnumerable<ReportComment>> GetAllRejectedReportCommentsAsync()
        {
            return await _oneFixedProblemContext.ReportComments.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.Rejected).ToListAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllReportCommentsAsync()
        {
            return await _oneFixedProblemContext.ReportComments.ToListAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllResolvedReportCommentsAsync()
        {
            return await _oneFixedProblemContext.ReportComments.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.Resolved).ToListAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllUnderReviewReportCommentsAsync()
        {
            return await _oneFixedProblemContext.ReportComments.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.UnderReview).ToListAsync();
        }

        public async Task<ReportComment> GetReportByIdAsync(int reportCommentID)
        {
            return await _oneFixedProblemContext.ReportComments.FindAsync(reportCommentID);
        }

        public async Task<ReportComment> UpdateReportCommentAsync(int reportID, ReportComment reportComment)
        {
            ReportComment report = await _oneFixedProblemContext.ReportComments.FindAsync(reportID);
            report.ReportType = reportComment.ReportType;
            report.ReportStatus = reportComment.ReportStatus;
            report.Status = Status.Updated;
            report.UpdatedDate = DateTime.UtcNow;
            _oneFixedProblemContext.Update(report);
            await _oneFixedProblemContext.SaveChangesAsync();
            return report;
        }
    }
}
