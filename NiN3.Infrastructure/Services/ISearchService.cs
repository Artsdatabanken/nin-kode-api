using NiN3.Core.Models.DTOs.search;
using NiN3.Core.Models.Enums;

namespace NiN3.Infrastructure.Services
{
    public interface ISearchService
    {
        public List<SearchResultDto> SimpleSearch(string searchTerm, KlasseEnum klasseEnum, SearchMethodEnum searchMethodEnum);
    }

}

