using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using NiN3.Core.Models;
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.DTOs.variabel;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.Services
{
    public class VariabelApiService : IVariabelApiService
    {
        private readonly NiN3DbContext _context;
        private readonly ILogger<VariabelApiService> _logger;
        private NiN3DbContext inmemorydb;
        private ILogger<VariabelApiService> logger;
        private IMapper _mapper;

        /*public VariabelApiService(NiN3DbContext context, ILogger<VariabelApiService> logger)
        {
            _context = context;
            _logger = logger;

        }*/
        public VariabelApiService(NiN3DbContext context, ILogger<VariabelApiService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public VersjonDto AllCodes(string versjon)
        {
            Versjon _versjon = _context.Versjon.Where(v => v.Navn == versjon)
                .Include(v => v.Variabler.OrderBy(v => v.Langkode))
                .ThenInclude(variabel => variabel.Variabelnavn)
                .Include(v => v.Variabler.OrderBy(v => v.Langkode))
                .ThenInclude(variabel => variabel.Variabelnavn)
                .ThenInclude(variabelnavn => variabelnavn.VariabelnavnMaaleskala)
                .ThenInclude(maaletrinn => maaletrinn.Maaleskala)
                .Include(v => v.Variabler.OrderBy(v => v.Langkode))
                .ThenInclude(variabel => variabel.Variabelnavn)
                .ThenInclude(variabelnavn => variabelnavn.VariabelnavnMaaleskala)
                .ThenInclude(variabelnavnMaaleskala =>variabelnavnMaaleskala.Maaleskala)
                .ThenInclude(maaleskala => maaleskala.Trinn)
                .Select(v => new Versjon { Id = v.Id, Navn = v.Navn, Variabler = v.Variabler })
                .AsNoTracking()
                .FirstOrDefault();
            return NiNkodeMapper.Instance.Map(_versjon);
            //return _mapper.Map<VersjonDto>(_versjon);
        }



        public KlasseDto GetVariabelKlasse(string kortkode, string versjon) {
            var alleKortkoder = _context.AlleKortkoder.Where(a => a.Kortkode == kortkode && a.Versjon.Navn == versjon).FirstOrDefault();
            return alleKortkoder != null ? NiNkodeMapper.Instance.Map(alleKortkoder) : null;
        }

        public VariabelDto GetVariabelByKortkode(string kode, string versjon) {
            Variabel variabel = _context.Variabel.Where(v => v.Kode == kode && v.Versjon.Navn == versjon)
                .Include(variabel => variabel.Variabelnavn)
                .ThenInclude(variabelnavn => variabelnavn.VariabelnavnMaaleskala)
                .ThenInclude(vn_maaleskala => vn_maaleskala.Maaleskala)
                .ThenInclude(maaleskala => maaleskala.Trinn)
                .Include(variabelnavn => variabelnavn.Versjon)
                .AsNoTracking()
                .FirstOrDefault();
            return variabel != null ? NiNkodeMapper.Instance.Map(variabel) : null;
        }

        public VariabelnavnDto GetVariabelnavnByKortkode(string kode, string versjon)
        {
            var versjonObj = _context.Versjon.FirstOrDefault(v => v.Navn == versjon);
            var exists = _context.Variabelnavn.Any(vn => vn.Kode == kode && vn.Versjon.Navn == versjon);
            if (!exists) return null;
            Variabelnavn variabelnavn = _context.Variabelnavn.Where(v => v.Kode == kode && v.Versjon.Navn == versjon)
                .Include(variabelnavn => variabelnavn.VariabelnavnMaaleskala)
                .ThenInclude(maaletrinn => maaletrinn.Maaleskala)
                .Include(variabelnavn => variabelnavn.VariabelnavnMaaleskala)
                .ThenInclude(vn_maaleskala => vn_maaleskala.Maaleskala)
                .ThenInclude(maaleskala => maaleskala.Trinn)
                .Include(variabelnavn => variabelnavn.Versjon)
                .AsNoTracking()
                .FirstOrDefault();
            if (variabelnavn != null)
            {
                //Get konverteringer
                variabelnavn.Konverteringer = _context.Konvertering.Where(Konvertering =>
                                             Konvertering.Kode == variabelnavn.Kode &&
                                             Konvertering.Versjon.Id == variabelnavn.Versjon.Id)
                .Include(k => k.Versjon).Include(k => k.ForrigeVersjon)
                .AsNoTracking()
                .ToList();
                //Get konverteringer for each trinn
                foreach (var trinn in variabelnavn.VariabelnavnMaaleskala.SelectMany(m => m.Maaleskala.Trinn))
                {
                    trinn.Konverteringer = _context.Konvertering.Where(k => k.Kode == trinn.Verdi && k.Versjon.Id == versjonObj.Id)
                    .Include(k => k.Versjon)
                    .Include(k => k.ForrigeVersjon)
                    .AsNoTracking()
                    .ToList();
                }
            }
            VariabelnavnDto variabelnavnDto = variabelnavn != null ? NiNkodeMapper.Instance.Map(variabelnavn) : null;
            // Sort Trinn by Verdi here
            foreach (var variabeltrinn in variabelnavnDto.Variabeltrinn)
            {
                variabeltrinn.Trinn = variabeltrinn.Trinn.OrderBy(t => t.Verdi).ToList();
            }
            return variabelnavnDto;
        }

        public MaaleskalaDto GetMaaleskalaByMaaleskalanavn(string maaleskalanavn, string versjon)
        {
            var versjonObj = _context.Versjon.FirstOrDefault(v => v.Navn == versjon);
            var exists = _context.Maaleskala.Any(ms => ms.MaaleskalaNavn == maaleskalanavn);
            if (!exists) return null;
            var maaleskala = _context.Maaleskala.Where(m => m.MaaleskalaNavn == maaleskalanavn)
                .Include(maaleskala => maaleskala.Trinn)
                .FirstOrDefault();
            foreach (var trinn in maaleskala.Trinn)
            {
                trinn.Konverteringer = _context.Konvertering.Where(k => k.Kode == trinn.Verdi && k.Versjon.Id == versjonObj.Id)
                    .Include(k => k.Versjon)
                    .Include(k => k.ForrigeVersjon)
                    .AsNoTracking()
                    .ToList();
            }

            var maaleskalaDto = maaleskala != null ? NiNkodeMapper.Instance.Map(maaleskala) : null;
            maaleskalaDto.Trinn = maaleskalaDto.Trinn.OrderBy(t => t.Verdi).ToList();
            return maaleskalaDto;
        }
    }
}