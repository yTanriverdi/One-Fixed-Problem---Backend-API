using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OFB_API.VM_s.ProblemVMs;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Likes;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LikesServices;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly IProblemService _problemService;
        private readonly ILogService _logService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly IProblemLikeService _problemLikeService;
        public ProblemController(IProblemService problemService, ILogService logService, UserManager<BaseUser> userManager, IProblemLikeService problemLikeService)
        {
            _problemService = problemService;
            _logService = logService;
            _userManager = userManager;
            _problemLikeService = problemLikeService;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProblem")]
        public async Task<IActionResult> CreateProblem([FromBody] ProblemCreateDTO problemCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Problem createResult = await _problemService.CreateProblemAsync(problemCreateDTO);
                await _logService.CreateLogAsync($"Create Problem: {problemCreateDTO.Content}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/CreateProblem");
                return Ok(createResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/CreateProblem");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetProblemByDate")]
        public async Task<IActionResult> GetProblemByDate()
        {
            try
            {
                Problem problem = await _problemService.GetProblemByDateAsync(DateTime.UtcNow);
                await _logService.CreateLogAsync($"Problem By Date Success {problem.Content}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetProblemByDate");
                return Ok(problem);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetProblemByDate");
                return BadRequest(ex.Message);
            }
        }



        [Authorize]
        [HttpGet("GetProblemByQuestionDate")]
        public async Task<IActionResult> GetProblemByQuestionDate()
        {
            try
            {
                Problem problem = await _problemService.GetProblemQuestionDateAsync();
                ProblemVM problemVm = new ProblemVM()
                {
                    Id = problem.Id,
                    Content = problem.Content,
                    Likes = problem.Likes,
                    AnswerCount = problem.AnswerCount
                };
                await _logService.CreateLogAsync($"Problem By QuestionDate Success: {problemVm.Content}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetProblemByQuestionDate");
                return Ok(problemVm);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetProblemByQuestionDate");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("DeleteProblem")]
        public async Task<IActionResult> DeleteProblem(int problemId)
        {
            if (problemId == null) return BadRequest("ProblemId is null");
            try
            {
                bool result = await _problemService.DeleteProblemAsync(problemId);
                await _logService.CreateLogAsync($"Delete Problem Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/DeleteProblem");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/DeleteProblem");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetProblemById")]
        public async Task<IActionResult> GetProblemById(int problemId)
        {
            if (problemId <= 0) return BadRequest("ProblemId error");
            try
            {
                Problem problem = await _problemService.GetProblemAsync(problemId);
                if(problem == null) return NotFound("Problem Not Found");
                await _logService.CreateLogAsync($"GetProblemById Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetProblemById");
                return Ok(problem);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetProblemById");
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetProblemByIdDetail")]
        public async Task<IActionResult> GetProblemByIdDetail(int problemId)
        {
            if (problemId <= 0) return BadRequest("ProblemId error");
            try
            {
                Problem problem = await _problemService.GetProblemAsync(problemId);
                if (problem == null) return NotFound("Problem Not Found");
                ProblemVM problemVM = new ProblemVM()
                {
                    Id = problem.Id,
                    AnswerCount = problem.AnswerCount,
                    Content = problem.Content,
                    Likes = problem.Likes,
                    ProblemDate = problem.QuestionDate,
                    Answers = problem.Answers
                };
                await _logService.CreateLogAsync($"GetProblemByIdDetail Success ProblemId: {problemId}", "byProblemDetail", false, "ProblemController/GetProblemByIdDetail");
                return Ok(problemVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetProblemByIdDetail");
                return BadRequest(ex.Message);
            }
        }





        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllActiveProblems")]
        public async Task<IActionResult> GetAllActiveProblems()
        {
            try
            {
                var result = await _problemService.GetAllActiveProblemsEnumerableAsync();
                if (result == null) return NotFound("Not Found Active Problems");
                await _logService.CreateLogAsync($"GetAllActiveProblems Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetAllActiveProblems");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetAllActiveProblems");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllPassiveProblems")]
        public async Task<IActionResult> GetAllPassiveProblems()
        {
            try
            {
                var result = await _problemService.GetAllPassiveProblemsEnumerableAsync();
                if (result == null) return NotFound("Not Found Passive Problems");
                await _logService.CreateLogAsync($"GetAllPassiveProblems Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetAllPassiveProblems");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetAllPassiveProblems");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllByLikesDescProblems")]
        public async Task<IActionResult> GetAllByLikesDescProblems()
        {
            try
            {
                var result = await _problemService.GetProblemByLikesEnumerableAsync();
                if (result == null) return NotFound("Not Found Problems");
                await _logService.CreateLogAsync($"GetAllByLikesDescProblems Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetAllByLikesDescProblems");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetAllByLikesDescProblems");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("AnyProblemLike")]
        public async Task<IActionResult> AnyProblemLike(int problemId, string userId)
        {
            if (problemId == null || string.IsNullOrWhiteSpace(userId)) return BadRequest("ProblemId or UserId is null");
            try
            {
                bool result = await _problemLikeService.AnyProblemLikeAsync(problemId, userId);
                await _logService.CreateLogAsync($"AnyProblemLike Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/AnyProblemLike");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/AnyProblemLike");
                return BadRequest(ex.Message);
            }
        }



        [Authorize]
        [HttpPost("ProblemLikeChange")]
        public async Task<IActionResult> ProblemLikeChange(int problemId)
        {
            if (problemId <= 0) return BadRequest("ProblemId Error");
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bool result = await _problemLikeService.AnyProblemLikeAsync(problemId, userId);
                if (result == true)
                {
                    ProblemLike problemLikebyUser = await _problemLikeService.GetProblemLikeByUserAndProblemIdAsync(userId, problemId);
                    if(problemLikebyUser == null)
                    {
                        await _logService.CreateLogAsync("GetProblemLikeByUserAndProblemIdAsync Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("ProblemLikeByUser Error - Null");
                    }
                    bool deleteProblemLike = await _problemLikeService.DeleteProblemLikeAsync(problemLikebyUser.Id);
                    if (deleteProblemLike == false)
                    {
                        await _logService.CreateLogAsync("DeleteProblemLike Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("ProblemLikeChange Error");
                    }
                    int problemLikeCount = await _problemService.ProblemLikeChangeAsync(problemId, false);
                    if (problemLikeCount == null)
                    {
                        await _logService.CreateLogAsync("ProblemLikesDown Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("ProblemLikeChange Error");
                    }
                    return Ok(problemLikeCount);
                }
                else
                {
                    ProblemLikeCreateDTO problemLikeCreateDTO = new ProblemLikeCreateDTO()
                    {
                        UserId = userId,
                        ProblemId = problemId
                    };
                    if(problemLikeCreateDTO.UserId == null || problemId <= 0)
                    {
                        await _logService.CreateLogAsync("ProblemLikeCreateDTO Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("ProblemLikeCreateDTO error");
                    }
                    ProblemLike problemLike = await _problemLikeService.CreateProblemLikeAsync(problemLikeCreateDTO);
                    if (problemLike == null) 
                    {
                        await _logService.CreateLogAsync("CreateProblemLike Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("CreateProblemLike Error");
                    }
                    
                    int problemLikeCount = await _problemService.ProblemLikeChangeAsync(problemId, true);
                    if (problemLikeCount == null)
                    {
                        await _logService.CreateLogAsync("ProblemLikeChangeAsync Error", userId, true, "ProblemController/ProblemLikeChange");
                        return BadRequest("ProblemLikeChangeAsync Error"); 
                    }
                    await _logService.CreateLogAsync($"ProblemLikesUp Success ProblemId: {problemId}", userId, false, "ProblemController/ProblemLikeChange");
                    return Ok(problemLikeCount);
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/ProblemLikeChange");
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize]
        [HttpPost("ProblemLikesDown")]
        public async Task<IActionResult> ProblemLikesDown(int problemId)
        {
            if (problemId == null) return BadRequest("ProblemId is null");
            try
            {
                ProblemLike problemLike = await _problemLikeService.GetProblemLikeByUserAndProblemIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), problemId);
                if (problemLike == null) return BadRequest("ProblemLikesDown Error");
                bool problemLikeDeleteResult = await _problemLikeService.DeleteProblemLikeAsync(problemLike.Id);
                if (!problemLikeDeleteResult) return BadRequest("ProblemLikesDown Error");
                int problemLikeCount = await _problemService.ProblemLikesDownAsync(problemId);
                if (problemLikeCount == null) return BadRequest("ProblemLikesDown Error");
                await _logService.CreateLogAsync($"ProblemLikesDown Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/ProblemLikesDown");
                return Ok(problemLikeCount);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/ProblemLikesDown");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("ProblemAnswerUp")]
        public async Task<IActionResult> ProblemAnswerUp(int problemId)
        {
            if (problemId == null) return BadRequest("ProblemId is null");
            try
            {
                int problemAnswerCount = await _problemService.ProblemAnswerUpAsync(problemId);
                await _logService.CreateLogAsync($"ProblemAnswerUp Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/ProblemAnswerUp");
                return Ok(problemAnswerCount);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/ProblemAnswerUp");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize]
        [HttpPut("ProblemAnswerDown")]
        public async Task<IActionResult> ProblemAnswerDown(int problemId)
        {
            if (problemId == null) return BadRequest("ProblemId is null");
            try
            {
                int problemAnswerCount = await _problemService.ProblemAnswerDownAsync(problemId);
                await _logService.CreateLogAsync($"ProblemAnswerDown Success ProblemId: {problemId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/ProblemAnswerDown");
                return Ok(problemAnswerCount);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/ProblemAnswerDown");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetProblemsByFiltered")]
        public async Task<IActionResult> GetProblemsByFiltered(string filteredKey)
        {
            if (string.IsNullOrEmpty(filteredKey)) return BadRequest("FilteredKey is null");
            try
            {
                var filterResult = await _problemService.GetProblemsByFilteredAsync(filteredKey);
                await _logService.CreateLogAsync($"GetProblemsByFiltered Success FilterKey: {filteredKey}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetProblemsByFiltered");
                return Ok(filterResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetProblemsByFiltered");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAllPastProblems")]
        public async Task<IActionResult> GetAllPastProblems()
        {
            try
            {
                IEnumerable<Problem> pastProblems = await _problemService.GetAllPastProblemsAsync();
                if (pastProblems == null) return NotFound("PastProblems is Not Found");
                List<ProblemVM> problemsVM = new List<ProblemVM>();
                foreach (var problem in pastProblems)
                {
                    ProblemVM problemVM = new ProblemVM()
                    {
                        Id = problem.Id,
                        AnswerCount = problem.AnswerCount,
                        Content = problem.Content,
                        Likes = problem.Likes,
                        ProblemDate = problem.QuestionDate
                    };
                    problemsVM.Add(problemVM);
                }
                if (problemsVM.Count <= 0) return NotFound("PastProblems is Not Found");
                problemsVM.Reverse();
                await _logService.CreateLogAsync($"GetAllPastProblems Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/GetAllPastProblems");
                return Ok(problemsVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/GetAllPastProblems");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProblem")]
        public async Task<IActionResult> UpdateProblem([FromBody] ProblemUpdateDTO problemUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Problem updatedProblem = await _problemService.UpdateProblemAsync(problemUpdateDTO);
                if (updatedProblem == null) return BadRequest("Update Problem error");
                await _logService.CreateLogAsync($"UpdateProblem Success ProblemId {updatedProblem.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "ProblemController/UpdateProblem");
                return Ok(updatedProblem);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "ProblemController/UpdateProblem");
                return BadRequest(ex.Message);
            }
        }
    }
}
