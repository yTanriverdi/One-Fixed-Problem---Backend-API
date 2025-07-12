using AutoMapper;
using Castle.Core.Smtp;
using Microsoft.Extensions.Configuration;
using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;
        private readonly IMapper _mapper;
        private readonly SmtpClient _smtpClient;

        public MailService(IMailRepository mailRepository, IMapper mapper, IConfiguration configuration)
        {
            _mailRepository = mailRepository;
            _mapper = mapper;

            var smtpSettings = configuration.GetSection("SmtpSettings");

            _smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public Task<Mail> CreateMailAsync(MailCreateDTO mail)
        {
            Mail newMail = _mapper.Map<Mail>(mail);
            return _mailRepository.CreateMailAsync(newMail);
        }

        public async Task<bool> DeleteByMailIdAsync(int mailId)
        {
            if (mailId == null) return false;
            return await _mailRepository.DeleteByMailIdAsync(mailId);
        }

        public async Task<bool> DeleteRangeMailAsync(List<int> mailIds)
        {
            if (mailIds == null) return false;
            return await _mailRepository.DeleteRangeMailAsync(mailIds);
        }

        public async Task<IEnumerable<Mail>> GetAllMailsAsync()
        {
            return await _mailRepository.GetAllMailsAsync();
        }

        public async Task<Mail> GetMailByEmailAsync(string email)
        {
            if (email == null) return null;
            return await _mailRepository.GetMailByEmailAsync(email);
        }

        public async Task<Mail> GetMailByIdAsync(int mailId)
        {
            if (mailId == null) return null;
            return await _mailRepository.GetMailByIdAsync(mailId);
        }

        
    }
}
