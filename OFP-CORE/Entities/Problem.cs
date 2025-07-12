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
    public class Problem : IBaseEntity
    {
        public Problem()
        {
            Answers = new HashSet<Answer>();
            ProblemLikes = new HashSet<ProblemLike>();
        }

        public int Id { get; set; }
        public string Content { get; set; } // SORU İÇERİĞİ
        public bool Viewed { get; set; } = false; // SORU ÖNCEDEN GÖSTERİLDİ Mİ?
        public int Likes { get; set; } = 0; // BEĞENİLER
        public int AnswerCount { get; set; } = 0; // CEVAPLAR - KAÇ KİŞİ CEVAP VERDİ?
        public DateTime QuestionDate { get; set; } // SORUNUN SORULMA TARİHİ - EKLEME YAPILIRKEN SON AKTİF OLAN SORUNUN BİR SONRAKİ GÜNÜNE EKLENİYOR


        // INTERFACE'DEN GELENLER
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedTime { get; set; }


        // NAVIGATION PROPERTY
        [JsonIgnore]
        public virtual ICollection<Answer> Answers { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProblemLike> ProblemLikes { get; set; }
        
    }
}
