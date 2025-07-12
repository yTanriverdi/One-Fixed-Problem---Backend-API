using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.DTOs.UpdateDTOs.Reports
{
    public class ReportCommentUpdateDTO
    {
        public int Id { get; set; }
        public ReportType ReportType { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
