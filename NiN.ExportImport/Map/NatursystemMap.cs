namespace NiN.ExportImport.Map
{
    using System.Text.Json;
    using CsvHelper.Configuration;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Common;

    public sealed class NatursystemMap : ClassMap<Natursystem>
    {
        public NatursystemMap()
        {
            //Map(x => x.Version.Navn).Name("natursystemVersion");
            //Map(x => x.Kategori).Name("natursystemKategori");
            Map(x => x.Navn).Name("natursystemNavn");
            Map(x => x.Kode.KodeName).Name("natursystemKode");
            //References<NatursystemKodeMap>(x => x.Kode);
        }
    }

    public sealed class HovedtypegruppeMap : ClassMap<Hovedtypegruppe>
    {
        public HovedtypegruppeMap()
        {
            //References<HovedtypegruppeKodeMap>(x => x.Kode);
            //Map(x => x.Version.Navn).Name("hovedtypegruppeVersion");
            //Map(x => x.Kategori).Name("hovedtypegruppeKategori");
            Map(x => x.Navn).Name("hovedtypegruppeNavn");
            Map(x => x.Kode.KodeName).Name("hovedtypeGruppeKode");
            //References<HovedtypeMap>(x => x.UnderordnetKoder);
            //foreach (var property in typeof(NatursystemKode).GetProperties())
            //{
            //    switch (property.Name.ToLowerInvariant())
            //    {
            //        case "kode":
            //            References<HovedtypegruppeKodeMap>(x => x.Kode);
            //            break;
            //        default:
            //            continue;
            //    }
            //}
        }
    }

    public sealed class HovedtypeMap : ClassMap<Hovedtype>
    {
        public HovedtypeMap()
        {
            //Map(x => x.Version.Navn).Name("hovedtypeVersion");
            //Map(x => x.Kategori).Name("hovedtypeKategori");
            Map(x => x.Navn).Name("hovedtypeNavn");
            Map(x => x.Kode.KodeName).Name("hovedtypeKode");
        }
    }

    public sealed class GrunntypeMap : ClassMap<Grunntype>
    {
        public GrunntypeMap()
        {
            //Map(x => x.Version.Navn).Name("grunntypeVersion");
            //Map(x => x.Kategori).Name("grunntypeKategori");
            Map(x => x.Navn).Name("grunntypeNavn");
            Map(x => x.Kode.KodeName).Name("grunntypeKode");
        }
    }

    public sealed class MiljovariabelMap : ClassMap<Miljovariabel>
    {
        public MiljovariabelMap()
        {
            //Map(x => x.Version.Navn).Name("miljovariabelVersion");
            Map(x => x.Type).Name("miljovariabelType");
            Map(x => x.Hovedtype.Kode.KodeName).Name("miljovariabelHovedtype");
            //Map(x => x.LkmKategori).Name("miljovariabelKategori");
            Map(x => x.Kode.Kode).Name("miljovariabelKode");
            Map(x => x.Navn).Name("miljovariabelNavn");
        }
    }

    public sealed class KartleggingsenhetMap : ClassMap<Kartleggingsenhet>
    {
        public KartleggingsenhetMap()
        {
            //Map(x => x.Version.Navn).Name("kartleggingsenhetVersion");
            //Map(x => x.Kategori).Name("kartleggingsenhetKategori");
            Map(x => x.Malestokk).Name("kartleggingsenhetMalestokk");
            Map(x => x.Definisjon).Name("kartleggingsenhetDef");
            Map(x => x.Kode.KodeName).Name("kartleggingsenhetKode");
        }
    }

    public sealed class TrinnMap : ClassMap<Trinn>
    {
        public TrinnMap()
        {
            //Map(x => x.Version.Navn).Name("trinnVersion");
            //Map(x => x.Kategori).Name("trinnKategori");
            Map(x => x.Kode.KodeName).Name("trinnKode");
            Map(x => x.Navn).Name("trinnNavn");
        }
    }

    public sealed class BasistrinnMap : ClassMap<Basistrinn>
    {
        public BasistrinnMap()
        {
            //Map(x => x.Version.Navn).Name("basistrinnVersion");
            //Map(x => x.Kategori).Name("basistrinnKategori");
            //Map(x => x.Kode.KodeName).Name("basistrinnKode");
            Map(x => x.Navn).Name("basistrinnNavn");
        }
    }

    public sealed class NatursystemKodeMap : ClassMap<NatursystemKode>
    {
        public NatursystemKodeMap()
        {
            Map(x => x.KodeName).Name("natursystemKodenavn");
        }
    }

    public sealed class HovedtypegruppeKodeMap : ClassMap<HovedtypegruppeKode>
    {
        public HovedtypegruppeKodeMap()
        {
            Map(x => x.KodeName).Name("hovetdypegruppeKodenavn");
        }
    }

    public sealed class HovedtypeKodeMap : ClassMap<HovedtypeKode>
    {
        public HovedtypeKodeMap()
        {
            Map(x => x.KodeName).Name("hovedtypeKodenavn");
        }
    }

    public sealed class NinVersionMap : ClassMap<NinVersion>
    {
        public NinVersionMap()
        {
            foreach (var property in typeof(NinVersion).GetProperties())
            {
                switch (property.Name.ToLowerInvariant())
                {
                    case "navn":
                        Map(typeof(NinVersion), property).Name(JsonNamingPolicy.CamelCase.ConvertName($"version{property.Name}"));
                        break;
                    default:
                        continue;
                }
            }
        }
    }
}