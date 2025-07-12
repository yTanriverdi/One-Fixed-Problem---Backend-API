using OFP_CORE.Entities;
using OFP_SERVICE.DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Interfaces
{
    public interface ISuggestService
    {
        /// <summary>
        /// SuggestID'ye ait olan Suggest nesnesini getirir.
        /// </summary>
        /// <param name="suggestID"></param>
        /// <returns></returns>
        Task<Suggest> GetSuggestAsync(int suggestID);

        /// <summary>
        /// Tüm SUGGESTLERİ DÖNDÜRÜR IENUMERABLE
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Suggest>> GetAllSuggestsAsync();

        /// <summary>
        /// Tüm ÖNERİ OLAN SUGGESTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Suggest>> GetAllSuggestsIsSuggestAsync();

        /// <summary>
        /// Tüm ŞİKAYET OLAN SUGGESTLERİ DÖNDÜRÜR
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Suggest>> GetAllSuggestsNoSuggestAsync();

        /// <summary>
        /// SuggestCreateDTO nesnesi ile Suggest oluşturur.
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        Task<Suggest> CreateSuggestAsync(SuggestCreateDTO suggest);
    }
}
