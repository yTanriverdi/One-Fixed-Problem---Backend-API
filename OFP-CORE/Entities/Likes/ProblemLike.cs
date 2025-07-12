using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Entities.Likes
{
    public class ProblemLike : IBaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProblemId { get; set; }

        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }

        // NAVIGATION PROTERTY
        public virtual BaseUser User { get; set; }
        public virtual Problem Problem { get; set; }
    }
}
