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
    public class ReportAnswerService : IReportAnswerService
    {
        private readonly IReportAnswerRepository _reportAnswerRepository;
        private readonly IMapper _mapper;

        public ReportAnswerService(IReportAnswerRepository reportAnswerRepository, IMapper mapper)
        {
            _reportAnswerRepository = reportAnswerRepository;
            _mapper = mapper;
        }

        public async Task<ReportAnswer> CreateReportAnswerAsync(ReportAnswerCreateDTO reportAnswer)
        {
            ReportAnswer newReportAnswer = _mapper.Map<ReportAnswer>(reportAnswer);
            return await _reportAnswerRepository.CreateReportAnswerAsync(newReportAnswer);
        }

        public async Task<ReportAnswer> DeleteReportAnswerAsync(int reportID)
        {
            if (reportID == null) return null;
            return await _reportAnswerRepository.DeleteReportAnswerAsync(reportID);
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllRejectedReportAnswersAsync()
        {
            return await _reportAnswerRepository.GetAllRejectedReportAnswersAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllReportAnswersAsync()
        {
            return await _reportAnswerRepository.GetAllReportAnswersAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllResolvedReportAnswersAsync()
        {
            return await _reportAnswerRepository.GetAllResolvedReportAnswersAsync();
        }

        public async Task<IEnumerable<ReportAnswer>> GetAllUnderReviewReportAnswersAsync()
        {
            return await _reportAnswerRepository.GetAllUnderReviewReportAnswersAsync();
        }

        public async Task<ReportAnswer> GetReportByIdAsync(int reportAnswerID)
        {
            if (reportAnswerID == null) return null;
            return await _reportAnswerRepository.GetReportByIdAsync(reportAnswerID);
        }

        public async Task<ReportAnswer> UpdateReportAnswerAsync(ReportAnswerUpdateDTO reportAnswer)
        {
            ReportAnswer updatedReportAnswer = _mapper.Map<ReportAnswer>(reportAnswer);
            return await _reportAnswerRepository.UpdateReportAnswerAsync(updatedReportAnswer.Id, updatedReportAnswer);
        }
    }
}
