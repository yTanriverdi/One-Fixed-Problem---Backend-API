using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Entities
{
    public class ReportComment : IBaseEntity
    {
        public int Id { get; set; }
        public string ReportUserId { get; set; }
        public int ReportedCommentId { get; set; }
        public ReportType ReportType { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.UnderReview;


        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }

        // NAVIGATION PROPERTY
        public virtual Comment Comment { get; set; }
    }
}
