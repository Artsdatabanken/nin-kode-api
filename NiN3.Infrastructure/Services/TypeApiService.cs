using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NiN3.Core.Models;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.in_data;
using NiN3.Infrastructure.Mapping;


namespace NiN3.Infrastructure.Services
{
    public class TypeApiService : ITypeApiService
    {
        private readonly ILogger<TypeApiService> _logger;
        private readonly NiN3DbContext _context;
        private IConfiguration _conf;
        private IMapper _mapper;
        private List<CsvdataImporter_htg_ht_gt_mapping> csvdataImporter_Htg_Ht_Gt_Mappings;
        private List<Versjon> Domenes;
        private NiN3DbContext inmemorydb;
        private ILogger<TypeApiService> logger;

        public TypeApiService(NiN3DbContext context, ILogger<TypeApiService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all codes for a given version.
        /// </summary>
        /// <param name="versjon">The version to retrieve codes for.</param>
        /// <returns>A VersjonDto containing all codes for the given version.</returns>
        public async Task<VersjonDto> AllCodesAsync(string versjon)
        {
            var mapper = NiNkodeMapper.Instance;
            //var typer =  await _context.Type.ToListAsync();
           
            Versjon version = await _context.Versjon.Where(v => v.Navn == versjon)
                .Include(v => v.Typer.OrderBy(t => t.Langkode))
                .ThenInclude(type => type.Hovedtypegrupper.OrderBy(htg => htg.Langkode))
                .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedtyper.OrderBy(ht => ht.Langkode))
                .ThenInclude(hovedtype => hovedtype.Grunntyper.OrderBy(t => t.Langkode))
                .Include(v => v.Typer)
                .ThenInclude(type => type.Hovedtypegrupper)
                .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedtyper)
                .ThenInclude(hovedtype => hovedtype.Hovedtype_Kartleggingsenheter)
                .ThenInclude(Hovedtype_Kartleggingsenheter => Hovedtype_Kartleggingsenheter.Kartleggingsenhet)
                .ThenInclude(kartleggingsenhet => kartleggingsenhet.Grunntyper)
                .Include(v => v.Typer.OrderBy(t => t.Langkode))
                .ThenInclude(type => type.Hovedtypegrupper.OrderBy(htg => htg.Langkode))
                .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedoekosystemer)
                .AsNoTracking()
                .FirstAsync();

