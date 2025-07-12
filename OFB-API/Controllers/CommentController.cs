using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OFB_API.VM_s.CommentLikeVMs;
using OFB_API.VM_s.CommentVMs;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Likes;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LikesServices;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Xml.Linq;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly ILogService _logService;
        private readonly ICommentLikeService _commentLikeService;

        public CommentController(ICommentService commentService, UserManager<BaseUser> userManager, ILogService logService, ICommentLikeService commentLikeService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _logService = logService;
            _commentLikeService = commentLikeService;
        }

        [Authorize]
        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDTO commentCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var commentResult = await _commentService.CreateCommentAsync(commentCreateDTO);
                if (commentResult == null) return BadRequest("Comment create error");
                CommentCreateAfterVM commentCreateAfterVM = new CommentCreateAfterVM() 
                { 
                    Id = commentResult.Id,
                    CommentContent = commentResult.CommentContent,
                    UserId = commentResult.UserId,
                    AnswerId = commentResult.AnswerId,
                }; 
                await _logService.CreateLogAsync($"Create Comment Success {commentResult.CommentContent}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/CreateComment");
                return Ok(commentCreateAfterVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/CreateComment");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDTO commentUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Comment updateResult = await _commentService.UpdateCommentAsync(commentUpdateDTO);
                if (updateResult == null) return BadRequest("Update comment error");
                CommentUpdateAfterVM commentUpdateAfterVM = new CommentUpdateAfterVM()
                {
                    Id = updateResult.Id,
                    CommentContent = updateResult.CommentContent,
                    UserId = updateResult.UserId,
                    AnswerId = updateResult.AnswerId,
                };
                await _logService.CreateLogAsync($"Update Comment Success CommentId: {commentUpdateAfterVM.Id}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/UpdateComment");
                return Ok(commentUpdateAfterVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/UpdateComment");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllActiveComments")]
        public async Task<IActionResult> GetAllActiveComments()
        {
            try
            {
                IEnumerable<Comment> activeComments = await _commentService.GetAllActiveCommentsAsync();
                if (activeComments == null) return NotFound("Not found Active Comments ");
                await _logService.CreateLogAsync($"GetAllActiveComments Success {activeComments.ToList().Count}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetAllActiveComments");
                return Ok(activeComments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetAllActiveComments");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if(commentId <= 0) return BadRequest("Invalid CommentId");
            try
            {
                bool deleteResult = await _commentService.DeleteCommentAsync(commentId);
                if (!deleteResult) return BadRequest($"DeleteComment Error {commentId}");
                await _logService.CreateLogAsync($"DeleteComment Success CommentId: {commentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/DeleteComment");
                return Ok(deleteResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/DeleteComment");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("CommentLikeUp")]
        public async Task<IActionResult> CommentLikeUp(int commentId)
        {
            if (commentId <= 0) return BadRequest("Invalid CommentId");
            try
            {
                int commentLikeUpResult = await _commentService.CommentLikesUpAsync(commentId);
                if (commentLikeUpResult <= 0) return BadRequest($"CommentLikeUp Error CommentId: {commentId}");
                CommentLikeCreateDTO commentLikeCreateDTO = new CommentLikeCreateDTO()
                {
                    CommentId = commentId,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                var result = await _commentLikeService.CreateCommentLikeAsync(commentLikeCreateDTO);
                if (result == null) return BadRequest($"CommentLikeUp Error CommentId: {commentId}");
                await _logService.CreateLogAsync($"CommentLikeUp Success CommentId: {commentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/CommentLikeUp");
                return Ok(commentLikeUpResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/CommentLikeUp");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize]
        [HttpPut("CommentLikeDown")]
        public async Task<IActionResult> CommentLikeDown(int commentId)
        {
            if (commentId <= 0) return BadRequest("Invalid CommentId");
            try
            {
                int commentLikeDownResult = await _commentService.CommentLikesDownAsync(commentId);
                if (commentLikeDownResult == null) return BadRequest($"CommentLikeDown Error CommentId: {commentId}");
                CommentLike commentLike = await _commentLikeService.GetCommentLikeByUserAndCommentIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), commentId);
                if (commentLike == null) return NotFound("CommentLike not found");
                bool commentLikeDeleteResult = await _commentLikeService.DeleteCommentLikeAsync(commentLike.Id);
                if (!commentLikeDeleteResult) return BadRequest($"CommentLikeDown Error CommentId: {commentId}");
                await _logService.CreateLogAsync($"CommentLikeDown Success CommentId: {commentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/CommentLikeDown");
                return Ok(commentLikeDownResult);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/CommentLikeDown");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetCommentsByAnswerId")]
        public async Task<IActionResult> GetCommentsByAnswerId(int answerId)
        {
            if (answerId <= 0) return BadRequest("Invalid AnswerId");
            try
            {
                IEnumerable<Comment> result = await _commentService.GetAllCommentsByAnswerIdAsync(answerId);
                if (result.Count() == 0) return Ok(0);
                IEnumerable<CommentVM> comments = result.Select(result => new CommentVM()
                {
                    Id = result.Id,
                    UserId = result.User.Id,
                    UserName = result.User.UserName,
                    CommentContent = result.CommentContent,
                    Likes = result.Likes,
                    AnswerId = result.AnswerId,
                    UpdatedDate = result.UpdatedDate,
                    CreatedDate = result.CreatedDate
                }).ToList();
                await _logService.CreateLogAsync($"GetCommentsByAnswerId Success AnswerId: {answerId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetCommentsByAnswerId");
                return Ok(comments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetCommentsByAnswerId");
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize]
        [HttpGet("GetCommentsByUserId")]
        public async Task<IActionResult> GetCommentsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is null");
            try
            {
                IEnumerable<Comment> result = await _commentService.GetAllCommentsByUserIdAsync(userId);
                if (result == null) return NotFound("Not found Comments");
                IEnumerable<CommentVM> comments = result.Select(result => new CommentVM()
                {
                    Id = result.Id,
                    UserId = result.User.Id,
                    CommentContent = result.CommentContent,
                    Likes = result.Likes,
                    AnswerId = result.AnswerId,
                    UpdatedDate = result.UpdatedDate,
                    CreatedDate = result.CreatedDate
                }).ToList();
                await _logService.CreateLogAsync($"GetCommentsByUserId Success UserId: {userId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetCommentsByUserId");
                return Ok(comments);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetCommentsByUserId");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetCommentById")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            if (commentId == null) return BadRequest("CommentId is null");
            try
            {
                Comment comment = await _commentService.GetByIdAsync(commentId);
                if (comment == null) return NotFound($"Comment not found CommentId: {commentId}");
                await _logService.CreateLogAsync($"GetCommentById Success CommentId: {commentId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetCommentById");
                return Ok(comment);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetCommentById");
                return BadRequest(ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllCommentsBySearchKey")]
        public async Task<IActionResult> GetAllCommentsBySearchKey(string searchKey)
        {
            if (string.IsNullOrWhiteSpace(searchKey)) return BadRequest("SearchKey format error. No white spaces");
            try
            {
                var result = await _commentService.GetAllCommentsBySearchKeyAsync(searchKey.Trim());
                if (result == null) return NotFound("Comments not found");
                IEnumerable<CommentVM> commentVMs = result.Select(result => new CommentVM()
                {
                    Id = result.Id,
                    UserId = result.UserId,
                    CommentContent = result.CommentContent,
                    AnswerId = result.AnswerId,
                    Likes = result.Likes,
                    CreatedDate = result.CreatedDate,
                    UpdatedDate = result.UpdatedDate,
                });
                await _logService.CreateLogAsync($"GetAllCommentsBySearchKey Success SearchKey: {searchKey}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetAllCommentsBySearchKey");
                return Ok(commentVMs);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetAllCommentsBySearchKey");
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpGet("GetAllCommentLikeByUserId")]
        public async Task<IActionResult> GetAllCommentLikeByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId Null");
            try
            {
                var commentLikes = await _commentLikeService.GetAllCommentLikeByUserIdAsync(userId);
                if (commentLikes == null || !commentLikes.Any()) return Ok(new List<CommentLike>());

                List<CommentLikeVM> commentLikesResponseData = commentLikes.Select(x => new CommentLikeVM
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    CommentId = x.CommentId,
                }).ToList();
                await _logService.CreateLogAsync($"GetAllCommentLikeByUserId Success UserId: {userId}", User.FindFirstValue(ClaimTypes.NameIdentifier), false, "CommentController/GetAllCommentLikeByUserId");
                return Ok(commentLikesResponseData);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "CommentController/GetAllCommentLikeByUserId");
                return BadRequest(ex.Message);
            }
        }
    }
}
