using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_CORE.Enums
{
    public enum ReportType
    {
        OffensiveLanguage = 0,          // Küfürlü dil
        Harassment = 1,                 // Hakaret
        Spam = 2,                       // Spam
        Violence = 3,                   // Şiddet içeriği
        InappropriateContent = 4,       // Uygunsuz içerik
        Other = 5                       // Diğer (belirli bir kategoriye uymayan durumlar)
    }
}
