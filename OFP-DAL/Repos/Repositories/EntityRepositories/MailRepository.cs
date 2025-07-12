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
    public class MailRepository : BaseRepo<Mail>, IMailRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public MailRepository(OneFixedProblemContext oneFixedProblemContext) : base(oneFixedProblemContext)
        {
            _oneFixedProblemContext = oneFixedProblemContext;
        }

        public async Task<Mail> CreateMailAsync(Mail mail)
        {
            await _oneFixedProblemContext.AddAsync(mail);
            await _oneFixedProblemContext.SaveChangesAsync();
            return mail;
        }

        public async Task<bool> DeleteByMailIdAsync(int mailId)
        {
            if (mailId == null) return false;
            Mail mail = await _oneFixedProblemContext.Mails.FindAsync(mailId);
            mail.Status = Status.Passive;
            mail.DeletedTime = DateTime.UtcNow;
            _oneFixedProblemContext.Update(mail);
            int result = await _oneFixedProblemContext.SaveChangesAsync();
            return result == 1 ? true : false;
        }

        public async Task<bool> DeleteRangeMailAsync(List<int> mailIds)
        {
            if (mailIds == null) return false;
            foreach (int mailId in mailIds)
            {
                Mail mail = await _oneFixedProblemContext.Mails.FindAsync(mailId);
                mail.Status = Status.Passive;
                mail.DeletedTime = DateTime.UtcNow;
                _oneFixedProblemContext.Update(mail);
            }
            int result = await _oneFixedProblemContext.SaveChangesAsync();
            return result == 1 ? true : false;
        }

        public async Task<IEnumerable<Mail>> GetAllMailsAsync()
        {
            return await _oneFixedProblemContext.Mails.Where(x => x.Status == Status.Active || x.Status == Status.Updated).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<Mail> GetMailByEmailAsync(string email)
        {
            return await _oneFixedProblemContext.Mails.Where(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Mail> GetMailByIdAsync(int mailId)
        {
            return await _oneFixedProblemContext.Mails.FindAsync(mailId);
        }

    }
}
