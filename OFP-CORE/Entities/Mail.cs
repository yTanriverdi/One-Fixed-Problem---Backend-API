using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Entities
{
    public class Mail : IBaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }

        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
