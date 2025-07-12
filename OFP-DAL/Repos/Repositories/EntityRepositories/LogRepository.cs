using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities.Log;
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
    public class LogRepository : BaseRepo<Log>, ILogRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public LogRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        public async Task<Log> CreateLogAsync(Log log)
        {
            await _oneFixedProblemContext.AddAsync(log);
            await _oneFixedProblemContext.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteLogAsync(int logId)
        {
            Log log = await _oneFixedProblemContext.Logs.FindAsync(logId);
            log.Status = Status.Passive;
            log.DeletedTime = DateTime.UtcNow;
            _oneFixedProblemContext.Update(log);
            int result = await _oneFixedProblemContext.SaveChangesAsync();
            return result == 1 ? true : false;
        }

        public async Task<bool> DeleteRangeLogsAsync(List<int> logIds)
        {
            foreach(long id in logIds)
            {
                Log log = await _oneFixedProblemContext.Logs.FindAsync(id);
                log.Status = Status.Passive;
                log.DeletedTime = DateTime.UtcNow;
                _oneFixedProblemContext.Update(log);
            }
            int result = await _oneFixedProblemContext.SaveChangesAsync();
            return result == 1 ? true : false;
        }

        public async Task<IEnumerable<Log>> GetAllErrorLogsAsync()
        {
            return await _oneFixedProblemContext.Logs.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.IsError == true).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllInformationLogsAsync()
        {
            return await _oneFixedProblemContext.Logs.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.IsError == false).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogsAsync()
        {
            return await _oneFixedProblemContext.Logs.Where(x => x.Status == Status.Active || x.Status == Status.Updated).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogsByControllerNameAsync(string controllerName)
        {
            return await _oneFixedProblemContext.Logs.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.Controller.Replace(" ", "").ToLower().Contains(controllerName.Replace(" ","").ToLower())).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetAllLogsByUserIdAsync(string userId)
        {
            return await _oneFixedProblemContext.Logs.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.UserId == userId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
    }
}