            return mapper.Map(version);
            //return _mapper.Map<VersjonDto>(version);
        }

        public TypeDto GetTypeByKortkode(string kode, string versjon) {
            //check if kode exist first before execution of complex query
            var typecount = _context.Type.Where(t => t.Kode == kode && t.Versjon.Navn == versjon).Count();
            if (typecount == 0) return null;

            NiN3.Core.Models.Type type = _context.Type.Where(t => t.Kode == kode && t.Versjon.Navn == versjon)
                .Include(type => type.Hovedtypegrupper.OrderBy(htg => htg.Langkode))
                    .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedtyper.OrderBy(ht => ht.Langkode))
                        .ThenInclude(hovedtype => hovedtype.Grunntyper.OrderBy(t => t.Langkode))
        .Include(type => type.Hovedtypegrupper)
            .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedtyper)
                .ThenInclude(hovedtype => hovedtype.Hovedtype_Kartleggingsenheter)
                .ThenInclude(Hovedtype_Kartleggingsenheter => Hovedtype_Kartleggingsenheter.Kartleggingsenhet)
                .ThenInclude(kartleggingsenhet => kartleggingsenhet.Grunntyper)
        .Include(type => type.Hovedtypegrupper.OrderBy(htg => htg.Langkode))
            .ThenInclude(hovedtypegruppe => hovedtypegruppe.Hovedoekosystemer)
        .Include(type =>type.Versjon)
             .AsNoTracking()
             .First();
            //return type != null ? _mapper.Map<TypeDto>(type) : null;
            return type != null ? NiNkodeMapper.Instance.Map(type) : null;
        }

        public KlasseDto GetTypeklasse(string kortkode, string versjon) {
            //var mapper = NiNkodeMapper.Instance;
            var alleKortkoderForType = _context.AlleKortkoder.Where(a => a.Kortkode == kortkode && a.Versjon.Navn == versjon).FirstOrDefault();
            return alleKortkoderForType != null ? NiNkodeMapper.Instance.Map(alleKortkoderForType) : null;            
            //return alleKortkoderForType != null ? _mapper.Map<KlasseDto>(alleKortkoderForType) : null;
        }

        public HovedtypegruppeDto GetHovedtypegruppeByKortkode(string kode, string versjon)
        {
            var exists = _context.Hovedtypegruppe.Any(htg => htg.Kode == kode && htg.Versjon.Navn == versjon);
            if (!exists) return null;
            var hovedtypegruppe = _context.Hovedtypegruppe.Where(htg => htg.Kode == kode && htg.Versjon.Navn == versjon)
                .Include(htg => htg.Hovedtyper.OrderBy(ht => ht.Langkode))
                    .ThenInclude(ht => ht.Grunntyper.OrderBy(t => t.Langkode))
                .Include(htg => htg.Hovedtyper)
                    .ThenInclude(ht => ht.Hovedtype_Kartleggingsenheter)
                        .ThenInclude(hke => hke.Kartleggingsenhet)
                            .ThenInclude(ke => ke.Grunntyper)
                .Include(htg => htg.Hovedoekosystemer)               
                .Include(htg => htg.Versjon)
                .AsNoTracking()
                .FirstOrDefault();
            var konverteringer = _context.Konvertering.Where(Konvertering => 
                                                             Konvertering.Kode == hovedtypegruppe.Kode && 
                                                             Konvertering.Versjon.Id == hovedtypegruppe.Versjon.Id).Include(k=>k.Versjon).Include(k => k.ForrigeVersjon)
                .AsNoTracking()
                .ToList();
            hovedtypegruppe.Konverteringer = konverteringer;
            return hovedtypegruppe != null ? NiNkodeMapper.Instance.Map(hovedtypegruppe) : null;
        }

        public HovedtypeDto GetHovedtypeByKortkode(string kode, string versjon)
        {
            HovedtypeDto hovedtypeDto= null;
            var hovedtype = _context.Hovedtype.Where(ht => ht.Kode == kode && ht.Versjon.Navn == versjon)
                .Include(ht => ht.Grunntyper.OrderBy(t => t.Langkode))
                .Include(ht => ht.Hovedtype_Kartleggingsenheter)  //NOT WORKING?
                    .ThenInclude(hke => hke.Kartleggingsenhet)
                        .ThenInclude(ke => ke.Grunntyper.OrderBy(t => t.Langkode))
                .Include(ht => ht.HovedtypeVariabeltrinn)
                    .ThenInclude(hvt => hvt.Maaleskala)
                    .ThenInclude(m => m.Trinn)
                .Include(gt => gt.HovedtypeVariabeltrinn)
                    .ThenInclude(hvt => hvt.Trinn)
                .Include(gt => gt.HovedtypeVariabeltrinn)
                    .ThenInclude(gvt => gvt.Variabelnavn)
                .Include(ht => ht.Versjon)
                .AsNoTracking()
                .FirstOrDefault();
            //var hovedtype_kartleggingsenheter = _context.Hovedtype_Kartleggingsenhet.Where(x => x.Hovedtype == hovedtype).Distinct().ToList();
            //hovedtype.Hovedtype_Kartleggingsenheter = hovedtype_kartleggingsenheter;
            if (hovedtype != null) 
            {
                //get grunntyper for KLE
                //for each kartleggingsenhet in hovedtype.Hovedtype_Kartleggingsenheter fetch grunntyper from kartleggingsenhet_grunntyper
                foreach (var hovedtype_kartleggingsenhet in hovedtype.Hovedtype_Kartleggingsenheter)
                {
                    hovedtype_kartleggingsenhet.Kartleggingsenhet.Grunntyper = _context.Kartleggingsenhet_Grunntype.Where(kg => kg.Kartleggingsenhet == hovedtype_kartleggingsenhet.Kartleggingsenhet)
                        .Select(kg => kg.Grunntype).OrderBy(t => t.Langkode)
                        .AsNoTracking()
                        .ToList();
                }
                hovedtype.Konverteringer = _context.Konvertering.Where(Konvertering =>
                                                             Konvertering.Kode == hovedtype.Kode &&
                                                             Konvertering.Versjon.Id == hovedtype.Versjon.Id)
                .Include(k => k.Versjon).Include(k => k.ForrigeVersjon)
                .AsNoTracking()
                .ToList();
                hovedtypeDto = NiNkodeMapper.Instance.Map(hovedtype);              
                foreach (var variabeltrinn in hovedtypeDto.Variabeltrinn)
                {
                    variabeltrinn.Maaleskala.Trinn = variabeltrinn.Maaleskala.Trinn.OrderBy(t => t.Verdi).ToList();
                }
            }
            return hovedtypeDto;
        }

        public GrunntypeDto GetGrunntypeByKortkode(string kode, string versjon)
        {
            GrunntypeDto gtd = null;
            //var mapper = NiNkodeMapper.Instance;
            var grunntype = _context.Grunntype.Where(gt => gt.Kode == kode && gt.Versjon.Navn == versjon)
                .Include(gt => gt.Versjon)
                .Include(gt => gt.GrunntypeVariabeltrinn)
                    .ThenInclude(gvt => gvt.Maaleskala)
                    .ThenInclude(m => m.Trinn) // Sort the Trinn by Verdi
                .Include(gt => gt.GrunntypeVariabeltrinn)
                    .ThenInclude(gvt => gvt.Trinn)
                .Include(gt => gt.GrunntypeVariabeltrinn)
                    .ThenInclude(gvt => gvt.Variabelnavn)
                .AsNoTracking()
                .FirstOrDefault();
            if (grunntype != null)
            {
                //Get konverteringer
                grunntype.Konverteringer = _context.Konvertering.Where(Konvertering =>
                                             Konvertering.Kode == grunntype.Kode &&
                                             Konvertering.Versjon.Id == grunntype.Versjon.Id)
                .Include(k => k.Versjon).Include(k => k.ForrigeVersjon)
                .AsNoTracking()
                .ToList();
                gtd = NiNkodeMapper.Instance.Map(grunntype);
                // Sort Trinn by Verdi here
                foreach (var variabeltrinn in gtd.Variabeltrinn)
                {
                    variabeltrinn.Maaleskala.Trinn = variabeltrinn.Maaleskala.Trinn.OrderBy(t => t.Verdi).ToList();
                }
            }
            return gtd;
        }


        public KartleggingsenhetDto GetKartleggingsenhetByKortkode(string kode, string versjon)
        {
            var kartleggingsenhet = _context.Kartleggingsenhet.Where(k => k.Kode == kode && k.Versjon.Navn == versjon)
                .Include(k => k.Versjon)
                //.Include(kartleggingsenhet => kartleggingsenhet.Grunntyper)
                .AsNoTracking()
                .FirstOrDefault();
            if (kartleggingsenhet != null) { 
            kartleggingsenhet.Grunntyper = _context.Kartleggingsenhet_Grunntype.Where(kg=> kg.Kartleggingsenhet == kartleggingsenhet)
                .Select(kg=> kg.Grunntype)
                .AsNoTracking()
                .ToList();
            }
            return kartleggingsenhet != null ? NiNkodeMapper.Instance.Map(kartleggingsenhet) : null;
        }
    }
}
