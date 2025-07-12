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
    public class ReportAnswerRepository : BaseRepo<ReportAnswer>, IReportAnswerRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;
        public ReportAnswerRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        public async Task<ReportAnswer> CreateReportAnswerAsync(ReportAnswer reportAnswer)
        {
            await _oneFixedProblemContext.AddAsync(reportAnswer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return reportAnswer;
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllReportAnswersAsync()
        {
            return await _oneFixedProblemContext.ReportAnswers.ToListAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllResolvedReportAnswersAsync()
        {
            return await _oneFixedProblemContext.ReportAnswers.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.Resolved).ToListAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllRejectedReportAnswersAsync()
        {
            return await _oneFixedProblemContext.ReportAnswers.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.Rejected).ToListAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllUnderReviewReportAnswersAsync()
        {
            return await _oneFixedProblemContext.ReportAnswers.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.ReportStatus == ReportStatus.UnderReview).ToListAsync();
        }

        public async Task<ReportAnswer> GetReportByIdAsync(int reportAnswerID)
        {
            return await _oneFixedProblemContext.ReportAnswers.FindAsync(reportAnswerID);
        }

        public async Task<ReportAnswer> UpdateReportAnswerAsync(int reportID, ReportAnswer reportAnswer)
        {
            ReportAnswer report = await _oneFixedProblemContext.ReportAnswers.FindAsync(reportID);
            report.UpdatedDate = DateTime.UtcNow;
            report.Status = Status.Updated;
            report.ReportStatus = reportAnswer.ReportStatus;
            report.ReportType = reportAnswer.ReportType;
            _oneFixedProblemContext.Update(report);
            await _oneFixedProblemContext.SaveChangesAsync();
            return report;
        }

        public async Task<ReportAnswer> DeleteReportAnswerAsync(int reportID)
        {
            ReportAnswer reportAnswer = await _oneFixedProblemContext.ReportAnswers.FindAsync(reportID);
            reportAnswer.Status = Status.Passive;
            reportAnswer.DeletedTime = DateTime.UtcNow;
            _oneFixedProblemContext.Update(reportAnswer);
            await _oneFixedProblemContext.SaveChangesAsync();
            return reportAnswer;
        }
    }
}
