using Microsoft.AspNetCore.Identity;
using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OFP_CORE.Entities
{
    public class BaseUser : IdentityUser, IBaseEntity
    {
        public BaseUser()
        {
            Answers = new HashSet<Answer>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Region { get; set; }
        public string Gender { get; set; }
        public int Likes { get; set; } = 0; // KULLANICININ BEĞENİLERİ

        public Badge Badge { get; set; } = Badge.None; // KULLANICININ ROZETİ
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }

        // NAVIGATION PROPERTY
        [JsonIgnore]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}