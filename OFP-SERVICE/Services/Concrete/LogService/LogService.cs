using AutoMapper;
using OFP_CORE.Entities.Log;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.Services.Interfaces.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete.LogService
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<Log> CreateLogAsync(string logContent, string userId = null, bool isError = false, string controller = null)
        {
            Log newLog = new Log()
            {
                LogContent = logContent,
                IsError = isError,
                Controller = controller,
                UserId = userId
                
            };
            await _logRepository.CreateLogAsync(newLog);
            return newLog;
        }

        public async Task<bool> DeleteLogAsync(int logId)
        {
            if(logId == null) return false;
            return await _logRepository.DeleteLogAsync(logId);
        }

        public async Task<bool> DeleteRangeLogsAsync(List<int> logIds)
        {
            if (logIds == null) return false;
            return await _logRepository.DeleteRangeLogsAsync(logIds);
        }

        public async Task<IEnumerable<Log>> GetAllErrorLogsAsync()
        {
            return await _logRepository.GetAllErrorLogsAsync();
        }

        public async Task<IEnumerable<Log>> GetAllInformationLogsAsync()
        {
            return await _logRepository.GetAllInformationLogsAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllLogsAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogsByControllerNameAsync(string controllerName)
        {
            if (controllerName == null) return null;
            return await _logRepository.GetAllLogsByControllerNameAsync(controllerName);
        }

        public async Task<IEnumerable<Log>> GetAllLogsByUserIdAsync(string userId)
        {
            if (userId == null) return null;
            return await _logRepository.GetAllLogsByUserIdAsync(userId);
        }
    }
}
