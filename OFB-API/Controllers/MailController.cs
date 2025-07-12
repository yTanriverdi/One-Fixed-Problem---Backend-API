using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Log;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILogService _logService;

        public MailController(IMailService mailService, ILogService logService)
        {
            _mailService = mailService;
            _logService = logService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllMails")]
        public async Task<IActionResult> GetAllMails()
        {
            try
            {
                IEnumerable<Mail> mails = await _mailService.GetAllMailsAsync();
                if (mails == null) return NotFound("GetAllMailsAsync not found");
                await _logService.CreateLogAsync($"GetAllMailsAsync Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "MailController/GetAllMailsAsync");
                return Ok(mails);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "MailController/GetAllMailsAsync");
                return BadRequest(ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetMailById")]
        public async Task<IActionResult> GetMailById(int mailId)
        {
            if (mailId == null) return BadRequest("MailId is null");
            try
            {
                Mail mail = await _mailService.GetMailByIdAsync(mailId);
                if (mail == null) return NotFound($"Mail not found MailId: {mailId}");
                await _logService.CreateLogAsync($"GetMailById Success MailId: {mailId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "MailController/GetMailById");
                return Ok(mail);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "MailController/GetMailById");
                return BadRequest(ex.Message);
            }
        }
        
        
        
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetMailByEmail")]
        public async Task<IActionResult> GetMailByEmail(string email)
        {
            if (email == null) return BadRequest("MailId is null");
            try
            {
                Mail mail = await _mailService.GetMailByEmailAsync(email);
                if (mail == null) return NotFound($"Mail not found Email: {email}");
                await _logService.CreateLogAsync($"GetMailByEmail Success Mail: {email}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "MailController/GetMailByEmail");
                return Ok(mail);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "MailController/GetMailByEmail");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeleteRangeMailByIds")]
        public async Task<IActionResult> DeleteRangeMailByIds(List<int> mailIds)
        {
            if (mailIds.Count == 0 || mailIds == null) return BadRequest("MailIds is null");
            try
            {
                bool deletedMails = await _mailService.DeleteRangeMailAsync(mailIds);
                if (!deletedMails) return BadRequest("DeleteRangeMail Error"); 
                await _logService.CreateLogAsync("DeleteRangeMailByIds Sucess", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "MailController/DeleteRangeMailByIds");
                return Ok(true);
            } 
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "MailController/DeleteRangeMailByIds");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPut("DeleteRangeMailById")]
        public async Task<IActionResult> DeleteRangeMailById(int mailId)
        {
            if (mailId == null) return BadRequest("MailId is null");
            try
            {
                bool deletedMail = await _mailService.DeleteByMailIdAsync(mailId);
                if (!deletedMail) return BadRequest("DeleteRangeMailById Error"); 
                await _logService.CreateLogAsync("DeleteRangeMailById Sucess", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "MailController/DeleteRangeMailById");
                return Ok(true);
            } 
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "MailController/DeleteRangeMailById");
                return BadRequest(ex.Message);
            }
        }
    }
}
