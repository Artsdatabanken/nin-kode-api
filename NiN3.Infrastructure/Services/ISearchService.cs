using NiN3.Core.Models.DTOs.search;
using NiN3.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.Services
{
    public interface ISearchService
    {
        public List<SearchResultDto> SimpleSearch(string searchTerm, KlasseEnum klasseEnum, SearchMethodEnum searchMethodEnum);
    }

}

