using OFP_CORE.Entities.Likes;
using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Entities
{
    public class Comment : IBaseEntity
    {
        public Comment()
        {
            CommentLikes = new HashSet<CommentLike>();
            ReportComments = new HashSet<ReportComment>();
        }
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public int Likes { get; set; }
        public bool Report { get; set; } = false;


        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }

        // NAVIGATION PROPERTY
        public int AnswerId { get; set; }
        public virtual Answer Answer { get; set; }
        public string UserId { get; set; }
        public virtual BaseUser User { get; set; }

        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<ReportComment> ReportComments { get; set; }
    }
}
