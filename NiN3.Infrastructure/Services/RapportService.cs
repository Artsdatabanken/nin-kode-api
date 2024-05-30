using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NiN3.Core.Models;
using NiN3.Core.Models.DTOs.rapport;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Mapping;
using System.Text;

namespace NiN3.Infrastructure.Services
{
    public class RapportService : IRapportService
    {
        private readonly ILogger<RapportService> _logger;
        private readonly NiN3DbContext _context;
        private IConfiguration _conf;
        private IMapper _mapper;
        private NiN3DbContext inmemorydb;

        public RapportService(NiN3DbContext context, ILogger<RapportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetDataDate()
        {
            var result = _context.Database.SqlQuery<string>($"SELECT Verdi from [db_info]").ToList();
            return result.First(); // Return null if no value is found
        }

        public List<KodeoversiktDto> GetKodeSummary(string versjon)
        {
            var mapper = NiNkodeMapper.Instance;
            var kodeoversiktDtoList = new List<KodeoversiktDto>();
            var kodeoversiktList = new List<Kodeoversikt>();
            var typer = _context.Type.Select(t => new Kodeoversikt()
            {
                Kortkode = t.Kode,
                Langkode = t.Langkode,
                Navn = t.Navn,
                Klasse = "Type"
            }).ToList(); //todo: where versjon 3.0
            kodeoversiktList.AddRange(typer);
            var hovedtypegrupper = _context.Hovedtypegruppe.Select(htg => new Kodeoversikt()
            {
                Kortkode = htg.Kode,
                Langkode = htg.Langkode,
                Navn = htg.Navn,
                Klasse = "Hovedtypegruppe"
            }).ToList();
            kodeoversiktList.AddRange(hovedtypegrupper);
            var hovedtyper = _context.Hovedtype.Select(htg => new Kodeoversikt()
            {
                Kortkode = htg.Kode,
                Langkode = htg.Langkode,
                Navn = htg.Navn,
                Klasse = "Hovedtype"
            }).ToList();
            kodeoversiktList.AddRange(hovedtyper);
            var grunntyper = _context.Grunntype.Select(htg => new Kodeoversikt()
            {
                Kortkode = htg.Kode,
                Langkode = htg.Langkode,
                Navn = htg.Navn,
                Klasse = "Grunntype"
            }).ToList();
            kodeoversiktList.AddRange(grunntyper);
            var kartleggingsenheter = _context.Kartleggingsenhet.Select(ke => new Kodeoversikt()
            {
                Kortkode = ke.Kode,
                Langkode = ke.Langkode,
                Navn = ke.Navn,
                Klasse = "Kartleggingsenhet"
            }).ToList();
            kodeoversiktList.AddRange(kartleggingsenheter);
            var variabler = _context.Variabel.Select(htg => new Kodeoversikt()
            {
                Kortkode = htg.Kode,
                Langkode = htg.Langkode,
                Navn = htg.Navn,
                Klasse = "Variabel"
            }).ToList();
            kodeoversiktList.AddRange(variabler);
            var variabelnavn = _context.Variabelnavn.Select(htg => new Kodeoversikt()
            {
                Kortkode = htg.Kode,
                Langkode = htg.Langkode,
                Navn = htg.Navn,
                Klasse = "Variabelnavn"
            }).ToList();
            kodeoversiktList.AddRange(variabelnavn);
            foreach (var t in kodeoversiktList) { 
                kodeoversiktDtoList.Add(mapper.Map(t));
            }
            return kodeoversiktDtoList;
        }

        public string MakeKodeoversiktCSV(string versjon, string separator=";") {
            var kodeoversiktDtoList = GetKodeSummary(versjon);
            var csv = new StringBuilder();
            csv.AppendLine($"Klasse{separator}Navn{separator}Kortkode{separator}Langkode");
            foreach (var kodeoversiktDto in kodeoversiktDtoList)
            {
                if (separator == ",") {
                    kodeoversiktDto.Navn = "\"" + kodeoversiktDto.Navn.Replace("\"", "\"\"") + "\"";
                }
                var newLine = $"{kodeoversiktDto.Klasse}{separator}{kodeoversiktDto.Navn}{separator}{kodeoversiktDto.Kortkode}{separator}{kodeoversiktDto.Langkode}";
                csv.AppendLine(newLine);
            }
            return csv.ToString();
        }

        public byte[] MakeKodeoversiktXlsx(string versjon)
        {
            var kodeoversiktDtoList = GetKodeSummary(versjon);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Kodeoversikt");

                // Set the headers
                worksheet.Cell(1, 1).Value = "Klasse";
                worksheet.Cell(1, 2).Value = "Navn";
                worksheet.Cell(1, 3).Value = "Kortkode";
                worksheet.Cell(1, 4).Value = "Langkode";

                // Fill the rows with data
                int i = 2;
                foreach (var kodeoversiktDto in kodeoversiktDtoList)
                {
                    worksheet.Cell(i, 1).Value = kodeoversiktDto.Klasse;
                    worksheet.Cell(i, 2).Value = kodeoversiktDto.Navn ?? ""; //Null coalescing operator to avoid null reference
                    worksheet.Cell(i, 3).Value = kodeoversiktDto.Kortkode;
                    worksheet.Cell(i, 4).Value = kodeoversiktDto.Langkode;
                    i++;
                }

                // Save the spreadsheet
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
