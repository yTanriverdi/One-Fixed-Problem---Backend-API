using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// YENİ BİR MAIL OLUŞTURMA
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        Task<Mail> CreateMailAsync(MailCreateDTO mail);

        /// <summary>
        /// MailID'ye ait olan Mail nesnesini döner
        /// </summary>
        /// <returns></returns>
        Task<Mail> GetMailByIdAsync(int mailId);

        /// <summary>
        /// TÜM MAILLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Mail>> GetAllMailsAsync();

        /// <summary>
        /// Email'e göre Mail nesnesi döndürür
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Mail> GetMailByEmailAsync(string email);

        /// <summary>
        /// MailIds içerisindeki tüm ID'lerdeki Mail nesnelerini siler
        /// </summary>
        /// <param name="mailIds"></param>
        /// <returns></returns>
        Task<bool> DeleteRangeMailAsync(List<int> mailIds);

        /// <summary>
        /// Id'ye ait olan Mail nesnesini siler
        /// </summary>
        /// <param name="mailId"></param>
        /// <returns></returns>
        Task<bool> DeleteByMailIdAsync(int mailId);


        /// <summary>
        /// Mail Gönderme işlemi yapar
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
