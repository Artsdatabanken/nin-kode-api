using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models;
using NiN3.Core.Models.DTOs.search;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Mapping;

namespace NiN3.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly NiN3DbContext _context;

        public SearchService(NiN3DbContext context)
        {
            _context = context;
        }
        public List<SearchResultDto> SimpleSearch(string searchTerm, KlasseEnum klasseEnum, SearchMethodEnum searchMethodEnum)
        {

            var resultlist = new List<SearchResult>();
            var searchTermQ = searchMethodEnum == SearchMethodEnum.SW ? searchTerm + "%" : "%" + searchTerm + "%";
            switch (klasseEnum)
            {
                case KlasseEnum.T:
                    // Implementation logic for Value1
                    // SQL query to retrieve data from the database using the NiN3DbContext
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Type" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.HTG:
                    // Implementation logic for Value2
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Hovedtypegruppe" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.HT:
                    // Implementation logic for Value3
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Hovedtype" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.GT:
                    // Implementation logic for Value3
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Grunntype" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.V:
                    // Implementation logic for Value3
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Variabel" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.VN:
                    // Implementation logic for Value3
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Variabelnavn" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                case KlasseEnum.KE:
                    // Add more cases for all possible values of KlasseEnum
                    resultlist = _context.AlleLangkoderView
                        .Where(x => x.Klasse == "Kartleggingsenhet" && EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        })
                        .ToList();
                    break;
                case KlasseEnum.ALL:
                    // Implementation logic for Value3
                    resultlist = _context.AlleLangkoderView
                        .Where(x => EF.Functions.Like(x.Navn, $"{searchTermQ}"))
                        .Select(x => new SearchResult
                        {
                            Klasse = x.Klasse,
                            Kode = x.Kode,
                            Langkode = x.Langkode,
                            Navn = x.Navn
                        }).ToList();
                    break;
                default:
                    // Default implementation logic
                    break;

            }
            return NiNkodeMapper.Instance.MapSearchResults(resultlist);
        }
    }
}
