using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs.Reports;
using OFP_SERVICE.DTOs.UpdateDTOs.Reports;
using OFP_SERVICE.Services.Interfaces.LogService;
using OFP_SERVICE.Services.Interfaces.ReportsServices;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportAnswerController : ControllerBase
    {
        private readonly IReportAnswerService _reportAnswerService;
        private readonly ILogService _logService;

        public ReportAnswerController(IReportAnswerService reportAnswerService, ILogService logService)
        {
            _reportAnswerService = reportAnswerService;
            _logService = logService;
        }

        [Authorize]
        [HttpPost("CreateReportAnswer")]
        public async Task<IActionResult> CreateReportAnswer([FromBody] ReportAnswerCreateDTO reportAnswerCreateDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                ReportAnswer reportAnswer = await _reportAnswerService.CreateReportAnswerAsync(reportAnswerCreateDTO);
                if(reportAnswer == null) return BadRequest("CreateReportAnswer Error");
                await _logService.CreateLogAsync($"CreateReportAnswer Success {reportAnswer.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/CreateReportAnswer");
                return Ok(true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/CreateReportAnswer");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetReportAnswerById")]
        public async Task<IActionResult> GetReportAnswerById(int reportAnswerId)
        {
            if (reportAnswerId == null) return BadRequest("ReportAnswerId is null");
            try
            {
                ReportAnswer reportAnswer = await _reportAnswerService.GetReportByIdAsync(reportAnswerId);
                if (reportAnswer == null) return NotFound($"ReportAnswer not found ReportAnswerId: {reportAnswerId}");
                await _logService.CreateLogAsync($"GetReportAnswerById Success {reportAnswerId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/GetReportAnswerById");
                return Ok(reportAnswer);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/GetReportAnswerById");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllReportAnswers")]
        public async Task<IActionResult> GetAllReportAnswers()
        {
            try
            {
                IEnumerable<ReportAnswer> reportAnswers = await _reportAnswerService.GetAllReportAnswersAsync();
                if (reportAnswers == null) return NotFound("ReportAnswers not found");
                await _logService.CreateLogAsync($"GetAllReportAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/GetAllReportAnswers");
                return Ok(reportAnswers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/GetAllReportAnswers");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUnderReviewReportAnswers")]
        public async Task<IActionResult> GetAllUnderReviewReportAnswers()
        {
            try
            {
                IEnumerable<ReportAnswer> reportAnswers = await _reportAnswerService.GetAllUnderReviewReportAnswersAsync();
                if (reportAnswers == null) return NotFound("UnderReviewReportAnswers not found");
                await _logService.CreateLogAsync($"GetAllUnderReviewReportAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/GetAllUnderReviewReportAnswers");
                return Ok(reportAnswers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/GetAllUnderReviewReportAnswers");
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllResolvedReportAnswers")]
        public async Task<IActionResult> GetAllResolvedReportAnswers()
        {
            try
            {
                IEnumerable<ReportAnswer> reportAnswers = await _reportAnswerService.GetAllResolvedReportAnswersAsync();
                if (reportAnswers == null) return NotFound("ResolvedReportAnswers not found");
                await _logService.CreateLogAsync($"GetAllResolvedReportAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/GetAllResolvedReportAnswers");
                return Ok(reportAnswers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/GetAllResolvedReportAnswers");
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRejectedReportAnswers")]
        public async Task<IActionResult> GetAllRejectedReportAnswers()
        {
            try
            {
                IEnumerable<ReportAnswer> reportAnswers = await _reportAnswerService.GetAllRejectedReportAnswersAsync();
                if (reportAnswers == null) return NotFound("RejectedReportAnswers not found");
                await _logService.CreateLogAsync($"GetAllRejectedReportAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/GetAllRejectedReportAnswers");
                return Ok(reportAnswers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/GetAllRejectedReportAnswers");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeleteReportAnswer")]
        public async Task<IActionResult> DeleteReportAnswer(int reportAnswerId)
        {
            if (reportAnswerId == null) return BadRequest("ReportAnswerId is null");
            try
            {
                ReportAnswer deleteReportAnswerResult = await _reportAnswerService.DeleteReportAnswerAsync(reportAnswerId);
                if (deleteReportAnswerResult == null) return BadRequest("DeleteReportAnswer Error");
                await _logService.CreateLogAsync($"DeleteReportAnswer Success ReportAnswerId: {reportAnswerId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/DeleteReportAnswer");
                return Ok(deleteReportAnswerResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/DeleteReportAnswer");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateReportAnswer")]
        public async Task<IActionResult> UpdateReportAnswer([FromBody] ReportAnswerUpdateDTO reportAnswerUpdateDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                ReportAnswer updatedReportAnswer = await _reportAnswerService.UpdateReportAnswerAsync(reportAnswerUpdateDTO);
                if (updatedReportAnswer == null) return BadRequest($"UpdateReportAnswer Error ReportAnswerId: {reportAnswerUpdateDTO.Id}");
                await _logService.CreateLogAsync($"UpdateReportAnswer Success ReportAnswerId: {reportAnswerUpdateDTO.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportAnswerController/UpdateReportAnswer");
                return Ok();
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportAnswerController/UpdateReportAnswer");
                return BadRequest(ex.Message);
            }
        } 
    }
}
