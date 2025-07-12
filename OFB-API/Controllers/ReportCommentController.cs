using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs.Reports;
using OFP_SERVICE.DTOs.UpdateDTOs.Reports;
using OFP_SERVICE.Services.Concrete.ReportsServices;
using OFP_SERVICE.Services.Interfaces.LogService;
using OFP_SERVICE.Services.Interfaces.ReportsServices;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportCommentController : ControllerBase
    {
        private readonly IReportCommentService _reportCommentService;
        private readonly ILogService _logService;

        public ReportCommentController(IReportCommentService reportCommentService, ILogService logService)
        {
            _reportCommentService = reportCommentService;
            _logService = logService;
        }

        [Authorize]
        [HttpPost("CreateReportComment")]
        public async Task<IActionResult> CreateReportComment([FromBody] ReportCommentCreateDTO reportCommentCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                ReportComment reportComment = await _reportCommentService.CreateReportCommentAsync(reportCommentCreateDTO);
                if (reportComment == null) return BadRequest("CreateReportComment Error");
                await _logService.CreateLogAsync($"CreateReportComment Success {reportComment.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/CreateReportComment");
                return Ok(true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/CreateReportComment");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetReportCommentById")]
        public async Task<IActionResult> GetReportCommentById(int reportCommentId)
        {
            if (reportCommentId == null) return BadRequest("ReportCommentId is null");
            try
            {
                ReportComment reportComment = await _reportCommentService.GetReportByIdAsync(reportCommentId);
                if (reportComment == null) return NotFound($"ReportComment not found ReportCommentId: {reportCommentId}");
                await _logService.CreateLogAsync($"GetReportCommentById Success {reportCommentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/GetReportCommentById");
                return Ok(reportComment);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/GetReportCommentById");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllReportComments")]
        public async Task<IActionResult> GetAllReportComments()
        {
            try
            {
                IEnumerable<ReportComment> reportComments = await _reportCommentService.GetAllReportCommentsAsync();
                if (reportComments == null) return NotFound("ReportComments not found");
                await _logService.CreateLogAsync($"GetAllReportComments Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/GetAllReportComments");
                return Ok(reportComments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/GetAllReportComments");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUnderReviewReportComments")]
        public async Task<IActionResult> GetAllUnderReviewReportComments()
        {
            try
            {
                IEnumerable<ReportComment> reportComments = await _reportCommentService.GetAllUnderReviewReportCommentsAsync();
                if (reportComments == null) return NotFound("UnderReviewReportComments not found");
                await _logService.CreateLogAsync($"GetAllUnderReviewReportComments Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/GetAllUnderReviewReportComments");
                return Ok(reportComments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/GetAllUnderReviewReportComments");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllResolvedReportComments")]
        public async Task<IActionResult> GetAllResolvedReportComments()
        {
            try
            {
                IEnumerable<ReportComment> reportComments = await _reportCommentService.GetAllResolvedReportCommentsAsync();
                if (reportComments == null) return NotFound("ResolvedReportComments not found");
                await _logService.CreateLogAsync($"GetAllResolvedReportComments Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/GetAllResolvedReportComments");
                return Ok(reportComments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/GetAllResolvedReportComments");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRejectedReportComments")]
        public async Task<IActionResult> GetAllRejectedReportComments()
        {
            try
            {
                IEnumerable<ReportComment> reportComments = await _reportCommentService.GetAllRejectedReportCommentsAsync();
                if (reportComments == null) return NotFound("RejectedReportComments not found");
                await _logService.CreateLogAsync($"GetAllRejectedReportComments Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/GetAllRejectedReportComments");
                return Ok(reportComments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/GetAllRejectedReportComments");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeleteReportComment")]
        public async Task<IActionResult> DeleteReportComment(int reportCommentId)
        {
            if (reportCommentId == null) return BadRequest("ReportCommentId is null");
            try
            {
                ReportComment deleteReportCommentResult = await _reportCommentService.DeleteReportCommentAsync(reportCommentId);
                if (deleteReportCommentResult == null) return BadRequest("DeleteReportComment Error");
                await _logService.CreateLogAsync($"DeleteReportComment Success ReportCommentId: {reportCommentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/DeleteReportComment");
                return Ok(deleteReportCommentResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/DeleteReportComment");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateReportComment")]
        public async Task<IActionResult> UpdateReportComment([FromBody] ReportCommentUpdateDTO reportCommentUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                ReportComment updatedReportComment = await _reportCommentService.UpdateReportCommentAsync(reportCommentUpdateDTO);
                if (updatedReportComment == null) return BadRequest($"UpdateReportComment Error ReportCommentId: {reportCommentUpdateDTO.Id}");
                await _logService.CreateLogAsync($"UpdateReportComment Success ReportCommentId: {reportCommentUpdateDTO.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ReportCommentController/UpdateReportComment");
                return Ok(updatedReportComment);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ReportCommentController/UpdateReportComment");
                return BadRequest(ex.Message);
            }
        }
    }
}
