using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.EntitiesInterfaces
{
    public interface IBaseEntity
    {
        Status Status { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        DateTime? DeletedTime { get; set; }
    }
}