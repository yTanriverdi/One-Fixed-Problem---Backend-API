using AutoMapper;
using OFP_CORE.Entities;
using OFP_DAL.Repos.Interfaces.EntityRepoInterfaces;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Services.Concrete
{
    public class SuggestService : ISuggestService
    {
        private readonly ISuggestRepository _suggestRepository;
        private readonly IMapper _mapper;

        public SuggestService(ISuggestRepository suggestRepository, IMapper mapper)
        {
            _suggestRepository = suggestRepository;
            _mapper = mapper;
        }
        public async Task<Suggest> CreateSuggestAsync(SuggestCreateDTO suggest)
        {
            Suggest newSuggest = _mapper.Map<Suggest>(suggest);
            return await _suggestRepository.CreateSuggestAsync(newSuggest);
        }

        public async Task<IEnumerable<Suggest>> GetAllSuggestsAsync()
        {
            return await _suggestRepository.GetAllSuggestsAsync();
        }

        public async Task<IEnumerable<Suggest>> GetAllSuggestsIsSuggestAsync()
        {
            return await _suggestRepository.GetAllSuggestsIsSuggestAsync();
        }

        public async Task<IEnumerable<Suggest>> GetAllSuggestsNoSuggestAsync()
        {
            return await _suggestRepository.GetAllSuggestsNoSuggestAsync();
        }

        public async Task<Suggest> GetSuggestAsync(int suggestID)
        {
            if (suggestID == null) return null;
            return await _suggestRepository.GetSuggestAsync(suggestID);
        }
    }
}
