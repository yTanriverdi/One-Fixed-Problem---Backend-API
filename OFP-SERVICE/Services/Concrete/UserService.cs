using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.Json;
using OFP_CORE.Entities;
using OFP_CORE.Enums;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<BaseUser> _userManager;
        private readonly RoleManager<BaseUserRole> _roleManager;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, UserManager<BaseUser> userManager, RoleManager<BaseUserRole> roleManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Answer>> GetAllAnswersByUserAsync(string userID)
        {
            return await _userRepository.GetAllAnswersByUserAsync(userID);
        }

        public async Task<int> GetAllCountUserByGender(string gender)
        {
            return await _userRepository.GetAllCountUserByGender(gender);
        }

        public async Task<int> GetAllCountUserByRegion(string region)
        {
            return await _userRepository.GetAllCountUserByRegion(region);
        }

        public async Task<IEnumerable<BaseUser>> GetAllUserByRegion(string region)
        {
            return await _userRepository.GetAllUserByRegion(region);
        }

        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<BaseUser> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public Task<BaseUser> PasswordChangeAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseUser> RegisterUserAsync(UserCreateDTO user)
        {
            BaseUser newUser = new BaseUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                Gender = user.Gender,
                Region = user.Region,
                UserName = user.UserName
            };

            PasswordHasher<BaseUser> hasher = new PasswordHasher<BaseUser>();
            newUser.PasswordHash = hasher.HashPassword(newUser, user.Password);

            var oldUser = await _userManager.FindByEmailAsync(user.Email);
            if (oldUser != null) return null;
            var result = await _userManager.FindByNameAsync(user.UserName);
            if (result != null) return null;
            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded) throw new Exception("User Create Error");
            var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
            if (!roleResult.Succeeded) throw new Exception("Register User, Role Add Error");
            BaseUser createdUser = await _userManager.FindByEmailAsync(user.Email);
            if (createdUser == null) throw new Exception("Register Find Mail Async");
            return createdUser;
        }

        public async Task<BaseUser> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            BaseUser baseUser = await _userRepository.GetUserByIdAsync(userUpdateDTO.Id);
            if (baseUser == null) throw new Exception("User not found");
            _mapper.Map(userUpdateDTO, baseUser);
            baseUser.UpdatedDate = DateTime.UtcNow;
            baseUser.Status = Status.Updated;
            var result = await _userManager.UpdateAsync(baseUser);
            if (result.Succeeded)
                return baseUser;
            else
                throw new Exception("User update error");
        }

        public async Task<bool> UserEmailConfirmAsync(string userId)
        {
            return await _userRepository.UserEmailConfirmAsync(userId);
        }

        public async Task<int> UserLikesDownAsync(string userId)
        {
            return await _userRepository.UserLikesDownAsync(userId);
        }

        public async Task<int> UserLikesUpAsync(string userId)
        {
            return await _userRepository.UserLikesUpAsync(userId);
        }
    }
}
