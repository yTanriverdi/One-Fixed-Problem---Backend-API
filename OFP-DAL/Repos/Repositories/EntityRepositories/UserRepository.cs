using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
using OFP_CORE.Enums;
using OFP_DAL.Context;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_DAL.Repos.Repositories.BaseRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace OFP_DAL.Repos.Repositories.EntityRepositories
{
    public class UserRepository : BaseRepo<BaseUser>, IUserRepository
    {
        private readonly OneFixedProblemContext _oneFixedProblemContext;

        public UserRepository(OneFixedProblemContext context) : base(context)
        {
            _oneFixedProblemContext = context;
        }

        /// <summary>
        /// UserId'ye ait BaseUser döndürür.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BaseUser> GetUserByIdAsync(string userId)
        {
            return await _oneFixedProblemContext.BaseUsers.FindAsync(userId);
        }

        /// <summary>
        /// Tüm Kullanıcıları döndürür.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BaseUser>> GetAllUsersAsync()
        {
            return await _oneFixedProblemContext.BaseUsers.ToListAsync();
        }

        /// <summary>
        /// UserID' ye ait olan tüm Answerları döndürür
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Answer>> GetAllAnswersByUserAsync(string userID)
        {
            return await _oneFixedProblemContext.BaseUsers.Where(x => x.Answers.Any(a => a.UserId == userID)).SelectMany(x => x.Answers).ToListAsync();
        }

        /// <summary>
        /// Region'a göre ülkelere göre BaseUser listesi döner
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<BaseUser>> GetAllUserByRegion(string region)
        {
            IEnumerable<BaseUser> baseUsersByRegion = await _oneFixedProblemContext.BaseUsers.Where(x => x.Region == region && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
            if (baseUsersByRegion == null) return null;
            return baseUsersByRegion;
        }

        /// <summary>
        /// Region'a göre hangi ülkeden kaç kişi kayıtlı onu bulmak
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetAllCountUserByRegion(string region)
        {
            IEnumerable<BaseUser> baseUsersByRegion = await _oneFixedProblemContext.BaseUsers.Where(x => x.Region == region && (x.Status == Status.Active || x.Status == Status.Updated)).ToListAsync();
            if (baseUsersByRegion == null) return 0;
            return baseUsersByRegion.Count();
        }

        /// <summary>
        /// Gender'a göre hangi cinsiyetten kaç kişi kullanıyor onu döner.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> GetAllCountUserByGender(string gender)
        {
            return await _oneFixedProblemContext.BaseUsers.Where(x => x.Gender == gender.ToLower()).CountAsync();
        }

        /// <summary>
        /// UserID'ye ait olan BaseUser'in Likes'ını azaltır. BADGE AYARLAMASI YAPAR
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UserLikesDownAsync(string userId)
        {
            BaseUser baseUser = await _oneFixedProblemContext.BaseUsers.FindAsync(userId);
            if(baseUser.Likes == 0) return 0;
            baseUser.Likes -= 1;
            if (baseUser.Likes < 200)
            {
                baseUser.Badge = Badge.None;
            }
            else if (baseUser.Likes >= 200 && baseUser.Likes < 400)
            {
                baseUser.Badge = Badge.Iron;
            }
            else if (baseUser.Likes >= 400 && baseUser.Likes < 600)
            {
                baseUser.Badge = Badge.Silver;
            }
            else if (baseUser.Likes >= 600 && baseUser.Likes < 800)
            {
                baseUser.Badge = Badge.Gold;
            }
            else if (baseUser.Likes >= 800 && baseUser.Likes < 1000)
            {
                baseUser.Badge = Badge.Platinum;
            }
            else if (baseUser.Likes >= 1000)
            {
                baseUser.Badge = Badge.Diamond;
            }
            _oneFixedProblemContext.Update(baseUser);
            await _oneFixedProblemContext.SaveChangesAsync();
            return baseUser.Likes;
        }

        /// <summary>
        /// UserID'ye ait olan BaseUser'in Likes'ını artırır. BADGE AYARLAMASI YAPAR
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<int> UserLikesUpAsync(string userId)
        {
            BaseUser baseUser = await _oneFixedProblemContext.BaseUsers.FindAsync(userId);
            baseUser.Likes += 1;
            if (baseUser.Likes < 200)
            {
                baseUser.Badge = Badge.None;
            }
            else if (baseUser.Likes >= 200 && baseUser.Likes < 400)
            {
                baseUser.Badge = Badge.Iron;
            }
            else if (baseUser.Likes >= 400 && baseUser.Likes < 600)
            {
                baseUser.Badge = Badge.Silver;
            }
            else if (baseUser.Likes >= 600 && baseUser.Likes < 800)
            {
                baseUser.Badge = Badge.Gold;
            }
            else if (baseUser.Likes >= 800 && baseUser.Likes < 1000)
            {
                baseUser.Badge = Badge.Platinum;
            }
            else if (baseUser.Likes >= 1000)
            {
                baseUser.Badge = Badge.Diamond;
            }
            _oneFixedProblemContext.Update(baseUser);
            await _oneFixedProblemContext.SaveChangesAsync();
            return baseUser.Likes;
        }

        /// <summary>
        /// Kullanıcı için Email onaylaması yapar
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UserEmailConfirmAsync(string userId)
        {
            BaseUser baseUser = await _oneFixedProblemContext.BaseUsers.FindAsync(userId);
            if (baseUser == null) return false;
            baseUser.EmailConfirmed = true;
            _oneFixedProblemContext.Update(baseUser);
            await _oneFixedProblemContext.SaveChangesAsync();
            return true;
        }
    }
}
