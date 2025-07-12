using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OFB_API.CODEHELPER;
using OFB_API.DTO_s.User;
using OFB_API.JWT;
using OFB_API.VM_s.AnswerUsers;
using OFB_API.VM_s.User;
using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.Services.Concrete;
using OFP_SERVICE.Services.Interfaces;
using OFP_SERVICE.Services.Interfaces.LogService;
using System.Security.Claims;

namespace OFB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly UserManager<BaseUser> _userManager;
        private readonly SignInManager<BaseUser> _signInManager;
        private readonly IAnswerService _answerService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public UserController(ILogService logService, IUserService userService, SignInManager<BaseUser> signInManager, UserManager<BaseUser> userManager, IAnswerService answerService, JwtTokenService jwtTokenService, IConfiguration configuration, IMailService mailService)
        {
            _logService = logService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _answerService = answerService;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _mailService = mailService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            { // KULLANICI ADI VEYA MAIL ÖNCEDEN ALINMIŞ MI KONTROLLERI REGISTERUSERASYNC İÇERİSİNDE KONTROL EDİLİYOR
                BaseUser newUser = await _userService.RegisterUserAsync(userCreateDTO);
                if (newUser == null) return BadRequest("The email address or username is already taken.");
                var userRole = await _userManager.GetRolesAsync(newUser);
                RegisterAfterVM afterRegister = new RegisterAfterVM()
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    FullName = newUser.FullName,
                    UserName = newUser.UserName,
                    Role = userRole.FirstOrDefault(),
                };
                string code = new Random().Next(100000, 999999).ToString();
                afterRegister.Code = code;
                var subject = "OneFixedProblem registration";
                var body = $@"<html>
                  <body style=""font-family: Arial, sans-serif; background-color: white; margin: 0; padding: 0;border: 1px solid orangered; border-radius: 20px;"">
                    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" width: 98%; margin:0 auto;"">
                      <tr>
                        <td style=""padding: 20px; text-align: center;"">
                          <p style=""font-size: 18px; color: black;"">Hi <strong style=""font-size: 18px; color: orangered;"">{userCreateDTO.UserName}</strong> :)</p>
                          <p style=""font-size: 18px; color: black;"">Welcome to OneFixedProblem, your registration code: <strong style=""font-size: 18px; color:       orangered;"">{code}</strong></p>
                        </td>
                      </tr>
                    </table>
                  </body>
                </html>";
                MailCreateDTO mailCreateDTO = new MailCreateDTO()
                {
                    Email = userCreateDTO.Email,
                    Content = body,
                    Code = code
                };
                var mailResult = await _mailService.CreateMailAsync(mailCreateDTO);
                await _mailService.SendEmailAsync(newUser.Email, subject, body);
                await _logService.CreateLogAsync($"Create User {userCreateDTO.FirstName + userCreateDTO.LastName}", newUser.Id, false, "UserController/CreateUser");
                return Ok(afterRegister);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/CreateUser");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailandCode confirmEmailandCode)
        {
            if (string.IsNullOrEmpty(confirmEmailandCode.Email) || string.IsNullOrEmpty(confirmEmailandCode.Code)) return BadRequest("Invalid email or code.");
            try
            {
                var user = await _userManager.FindByEmailAsync(confirmEmailandCode.Email);
                if (user == null) return NotFound("There are no registered users for this email address");
                Mail mail = await _mailService.GetMailByEmailAsync(user.Email);
                if (mail == null) return BadRequest("Email Confirm Error");
                if (mail.Code != confirmEmailandCode.Code) return BadRequest("Incorrect Code");
                bool emailConfirmed = await _userService.UserEmailConfirmAsync(user.Id);
                if (!emailConfirmed) return BadRequest("Email Confirm Error");
                await _logService.CreateLogAsync($"Email Confirmed Success UserId:{user.Id}", user.Id, false, "UserController/ConfirmEmail");
                return Ok("Email confirmed successfully. Welcome to OneFixedProblem !!");
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, "", true, "UserController/ConfirmEmail");
                return BadRequest("Email confirmation failed.");
            }
        }

        [HttpPost("ResendEmail")]
        public async Task<IActionResult> ResendEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Email is null");
            try
            {
                BaseUser baseUser = await _userManager.FindByEmailAsync(email);
                if (baseUser == null) return NotFound("There are no registered users for this email address");
                Mail mail = await _mailService.GetMailByEmailAsync(baseUser.Email);
                bool mailDelete = await _mailService.DeleteByMailIdAsync(mail.Id);
                if (!mailDelete) return BadRequest("Resend Mail Error");
                string code = new Random().Next(100000, 999999).ToString();
                var subject = "OneFixedProblem resend code";
                var body = $@"<html>
                  <body style=""font-family: Arial, sans-serif; background-color: white; margin: 0; padding: 0;border: 1px solid orangered; border-radius: 20px;"">
                    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" width: 98%; margin:0 auto;"">
                      <tr>
                        <td style=""padding: 20px; text-align: center;"">
                          <p style=""font-size: 18px; color: black;"">Hi <strong style=""font-size: 18px; color: orangered;"">{baseUser.UserName}</strong> :)</p>
                          <p style=""font-size: 18px; color: black;"">Welcome to OneFixedProblem, your registration code: <strong style=""font-size: 18px; color:orangered;"">{code}</strong></p>
                        </td>
                      </tr>
                    </table>
                  </body>
                </html>";
                MailCreateDTO mailCreateDTO = new MailCreateDTO()
                {
                    Email = baseUser.Email,
                    Content = body,
                    Code = code
                };
                var mailResult = await _mailService.CreateMailAsync(mailCreateDTO);
                await _mailService.SendEmailAsync(baseUser.Email, subject, body);
                await _logService.CreateLogAsync($"Email Resend Success Email:{mail}", baseUser.Id, false, "UserController/ResendEmail");
                return Ok("Email resent");
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, "Resend NO ID", true, "UserController/ResendEmail");
                return BadRequest("Email confirmation failed.");
            }
        }

        [Authorize]
        [HttpGet("UserDetail")]
        public async Task<IActionResult> UserDetail(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                BaseUser user = await _userManager.FindByIdAsync(userId);
                var userAnswers = await _answerService.GetAnswersByUserIdEnumerableAsync(userId);
                UserDetailsVM userDetail = new UserDetailsVM()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Badge = user.Badge,
                    CreatedDate = user.CreatedDate,
                    Likes = user.Likes,
                    Gender = user.Gender,
                    Region = user.Region,
                    Email = user.Email,
                    UserName = user.UserName
                };
                if (user.UpdatedDate != null) userDetail.UpdatedDate = user.UpdatedDate.Value;
                await _logService.CreateLogAsync($"User detail User: {user.FullName}", user.Id, false, "UserController/UserDetail");
                return Ok(userDetail);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/UserDetail");
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UserLikeUp"), Authorize]
        public async Task<IActionResult> UserLikeUp(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                BaseUser baseUser = await _userManager.FindByIdAsync(userId);
                int userLikes = await _userService.UserLikesUpAsync(userId);
                await _logService.CreateLogAsync($"User Like Up Completed User: {baseUser.FullName}", baseUser.Id, false, "UserController/UserLikeUp");
                return Ok(userLikes);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/UserLikeUp");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("UserLikeDown"), Authorize]
        public async Task<IActionResult> UserLikeDown(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                BaseUser baseUser = await _userManager.FindByIdAsync(userId);
                int userLikes = await _userService.UserLikesDownAsync(userId);
                await _logService.CreateLogAsync($"User Like Down Completed User: {baseUser.FullName}", baseUser.Id, false, "UserController/UserLikeDown");
                return Ok(userLikes);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/UserLikeDown");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PasswordChange"), Authorize]
        public async Task<IActionResult> PasswordChange([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return BadRequest("User is not authenticated");
                BaseUser baseUser = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.CheckPasswordAsync(baseUser, changePasswordDTO.OldPassword);
                if (!result) return BadRequest("Incorrect Old Password");
                var passChangeResult = await _userManager.ChangePasswordAsync(baseUser, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
                if (!passChangeResult.Succeeded) return BadRequest(passChangeResult.Errors);
                await _logService.CreateLogAsync($"Password Changed Success User: {baseUser.FullName}", baseUser.Id, false, "UserController/PasswordChange");
                return Ok("Password Changed Success");
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/PasswordChange");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                string user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (user == null) return BadRequest("User is not authenticated");
                BaseUser baseUser = await _userManager.FindByIdAsync(userId);
                await _logService.CreateLogAsync($"GetByUserId Success UserId: {baseUser.Id}", baseUser.Id, false, "UserController/GetByUserId");
                return Ok(baseUser);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetByUserId");
                return NotFound("User Not Found");
            }
        }


        [Authorize]
        [HttpGet("GetUserDetailById")]
        public async Task<IActionResult> GetUserDetailById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                BaseUser user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound("User not found");
                UserDetailCardVM userDetailCardVM = new UserDetailCardVM()
                {
                    UserName = user.UserName,
                    Badge = user.Badge,
                    AnswerCount = user.Answers.Count,
                    Likes = user.Likes,
                };
                if (userDetailCardVM == null) return NotFound("User not found");
                await _logService.CreateLogAsync($"GetUserDetailById Success UserId: {userId}", userId, false, "UserController/GetUserDetailById");
                return Ok(userDetailCardVM);

            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetUserDetailById");
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                BaseUser baseUser = await _userManager.FindByEmailAsync(loginUserDTO.Email);
                if (baseUser == null) return NotFound("Invalid Email or Password");
                var result = await _userManager.CheckPasswordAsync(baseUser, loginUserDTO.Password);
                if (!result) return NotFound("Invalid Email or Password");
                bool emailConfirmed = await _userManager.IsEmailConfirmedAsync(baseUser);
                if (!emailConfirmed) return BadRequest("Please confirm your account. Check your email.");
                var userRole = await _userManager.GetRolesAsync(baseUser);
                var jwtToken = _jwtTokenService.GenerateToken(baseUser.Id, baseUser.UserName, baseUser.Email, userRole.FirstOrDefault(), _configuration);
                LoginUserVM loginUserVM = new LoginUserVM()
                {
                    Id = baseUser.Id,
                    Email = baseUser.Email,
                    FullName = baseUser.FullName,
                    UserName = baseUser.UserName,
                    Role = userRole.FirstOrDefault(),
                    Token = jwtToken,
                };
                await _signInManager.SignInAsync(baseUser, isPersistent: true);
                await _logService.CreateLogAsync($"Login Success {baseUser.FullName}", baseUser.Id, false, "UserController/Login");
                return Ok(loginUserVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/Login");
                return BadRequest("Login Error");
            }
        }


        [Authorize]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Logout error, UserId is null");
            try
            {
                await _signInManager.SignOutAsync();
                await _logService.CreateLogAsync($"Logout Success UserId: {userId}", userId, false, "UserController/Logout");
                return Ok(true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, userId, true, "UserController/Logout");
                return BadRequest("Logout Error");
            }
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return BadRequest("User is not authenticated");
                BaseUser baseUser = await _userManager.FindByIdAsync(userId);
                if (baseUser == null) return BadRequest("User not found");
                userUpdateDTO.Id = userId;
                BaseUser resultUser = await _userService.UpdateUser(userUpdateDTO);
                if (resultUser == null) return BadRequest($"Update Error User ID: {userId}");
                UpdateAfterVM updateAfterVM = new UpdateAfterVM()
                {
                    Id = resultUser.Id,
                    FirstName = resultUser.FirstName,
                    LastName = resultUser.LastName,
                    UserName = resultUser.UserName,
                    Region = resultUser.Region,
                    Gender = resultUser.Gender,
                };
                await _logService.CreateLogAsync($"Update User Success {baseUser.FullName}", baseUser.Id, false, "UserController/UpdateUser");
                return Ok(updateAfterVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/UpdateUser");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllAnswerByUserId")]
        public async Task<IActionResult> GetAllAnswersByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return BadRequest("UserId is null");
            try
            {
                string userClaimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userClaimId == null) return BadRequest("User is not authenticated");
                var serviceResult = await _userService.GetAllAnswersByUserAsync(userId);
                if (serviceResult == null) return NotFound("No answers from user");
                IEnumerable<AnswerUserVM> answerUserVM = serviceResult.Select(x => new AnswerUserVM
                {
                    Likes = x.Likes,
                    AnswerContent = x.AnswerContent,
                    AnswerDate = x.AnswerDate,
                    ProblemId = x.ProblemId,
                    UserId = x.UserId
                });
                await _logService.CreateLogAsync($"GetAllAnswersByUserId Success UserId: {userId}", userId, false, "UserController/GetAllAnswersByUserId");
                return Ok(answerUserVM);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetAllAnswersByUserId");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUserByRegion")]
        public async Task<IActionResult> GetAllUserByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region)) return BadRequest("Region is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                IEnumerable<BaseUser> result = await _userService.GetAllUserByRegion(region);
                await _logService.CreateLogAsync($"GetAllUserByRegion Success UserId: {userID}", userID, false, "UserController/GetAllAnswersByUserId");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetAllUserByRegion");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllCountUserByRegion")]
        public async Task<IActionResult> GetAllCountUserByRegion(string region)
        {
            if (string.IsNullOrWhiteSpace(region)) return BadRequest("Region is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                int result = await _userService.GetAllCountUserByRegion(region);
                await _logService.CreateLogAsync($"GetAllCountUserByRegion Success UserId: {userID}", userID, false, "UserController/GetAllCountUserByRegion");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetAllCountUserByRegion");
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllCountUserByGender")]
        public async Task<IActionResult> GetAllCountUserByGender(string gender)
        {
            if (string.IsNullOrWhiteSpace(gender)) return BadRequest("Gender is null");
            try
            {
                string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userID == null) return BadRequest("User is not authenticated");
                int result = await _userService.GetAllCountUserByGender(gender);
                await _logService.CreateLogAsync($"GetAllCountUserByGender Success UserId: {userID}", userID, false, "UserController/GetAllCountUserByGender");
                return Ok(result);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogAsync(ex.Message, User.FindFirstValue(ClaimTypes.NameIdentifier), true, "UserController/GetAllCountUserByGender");
                return BadRequest(ex.Message);
            }
        }
    }
}
