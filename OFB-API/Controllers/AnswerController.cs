using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OFB_API.VM_s.AnswerVMs;
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
using System.Text.Json.Serialization;
using System.Text.Json;
using OFB_API.VM_s;
using OFP_CORE.Enums;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly ILogService _logService;
        private readonly IProblemService _problemService;
        private readonly IAnswerLikeService _answerLikeService;
        private readonly IUserService _userService;

        public AnswerController(IAnswerService answerService, UserManager<BaseUser> userManager, ILogService logService, IProblemService problemService, IAnswerLikeService answerLikeService, IUserService userService)
        {
            _answerService = answerService;
            _userManager = userManager;
            _logService = logService;
            _problemService = problemService;
            _answerLikeService = answerLikeService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("CreateAnswer")]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerCreateDTO answerCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                              .SelectMany(v => v.Errors)
                              .Select(e => e.ErrorMessage)
                              .ToList();
                return BadRequest(errors);
            }
            try
            {
                bool anyAnswer = await _answerService.AnyAnswerByProblemAndUserIdAsync(answerCreateDTO.UserId, answerCreateDTO.ProblemId);
                if(anyAnswer) return BadRequest("User has already answered");
                Answer answer = await _answerService.CreateAnswerAsync(answerCreateDTO);
                if (answer == null) return BadRequest("Answer Create Error");
                int result = await _problemService.ProblemAnswerUpAsync(answerCreateDTO.ProblemId);
                if (result == null) return BadRequest("Answer Create Error / Problem Answer UP");
                AnswerCreateAfterVM answerCreateAfterVM = new AnswerCreateAfterVM()
                {
                    Id = answer.Id,
                    AnswerContent = answer.AnswerContent,
                    AnswerDate = answer.AnswerDate,
                    EndAnswerDate = Convert.ToDateTime(answer.EndAnswerDate),
                    EndAnswer = answer.EndAnswer,
                    UserId = answer.UserId,
                    ProblemId = answer.ProblemId,
                };
                await _logService.CreateLogAsync($"Create Answer Success Content: {answerCreateDTO.AnswerContent}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/CreateAnswer");
                return Ok(answerCreateAfterVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/CreateAnswer");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("UpdateAnswer")]
        public async Task<IActionResult> UpdateAnswer([FromBody] AnswerUpdateDTO answerUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Answer answer = await _answerService.GetAnswerByIdAsync(answerUpdateDTO.Id);
                if (answer.EndAnswerDate <= DateTime.UtcNow) return BadRequest("Your reply update period has expired.");
                var updatedAnswer = await _answerService.UpdateAnswerAsync(answerUpdateDTO);
                if (updatedAnswer == null) return BadRequest("Update Answer Error");
                await _logService.CreateLogAsync($"Update Answer Success AnswerId: {answerUpdateDTO.Id} ", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/UpdateAnswer");
                AnswerUpdateAfterVM answerUpdateAfterVM = new AnswerUpdateAfterVM()
                {
                    Id = answerUpdateDTO.Id,
                    AnswerContent = updatedAnswer.AnswerContent,
                    BaseUser = answer.User
                };
                return Ok(answerUpdateAfterVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/UpdateAnswer");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("DeleteAnswer")]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            if (answerId == null) return BadRequest("AnswerId is null");
            try
            {
                Answer answer = await _answerService.GetAnswerByIdAsync(answerId);
                if (answer == null) return BadRequest("Answer not found");
                bool deleteAnswerResult = await _answerService.DeleteAnswerAsync(answerId);
                if (!deleteAnswerResult) return BadRequest("Answer Delete Error"); 
                int resultProblemDown = await _problemService.ProblemAnswerDownAsync(answer.ProblemId);
                if (resultProblemDown == null) return BadRequest("Answer Delete Error / Problem Answer DOWN");
                await _logService.CreateLogAsync($"Delete Answer Success AnswerId: {answerId} ", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/UpdateAnswer");
                return Ok(deleteAnswerResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/DeleteAnswer");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAllAnswersByProblemId")]
        public async Task<IActionResult> GetAllAnswersByProblemId(int problemId)
        {
            if (problemId <= 0) return BadRequest("ProblemId is null");
            try
            {
                var answers = (await _answerService.GetAnswerByProblemIdEnumerableAsync(problemId)).ToList();
                if (!answers.Any()) return NotFound($"Not Found Answer for ProblemId {problemId}");
                List<AnswerVM> answerVMs = new List<AnswerVM>();

                int commentCount = 0;
                foreach (Answer answer in answers)
                {
                    AnswerVM vm = new AnswerVM()
                    {
                        Id = answer.Id,
                        //AnswerComments = answer.Comments,
                        AnswerContent = answer.AnswerContent,
                        AnswerDate = answer.AnswerDate,
                        User = answer.User,
                        Likes = answer.Likes,
                    };
                    foreach(Comment comment in answer.Comments)
                    {
                        if(comment.Status == Status.Active || comment.Status == Status.Updated)
                        {
                            commentCount++;
                        }
                    }
                    vm.CommentCount = commentCount;
                    commentCount = 0;
                    answerVMs.Add(vm);
                }
                if (answerVMs.Count <= 0) return Ok(false);

                await _logService.CreateLogAsync($"GetAllAnswersByProblemId Success ProblemId: {problemId} ", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetAllAnswersByProblemId");
                return Ok(answerVMs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAllAnswersByProblemId");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAnswerById")]
        public async Task<IActionResult> GetAnswerById(int answerId)
        {
            if (answerId == null) return BadRequest("AnswerId is null");
            try
            {
                Answer answer = await _answerService.GetAnswerByIdAsync(answerId);
                if (answer == null) return NotFound($"Answer not found AnswerId {answerId}");
                await _logService.CreateLogAsync($"GetAnswerById Success AnswerId: {answerId} ", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetAnswerById");
                return Ok(answer);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAnswerById");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllAnswersByUserId")]
        public async Task<IActionResult> GetAllAnswersByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                var answers = await _answerService.GetAnswersByUserIdEnumerableAsync(userId);
                if (answers == null) return NotFound($"Answers not found UserId: {userId}");
                await _logService.CreateLogAsync($"GetAllAnswersByUserId Success UserId: {userId} ", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetAllAnswersByUserId");
                return Ok(answers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAllAnswersByUserId");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAllActiveAnswers")]
        public async Task<IActionResult> GetAllActiveAnswers()
        {
            try
            {
                IEnumerable<Answer> answers = await _answerService.GetAllActiveAnswersEnumerableAsync();
                if (answers == null) return NotFound("Not found Active Answers");
                await _logService.CreateLogAsync($"GetAllActiveAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetAllActiveAnswers");
                return Ok(answers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAllActiveAnswers");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllPassiveAnswers")]
        public async Task<IActionResult> GetAllPassiveAnswers()
        {
            try
            {
                IEnumerable<Answer> answers = await _answerService.GetAllPassiveAnswersEnumerableAsync();
                if (answers == null) return NotFound("Not found Passive Answers");
                await _logService.CreateLogAsync($"GetAllPassiveAnswers Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetAllPassiveAnswers");
                return Ok(answers);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAllPassiveAnswers");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPut("AnswerLikeUp")]
        public async Task<IActionResult> AnswerLikeUp(int answerId)
        {
            if (answerId == null) return BadRequest("AnswerId is null");
            try
            {
                Answer answer = await _answerService.GetAnswerByIdAsync(answerId);
                if (answer == null) return NotFound("Answer Not Found");
                int answerLikes = await _answerService.AnswerLikesUpAsync(answerId);
                if (answerLikes == null) return BadRequest("AnswerLikeUp Error");
                AnswerLikeCreateDTO answerLikeCreateDTO = new AnswerLikeCreateDTO()
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    AnswerId = answer.Id,
                };
                var result = await _answerLikeService.CreateAnswerLikeAsync(answerLikeCreateDTO);
                if (result == null) return BadRequest("AnswerLike Error");
                var userLikes = await _userService.UserLikesUpAsync(answer.UserId);
                await _logService.CreateLogAsync($"AnswerLikeUp Success AnswerId {answerId} Likes {answerLikes}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/AnswerLikeUp");
                return Ok(answerLikes);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/AnswerLikeUp");
                return BadRequest(ex.Message);
            }
        }
        
        
        [Authorize]
        [HttpPut("AnswerLikeDown")]
        public async Task<IActionResult> AnswerLikeDown(int answerId)
        {
            if (answerId == null) return BadRequest("AnswerId is null");
            try
            {
                Answer answer = await _answerService.GetAnswerByIdAsync(answerId);
                if (answer == null) return NotFound("Answer Not Found");
                int answerLikes = await _answerService.AnswerLikesDownAsync(answerId);
                if (answerLikes == null) return BadRequest("AnswerLikeDown Error");
                var result = await _answerLikeService.GetAnswerLikeByUserAndAnswerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), answerId);
                if (result == null) return BadRequest("AnswerLike Error");
                bool deleteAnswerLike = await _answerLikeService.DeleteAnswerLikeAsync(result.Id);
                if (!deleteAnswerLike) return BadRequest("AnswerLike Error");
                var userLikes = await _userService.UserLikesDownAsync(answer.UserId);
                await _logService.CreateLogAsync($"AnswerLikeDown Success AnswerId {answerId} Likes {answerLikes}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/AnswerLikeDown");
                return Ok(answerLikes);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/AnswerLikeDown");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("AnswerEnd")]
        public async Task<IActionResult> AnswerEnd(int answerId)
        {
            if (answerId == null) return BadRequest("AnswerId is null");
            try
            {
                bool result = await _answerService.AnswerEndAsync(answerId);
                await _logService.CreateLogAsync($"AnswerEnd Success AnswerId {answerId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/AnswerEnd");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/AnswerEnd");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("AnyAnswerByUser")]
        public async Task<IActionResult> AnyAnswerByUser(int problemId, string userId)
        {
            if (problemId <= 0 || string.IsNullOrWhiteSpace(userId)) return BadRequest("Invalid ProblemId or UserId");
            try
            {
                bool anyAnswer = await _answerService.AnyAnswerByProblemAndUserIdAsync(userId, problemId);
                await _logService.CreateLogAsync($"AnyAnswerByUser Success ProblemId={problemId} UserId={userId}", userId, false, "AnswerController/AnyAnswerByUser");
                return Ok(anyAnswer);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/AnyAnswerByUser");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAnswerDailyUser")]
        public async Task<IActionResult> GetAnswerDailyUser(string userId, int problemId)
        {
            if (string.IsNullOrEmpty(userId) || problemId <= 0) return BadRequest("UserId or ProblemId Error");
            try
            {
                Answer dailyAnswer = await _answerService.GetAnswerByUserIdAndProblemIdAsync(userId, problemId);
                if (dailyAnswer == null) return Ok(false);
                else
                {
                    int commentCount = 0;
                    DailyAnswerVM dailyAnswerVM = new DailyAnswerVM()
                    {
                        Id = dailyAnswer.Id,
                        AnswerContent = dailyAnswer.AnswerContent,
                        AnswerDate = dailyAnswer.AnswerDate,
                        CreatedDate = dailyAnswer.CreatedDate,
                        EndAnswer = dailyAnswer.EndAnswer,
                        EndAnswerDate = dailyAnswer.CreatedDate.AddMinutes(30),
                        Likes = dailyAnswer.Likes,
                        ProblemId = problemId,
                        Report = dailyAnswer.Report,
                        UserId = userId,
                    };
                    if(dailyAnswer.UpdatedDate != null)
                    {
                        dailyAnswerVM.UpdatedDate = dailyAnswer.UpdatedDate;
                    }
                    foreach(Comment comment in dailyAnswer.Comments)
                    {
                        if(comment.Status == Status.Active || comment.Status == Status.Updated)
                        {
                            commentCount++;
                        }
                    }
                    dailyAnswerVM.Comments = commentCount;
                    await _logService.CreateLogAsync($"GetAnswerDailyUser Success UserId= {userId} ProblemId= {problemId}", userId, false, "AnswerController/GetAnswerDailyUser");
                    return Ok(dailyAnswerVM);
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "AnswerController/GetAnswerDailyUser");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetUserAnswerLikes")]
        public async Task<IActionResult> GetUserAnswerLikes (string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId error");
            try
            {
                IEnumerable<AnswerLike> answerLikes = await _answerLikeService.GetAllAnswerLikeByUserIdAsync(userId);
                if (!answerLikes.Any()) return NotFound("Not found AnswerLikes");

                List<AnswerLikeVM> userAnswerLikes = answerLikes.Select(x => new AnswerLikeVM
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    AnswerId = x.AnswerId
                }).ToList();

                await _logService.CreateLogAsync("GetUserAnswerLikes Success", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetUserAnswerLikes");
                return Ok(userAnswerLikes);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), false, "AnswerController/GetUserAnswerLikes");
                return BadRequest(ex.Message);
            }
        }
    }
}
