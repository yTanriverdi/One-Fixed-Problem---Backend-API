using AutoMapper;
using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs.Reports;
using OFP_SERVICE.DTOs.UpdateDTOs.Reports;
using OFP_SERVICE.Services.Interfaces.ReportsServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete.ReportsServices
{
    public class ReportCommentService : IReportCommentService
    {
        private readonly IReportCommentRepository _reportCommentRepository;
        private readonly IMapper _mapper;

        public ReportCommentService(IReportCommentRepository reportCommentRepository, IMapper mapper)
        {
            _reportCommentRepository = reportCommentRepository;
            _mapper = mapper;
        }

        public async Task<ReportComment> CreateReportCommentAsync(ReportCommentCreateDTO reportComment)
        {
            ReportComment newReportComment = _mapper.Map<ReportComment>(reportComment);
            return await _reportCommentRepository.CreateReportCommentAsync(newReportComment);
        }

        public async Task<ReportComment> DeleteReportCommentAsync(int reportID)
        {
            if (reportID == null) return null;
            return await _reportCommentRepository.DeleteReportCommentAsync(reportID);
        }

        public async Task<IEnumerable<ReportComment>> GetAllRejectedReportCommentsAsync()
        {
            return await _reportCommentRepository.GetAllRejectedReportCommentsAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllReportCommentsAsync()
        {
            return await _reportCommentRepository.GetAllReportCommentsAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllResolvedReportCommentsAsync()
        {
            return await _reportCommentRepository.GetAllResolvedReportCommentsAsync();
        }

        public async Task<IEnumerable<ReportComment>> GetAllUnderReviewReportCommentsAsync()
        {
            return await _reportCommentRepository.GetAllUnderReviewReportCommentsAsync();
        }

        public async Task<ReportComment> GetReportByIdAsync(int reportCommentID)
        {
            if (reportCommentID == null) return null;
            return await _reportCommentRepository.GetReportByIdAsync(reportCommentID);
        }

        public async Task<ReportComment> UpdateReportCommentAsync(ReportCommentUpdateDTO reportComment)
        {
            ReportComment updatedReportComment = _mapper.Map<ReportComment>(reportComment);
            return await _reportCommentRepository.UpdateReportCommentAsync(updatedReportComment.Id, updatedReportComment);
        }
    }
}
