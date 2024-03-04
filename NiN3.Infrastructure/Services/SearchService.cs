
using NiN3.Core.Models.DTOs.search;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using AutoMapper;
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
                    var queryT = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'Type' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryT, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
 
                    break;
                case KlasseEnum.HTG:
                    // Implementation logic for Value2
                    var queryHTG = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'Hovedtypegruppe' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryHTG, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.HT:
                    // Implementation logic for Value3
                    var queryHT = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'Hovedtype' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryHT, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.GT:
                    // Implementation logic for Value3
                    var queryGT = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'Grunntype' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryGT, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.V:
                    // Implementation logic for Value3
                    var queryV = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'V' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryV, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.VN:
                    // Implementation logic for Value3
                    var queryVN = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'VN' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryVN, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.KE:
                    // Add more cases for all possible values of KlasseEnum
                    var queryKE = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Klasse = 'KE' AND Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryKE, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                case KlasseEnum.ALL:
                    // Implementation logic for Value3
                    var queryALL = "SELECT Klasse, Kode, Langkode, Navn FROM AlleLangkoderView WHERE Navn COLLATE NOCASE LIKE @searchTerm";
                    resultlist = _context.Set<Core.Models.SearchResult>().FromSqlRaw(queryALL, new SqliteParameter("@searchTerm", searchTermQ)).ToList();
                    break;
                default:
                    // Default implementation logic
                    break;

            }
            return NiNkodeMapper.Instance.MapSearchResults(resultlist);
        }
    }
}
