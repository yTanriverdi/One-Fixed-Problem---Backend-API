using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Log;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllLogs")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                IEnumerable<Log> logs =  await _logService.GetAllLogsAsync();
                if (logs == null) return NotFound("Logs not found");
                await _logService.CreateLogAsync($"GetAllLogs Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "LogController/GetAllLogs");
                return Ok(logs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "LogController/GetAllLogs");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllInformationLogs")]
        public async Task<IActionResult> GetAllInformationLogs()
        {
            try
            {
                IEnumerable<Log> informationLogs = await _logService.GetAllInformationLogsAsync();
                if (informationLogs == null) return NotFound("GetAllInformationLogs not found");
                await _logService.CreateLogAsync($"GetAllInformationLogs Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "LogController/GetAllInformationLogs");
                return Ok(informationLogs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "LogController/GetAllInformationLogs");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllErrorLogs")]
        public async Task<IActionResult> GetAllErrorLogs()
        {
            try
            {
                IEnumerable<Log> errorLogs = await _logService.GetAllErrorLogsAsync();
                if (errorLogs == null) return NotFound("GetAllErrorLogs not found");
                await _logService.CreateLogAsync($"GetAllErrorLogs Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "LogController/GetAllErrorLogs");
                return Ok(errorLogs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "LogController/GetAllErrorLogs");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllLogsByControllerName")]
        public async Task<IActionResult> GetAllLogsByControllerName(string controllerName)
        {
            if (string.IsNullOrWhiteSpace(controllerName)) return BadRequest("ControllerName is null");
            try
            {
                IEnumerable<Log> controllerNameLogs = await _logService.GetAllLogsByControllerNameAsync(controllerName);
                if (controllerNameLogs == null) return NotFound("GetAllLogsByControllerName not found");
                await _logService.CreateLogAsync($"GetAllLogsByControllerName Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "LogController/GetAllLogsByControllerName");
                return Ok(controllerNameLogs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "LogController/GetAllLogsByControllerName");
                return BadRequest(ex.Message);
            }
        }
        
        
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllLogsByUserId")]
        public async Task<IActionResult> GetAllLogsByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                IEnumerable<Log> userLogs = await _logService.GetAllLogsByControllerNameAsync(userId);
                if (userLogs == null) return NotFound("GetAllLogsByUserId not found");
                await _logService.CreateLogAsync($"GetAllLogsByUserId Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "LogController/GetAllLogsByUserId");
                return Ok(userLogs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "LogController/GetAllLogsByUserId");
                return BadRequest(ex.Message);
            }
        }

    }
}
