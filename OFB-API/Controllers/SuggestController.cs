using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestController : ControllerBase
    {
        private readonly ISuggestService _suggestService;
        private readonly ILogService _logService;

        public SuggestController(ISuggestService suggestService, ILogService logService)
        {
            _suggestService = suggestService;
            _logService = logService;
        }

        [Authorize]
        [HttpPost("CreateSuggest")]
        public async Task<IActionResult> CreateSuggest([FromBody] SuggestCreateDTO suggestCreateDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Suggest suggest = await _suggestService.CreateSuggestAsync(suggestCreateDTO);
                if( suggest == null ) return BadRequest("Suggest Create Error");
                await _logService.CreateLogAsync($"CreateSuggest Success SuggestId: {suggest.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "SuggestController/CreateSuggest");
                return Ok("Thank you for your suggestion");
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "SuggestController/CreateSuggest");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllSuggests")]
        public async Task<IActionResult> GetAllSuggests()
        {
            try
            {
                IEnumerable<Suggest> suggests = await _suggestService.GetAllSuggestsAsync();
                if (suggests == null) return NotFound("Suggest Not Found");
                await _logService.CreateLogAsync($"GetAllSuggests Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "SuggestController/GetAllSuggests");
                return Ok(suggests);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "SuggestController/GetAllSuggests");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllIsSuggests")]
        public async Task<IActionResult> GetAllIsSuggests()
        {
            try
            {
                IEnumerable<Suggest> isSuggests = await _suggestService.GetAllSuggestsIsSuggestAsync();
                if (isSuggests == null) return NotFound("IsSuggest not found");
                await _logService.CreateLogAsync($"GetAllIsSuggests Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "SuggestController/GetAllIsSuggests");
                return Ok(isSuggests);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "SuggestController/GetAllIsSuggests");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllNoSuggests")]
        public async Task<IActionResult> GetAllNoSuggests()
        {
            try
            {
                IEnumerable<Suggest> noSuggests = await _suggestService.GetAllSuggestsNoSuggestAsync();
                if (noSuggests == null) return NotFound("NoSuggests not found");
                await _logService.CreateLogAsync($"GetAllNoSuggests Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "SuggestController/GetAllNoSuggests");
                return Ok(noSuggests);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "SuggestController/GetAllNoSuggests");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetSuggestById")]
        public async Task<IActionResult> GetSuggestById(int suggestId)
        {
            if (suggestId == null) return BadRequest("SuggestId is null");
            try
            {
                Suggest suggest = await _suggestService.GetSuggestAsync(suggestId);
                if (suggest == null) return NotFound($"Suggest not found SuggestId: {suggestId}");
                await _logService.CreateLogAsync($"GetSuggestById success SuggestId: {suggestId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "SuggestController/GetSuggestById");
                return Ok(suggest);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "SuggestController/GetSuggestById");
                return BadRequest(ex.Message);
            }
        }
    }
}
