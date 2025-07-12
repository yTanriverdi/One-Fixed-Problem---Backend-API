using Microsoft.EntityFrameworkCore.Migrations.Operations;
using OFP_CORE.Entities.Likes;
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
    public class Answer : IBaseEntity
    {
        public Answer() {
            Comments = new HashSet<Comment>();
            ReportAnswers = new HashSet<ReportAnswer>();
        }

        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public int Likes { get; set; } // BEĞENİLER

        public DateTime AnswerDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndAnswerDate => AnswerDate.AddMinutes(30);

        public bool? EndAnswer { get; set; } = false;
        public bool Report { get; set; } = false;


        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }

        // NAVIGATION PROPERTY
        public string UserId { get; set; }
        public int ProblemId { get; set; }
        public virtual BaseUser User { get; set; }
        [JsonIgnore]
        public virtual Problem Problem { get; set; }

        // CEVABIN YORUMLARI
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<AnswerLike> AnswerLikes { get; set; }
        [JsonIgnore]
        public virtual ICollection<ReportAnswer> ReportAnswers { get; set; }
    }
}
