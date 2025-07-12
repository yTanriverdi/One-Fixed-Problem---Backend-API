using OFP_CORE.EntitiesInterfaces;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Entities
{
    public class Suggest : IBaseEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; } // Kullanıcının ismi
        public string Content { get; set; }
        public bool IsSuggest { get; set; } = true; // ÖNERİ Mİ ŞİKAYET Mİ ?


        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }
    }
}
