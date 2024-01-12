using NiN3.Core.Models.DTOs;
using NiN3.Core.Models;
using NiN3.Core.Models.Enums;
using System.Data;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using NiN3.Core.Models.DTOs.type;
using NiN3.Core.Models.DTOs.variabel;
using System.Collections.Concurrent;
using System;
using System.ComponentModel.DataAnnotations;
using NiN3.Core.Models.DTOs.rapport;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NiN3.Infrastructure.Mapping
{

    /// <summary>
    /// Maps a Versjon object to a VersjonDto object.
    /// </summary>
    /// <param name="versjon">The Versjon object to be mapped.</param>
    /// <returns>A VersjonDto object with the mapped values.</returns>
    /// Usage: 
    /// NiNkodeMapper ninkodeMapper = NiNkodeMapper.Instance;
    /// ninkodeMapper.SetConfiguration(configuration);

    public sealed class NiNkodeMapper
    {
        private IConfiguration _configuration;
        private static readonly NiNkodeMapper instance = new NiNkodeMapper();
        private string _root_url;

        /// <summary>
        /// Gets the singleton instance of the NiNkodeMapper class.
        /// </summary>
        /// <returns>The singleton instance of the NiNkodeMapper class.</returns>
        public static NiNkodeMapper Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Sets the configuration for the application.
        /// </summary>
        /// <param name="configuration">The configuration to set.</param>
        public void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            _root_url = _configuration?["root_url"];
        }



        /// <summary>
        /// Maps a Versjon object to a VersjonDto object.
        /// </summary>
        /// <param name="versjon">The Versjon object to be mapped.</param>
        /// <returns>A VersjonDto object with the mapped data.</returns>



        public VersjonDto Map(Versjon versjon)
        {
            //Create a new VersjonDto object
            var versjonDto = new VersjonDto()
            {
                Navn = versjon.Navn,
            };
            // Create a ConcurrentBag for mapped Typer
            /*if (versjon.Typer != null && versjon.Typer.Any())
            {
                var typerBag = new ConcurrentBag<TypeDto>();
                Parallel.ForEach(versjon.Typer, t => typerBag.Add(Map(t)));
                versjonDto.Typer = typerBag.ToList().OrderBy(t => t.Kode.Id).ToList();
            }*/
            if (versjon.Typer != null && versjon.Typer.Any())
            {
                var typerList = versjon.Typer.AsParallel()
                    .Select(t => Map(t))
                    .OrderBy(t => t.Kode.Id)
                    .ToList();
                versjonDto.Typer = typerList;
            }



            // Create a ConcurrentBag for mapped Variabler
            if (versjon.Variabler != null && versjon.Variabler.Any())
            {
                var variablerBag = new ConcurrentBag<VariabelDto>();
                Parallel.ForEach(versjon.Variabler, v => variablerBag.Add(Map(v)));
                versjonDto.Variabler = variablerBag.ToList().OrderBy(v => v.Kode.Id).ToList();
            }

            //Return the VersjonDto object
            return versjonDto;
        }




        /// <summary>
        /// Maps a NiN3.Core.Models.Type to a TypeDto
        /// </summary>
        /// <param name="type">The NiN3.Core.Models.Type to map</param>
        /// <returns>A TypeDto with the mapped values</returns>
        public TypeDto Map(NiN3.Core.Models.Type type)
        {
            var _typekategori2 = type.Typekategori2 != null ? $"{type.Typekategori2.ToString()}: {EnumUtil.ToDescriptionBlankIfNull(type.Typekategori2)}" : null;
            var typedto = new TypeDto
            {
                //Navn = $"{EnumUtil.ToDescription(type.Ecosystnivaa)} {EnumUtil.ToDescription(type.Typekategori)} {EnumUtil.ToDescriptionBlankIfNull(type.Typekategori2)}",
                Navn = type.Navn,
                Kategori = "Type",
                //Ecosystnivaa = $"{type.Ecosystnivaa.ToString()}: {EnumUtil.ToDescription(type.Ecosystnivaa)}",
                EcosystnivaaEnum = type.Ecosystnivaa,
                EcosystnivaaNavn = EnumUtil.ToDescription(type.Ecosystnivaa),
                TypekategoriEnum = type.Typekategori,
                TypekategoriNavn = EnumUtil.ToDescription(type.Typekategori),
                Typekategori2Enum = type.Typekategori2,
                Typekategori2Navn = EnumUtil.ToDescriptionBlankIfNull(type.Typekategori2),
                Kode = MapKode(type.Kode, type.Langkode)
            };
            //type.Hovedtypegrupper.ToList().ForEach(h => typedto.Hovedtypegrupper.Add(Map(h)));
            var hovedtypegrupperBag = new ConcurrentBag<HovedtypegruppeDto>();
            Parallel.ForEach(type.Hovedtypegrupper.ToList(), g => hovedtypegrupperBag.Add(Map(g)));
            typedto.Hovedtypegrupper = hovedtypegrupperBag.ToList();
            return typedto;
        }


        /// <summary>
        /// Maps a Hovedtypegruppe object to a HovedtypegruppeDto object.
        /// </summary>
        /// <returns>
        /// A HovedtypegruppeDto object.
        /// </returns>
        public HovedtypegruppeDto Map(Hovedtypegruppe hovedtypegruppe)
        {
            var hovedtypegruppedto = new HovedtypegruppeDto()
            {
                Navn = hovedtypegruppe.Navn,
                Kategori = "Hovedtypegruppe",
                Typekategori3Enum = hovedtypegruppe.Typekategori3,
                Typekategori3Navn = hovedtypegruppe.Typekategori3 != null ? EnumUtil.ToDescription(hovedtypegruppe.Typekategori3) : null,
                Kode = MapKode(hovedtypegruppe.Kode, hovedtypegruppe.Langkode)
            };
            // This code uses the Parallel.ForEach method to loop through a list of Hovedtyper and add them to a Hovedtypegruppedto. The Map method is used to map the Hovedtyper to the Hovedtypegruppedto.
            //Parallel.ForEach(hovedtypegruppe.Hovedtyper.ToList(), g => hovedtypegruppedto.Hovedtyper.Add(Map(g)));
            var hovedtyperBag = new ConcurrentBag<HovedtypeDto>();
            Parallel.ForEach(hovedtypegruppe.Hovedtyper.ToList(), g => hovedtyperBag.Add(Map(g)));
            hovedtypegruppedto.Hovedtyper = hovedtyperBag.ToList();
            foreach (var hovedoekosystem in hovedtypegruppe.Hovedoekosystemer)
            {
                hovedtypegruppedto.Hovedoekosystemer.Add(Map(hovedoekosystem));
            }
            foreach (var konvertering in hovedtypegruppe.Konverteringer)
            {
                hovedtypegruppedto.konverteringer.Add(Map(konvertering));
            }
            return hovedtypegruppedto;
        }


        public HovedoekosystemDto Map(Hovedtypegruppe_Hovedoekosystem hovedtypegruppe_Hovedoekosystem)
        {
            var hovedoekosystemdto = new HovedoekosystemDto()
            {
                HovedoekosystemEnum = hovedtypegruppe_Hovedoekosystem.HovedoekosystemEnum,
                Navn = EnumUtil.ToDescription(hovedtypegruppe_Hovedoekosystem.HovedoekosystemEnum),

            };
            return hovedoekosystemdto;
        }

        public KonverteringDto Map(Konvertering konvertering)
        {
            var konverteringDto = new KonverteringDto()
            {
                Kode = konvertering.Kode,
                ForrigeKode = konvertering.ForrigeKode,
                Klasse = konvertering.Klasse,
                KlasseNavn = EnumUtil.ToDescription(konvertering.Klasse),
                ForrigeVersjon = konvertering.ForrigeVersjon.Navn,
                FoelsomhetsPresisjon = konvertering.FoelsomhetsPresisjon,
                Spesifiseringsevne = konvertering.Spesifiseringsevne,
                Url = konvertering.Url
            };
            return konverteringDto;
        }

        /// <summary>
        /// Maps a Hovedtype object to a HovedtypeDto object.
        /// </summary>
        /// <param name="hovedtype">The Hovedtype object to be mapped.</param>
        /// <returns>A HovedtypeDto object with the mapped values.</returns>
        //This method maps a Hovedtype object to a HovedtypeDto object
        public HovedtypeDto Map(Hovedtype hovedtype)
        {
            var hovedtypedto = new HovedtypeDto()
            {
                Navn = hovedtype.Navn,
                Kategori = "Hovedtype",
                Kode = MapKode(hovedtype.Kode, hovedtype.Langkode),
                //Prosedyrekategori = $"{hovedtype.Prosedyrekategori.ToString()}: {EnumUtil.ToDescription(hovedtype.Prosedyrekategori)}",
                ProsedyrekategoriEnum = hovedtype.Prosedyrekategori,
                ProsedyrekategoriNavn = hovedtype.Prosedyrekategori != null ? EnumUtil.ToDescription(hovedtype.Prosedyrekategori) : null,
            };
            var grunntyperBag = new ConcurrentBag<GrunntypeDto>();
            Parallel.ForEach(hovedtype.Grunntyper.ToList(), g => grunntyperBag.Add(Map(g)));
            hovedtypedto.Grunntyper = grunntyperBag.ToList();
            var kartleggingsenheterBag = new ConcurrentBag<KartleggingsenhetDto>();
            Parallel.ForEach(hovedtype.Hovedtype_Kartleggingsenheter.ToList(), g =>
            {
                if (g.Kartleggingsenhet != null)
                {
                    kartleggingsenheterBag.Add(Map(g.Kartleggingsenhet));
                }
                else
                {
                    Console.WriteLine($"Kartleggingsenhet is null for hovedtype {hovedtype.Navn} (, ht_kl-object: {g.Id})");
                }
            });
            if (hovedtype.HovedtypeVariabeltrinn.Any())
            {
                var variabeltrinnBag = new ConcurrentBag<VariabeltrinnDto>();
                var trinnIds = new List<int>();
                foreach (var htvt in hovedtype.HovedtypeVariabeltrinn)
                {
                    if (htvt.Trinn != null)
                    {
                        trinnIds.Add(htvt.Trinn.Id);
                    }
                }
                var variabelnavn = hovedtype.HovedtypeVariabeltrinn.Select(vt => vt.Variabelnavn).ToList();
                Parallel.ForEach(hovedtype.HovedtypeVariabeltrinn.ToList(), vt => variabeltrinnBag.Add(Map(vt, trinnIds)));
                hovedtypedto.Variabeltrinn = variabeltrinnBag.Distinct().ToList();
            }
            hovedtypedto.Kartleggingsenheter = kartleggingsenheterBag.ToList();
            hovedtypedto.Variabeltrinn = hovedtypedto.Variabeltrinn
                .GroupBy(vt => vt.Maaleskala.MaaleskalaNavn)//TODO: Lazy solution to duplicate MaaleskalaDto objects inside VariabeltrinnCollection, please improve
                .Select(group => group.First())
                .ToList();
            foreach (var konv in hovedtype.Konverteringer)
            {
                hovedtypedto.Konverteringer.Add(Map(konv));
            }
            return hovedtypedto;
        }


        /// <returns>A GrunntypeDto object with the mapped values.</returns>
        public GrunntypeDto Map(Grunntype grunntype)
        {
            var grunntypedto = new GrunntypeDto()
            {
                Navn = grunntype.Navn,
                Kategori = "Grunntype",
                Kode = MapKode(grunntype.Kode, grunntype.Langkode)
            };
            //for each maaleskala make a list of maaleskalaDto
            if (grunntype.GrunntypeVariabeltrinn.Any())
            {
                var variabeltrinnBag = new ConcurrentBag<VariabeltrinnDto>();
                var trinnIds = grunntype.GrunntypeVariabeltrinn != null && grunntype.GrunntypeVariabeltrinn.Any(gtvt => gtvt.Trinn != null)
                ? grunntype.GrunntypeVariabeltrinn.Where(gtvt => gtvt.Trinn != null).Select(gtvt => gtvt.Trinn.Id).ToList()
                : new List<int>();
                var variabelnavn = grunntype.GrunntypeVariabeltrinn.Any() ? grunntype.GrunntypeVariabeltrinn.Select(vt => vt.Variabelnavn).ToList() : new List<Variabelnavn?>();
                Parallel.ForEach(grunntype.GrunntypeVariabeltrinn.ToList(), vt => variabeltrinnBag.Add(Map(vt, trinnIds)));
                grunntypedto.Variabeltrinn = variabeltrinnBag.ToList();
                grunntypedto.Variabeltrinn = grunntypedto.Variabeltrinn//TODO: Lazy solution to duplicate MaaleskalaDto objects inside VariabeltrinnCollection, please improve
                    .GroupBy(vt => vt.Maaleskala.MaaleskalaNavn)
                    .Select(group => group.First())

                    .ToList();
                foreach (var konv in grunntype.Konverteringer)
                {
                    grunntypedto.Konverteringer.Add(Map(konv));
                }
            }
            return grunntypedto;
        }

        /*
        //put together new list of variabeltrinn
        foreach (var m in distinctMaaleskalas) { 
          var variabeltrinn = grunntype.GrunntypeVariabeltrinn.FirstOrDefault(vt => vt.Maaleskala.Id == m.Id);
          variabeltrinnList.Add(new Variabeltrinn() { Maaleskala = m, Variabelnavn = variabeltrinn.Variabelnavn });
        }*/


        //for each trinn find maaleskala for trinn and add (map)trinndto

        public VariabeltrinnDto Map(GrunntypeVariabeltrinn gtVariabeltrinn, List<int> registertTrinnIds = null)
        {
            Variabeltrinn variabeltrinn = new Variabeltrinn()
            {
                Maaleskala = gtVariabeltrinn.Maaleskala,
                Variabelnavn = gtVariabeltrinn.Variabelnavn
            };
            return Map(variabeltrinn, registertTrinnIds);
        }

        public VariabeltrinnDto Map(HovedtypeVariabeltrinn htVariabeltrinn, List<int> registertTrinnIds = null)
        {
            Variabeltrinn variabeltrinn = new Variabeltrinn()
            {
                Maaleskala = htVariabeltrinn.Maaleskala,
                Variabelnavn = htVariabeltrinn.Variabelnavn
            };
            return Map(variabeltrinn, registertTrinnIds);
        }


        public VariabeltrinnDto Map(Variabeltrinn variabeltrinn, List<int> registertTrinnIds = null)
        {
            var variabeltrinnDto = new VariabeltrinnDto()
            {
                Maaleskala = Map(variabeltrinn.Maaleskala, registertTrinnIds)
            };
            variabeltrinnDto.Variabelnavn = variabeltrinn.Variabelnavn != null ? Map(variabeltrinn.Variabelnavn) : null;
            return variabeltrinnDto;
        }


        /*
        public TrinnForType MapForType(Trinn trinn) { 
            var trinnForType = new TrinnForType()
            {
                Id = trinn.Id,
                Verdi = trinn.Verdi,
                Beskrivelse = trinn.Beskrivelse,
                //Registrert = trinn.Registrert,
            };
            return trinnForType;
        }*/

        /*
        public TrinnForTypeDto Map(TrinnForType trinnForType)
        { 
            var trinnForTypeDto = new TrinnForTypeDto()
            {
                Verdi = trinnForType.Verdi,
                Beskrivelse = trinnForType.Beskrivelse,
                Registrert = trinnForType.Registrert,
            };
            return trinnForTypeDto;
        }*/

        /*
        /// <summary>
        /// Maps a Grunntype object to a GrunntypeDto object.
        /// </summary>
        /// <param name="grunntype">The Grunntype object to be mapped.</param>
        /// <returns>A GrunntypeDto object with the mapped values.</returns>
        public GrunntypeDto Map(Grunntype grunntype)
        {
            var grunntypedto = new GrunntypeDto()
            {
                Navn = grunntype.Navn,
                Kategori = "Grunntype",
                Kode = MapKode(grunntype.Kode, grunntype.Langkode)
            };
            //for each maaleskala make a list of maaleskalaDto
            if (grunntype.GrunntypeVariabeltrinn.Any())
            {
                
                var maaleskalas = grunntype.GrunntypeVariabeltrinn.Select(g => g.Maaleskala).Distinct().ToList();

            }

            

            //for each trinn find maaleskala for trinn and add (map)trinndto
            return grunntypedto;
        }*/

        /// <summary>
        /// Maps a Kartleggingsenhet object to a KartleggingsenhetDto object.
        /// </summary>
        /// <param name="kartleggingsenhet">The object to map</param>
        /// <returns>A newly created KartleggingsenhetDto object</returns>
        public KartleggingsenhetDto Map(Kartleggingsenhet kartleggingsenhet)
        {
            var kartleggingsenhetdto = new KartleggingsenhetDto()
            {
                Navn = kartleggingsenhet.Navn,
                Kategori = "Kartleggingsenhet",
                //Kode = kartleggingsenhet.Kode,
                //Kortkode = kartleggingsenhet.Kortkode,
                Kode = MapKode(kartleggingsenhet.Kode, kartleggingsenhet.Langkode, false),
                //Maalestokk = $"{kartleggingsenhet.Maalestokk.ToString()}: {EnumUtil.ToDescription(kartleggingsenhet.Maalestokk)}"
                MaalestokkEnum = kartleggingsenhet.Maalestokk,
                MaalestokkNavn = EnumUtil.ToDescription(kartleggingsenhet.Maalestokk)
            };
            var grunntyperBag = new ConcurrentBag<GrunntypeDto>();
            Parallel.ForEach(kartleggingsenhet.Grunntyper.ToList(), g => grunntyperBag.Add(Map(g)));
            kartleggingsenhetdto.Grunntyper = grunntyperBag.ToList();

            return kartleggingsenhetdto;
        }


        /// <summary>
        /// Maps a code to a KodeDto object.
        /// </summary>
        /// <param name="kode">The code to be mapped.</param>
        /// <returns>A KodeDto object containing the code and its definition.</returns>
        public KodeDto MapKode(String kode, String? langkode = null, bool fortyper = true)
        {
            var typeOrVar = fortyper ? "typer" : "variabler";
            //var _root_url = "https://nin-kode-api.artsdatabanken.no/v3.0";
            var kodeDto = new KodeDto()
            {
                Id = kode,
                Definisjon = _root_url + $"/v3.0/{typeOrVar}/{GetEndpointnameByLangkode(langkode)}/" + kode,
            };
            if (langkode != null)
            {
                kodeDto.Langkode = langkode;
            }
            return kodeDto;
        }

        /// <summary>
        /// Maps a <paramref name="variabel"/> to a <see cref="VariabelDto"/> object.
        /// </summary>
        /// <param name="variabel">The <see cref="Variabel"/> object to map.</param>
        /// <returns>A new <see cref="VariabelDto"/> object that represents the mapped <paramref name="variabel"/>.</returns>
        public VariabelDto Map(Variabel variabel)
        {
            var variabelDto = new VariabelDto()
            {
                Kode = MapKode(variabel.Kode, variabel.Langkode, false),
                Navn = variabel.Navn,
                Kategori = "Variabel",
                EcosystnivaaEnum = variabel.Ecosystnivaa,
                EcosystnivaaNavn = EnumUtil.ToDescription(variabel.Ecosystnivaa),
                VariabelkategoriEnum = variabel.Variabelkategori,
                VariabelkategoriNavn = EnumUtil.ToDescription(variabel.Variabelkategori)
            };

            var variabelnavnBag = new ConcurrentBag<VariabelnavnDto>();
            Parallel.ForEach(variabel.Variabelnavn.ToList(), vn => variabelnavnBag.Add(Map(vn)));
            variabelDto.Variabelnavn = variabelnavnBag.ToList();

            return variabelDto;
        }


        // <summary>
        /// Maps a Variabelnavn object to a VariabelnavnDto object.
        /// </summary>
        /// <param name="variabelnavn">The Variabelnavn object to map.</param>
        /// <returns>A VariabelnavnDto object mapping the values of the Variabelnavn parameter.</returns>
        public VariabelnavnDto Map(Variabelnavn variabelnavn)
        {
            var variabelnavnDto = new VariabelnavnDto()
            {
                Navn = variabelnavn.Navn,
                Kode = MapKode(variabelnavn.Kode, variabelnavn.Langkode, false),
                Variabelkategori2Enum = variabelnavn.Variabelkategori2,
                Variabelkategori2Navn = EnumUtil.ToDescription(variabelnavn.Variabelkategori2),
                VariabeltypeEnum = variabelnavn.Variabeltype,
                VariabeltypeNavn = EnumUtil.ToDescription(variabelnavn.Variabeltype),
                VariabelgruppeEnum = variabelnavn.Variabelgruppe,
                VariabelgruppeNavn = EnumUtil.ToDescription(variabelnavn.Variabelgruppe)
            };
            //mapping konvertering
            foreach (var konv in variabelnavn.Konverteringer)
            {
                variabelnavnDto.Konverteringer.Add(Map(konv));
            }
            var maaleskalaBag = new ConcurrentBag<MaaleskalaDto>();
            Parallel.ForEach(variabelnavn.VariabelnavnMaaleskala.ToList(), g => maaleskalaBag.Add(Map(g.Maaleskala)));
            variabelnavnDto.Variabeltrinn = maaleskalaBag.ToList();
            return variabelnavnDto;
        }

        public MaaleskalaDto Map(Maaleskala maaleskala, List<int> registertTrinnIds = null)
        {
            var MaaleskalaDto = new MaaleskalaDto()
            {
                MaaleskalaNavn = maaleskala.MaaleskalaNavn,
                MaaleskalatypeEnum = maaleskala.MaaleskalatypeEnum,
                MaaleskalatypeNavn = EnumUtil.ToDescription(maaleskala.MaaleskalatypeEnum),
                EnhetEnum = maaleskala.EnhetEnum,
                EnhetNavn = EnumUtil.ToDescription(maaleskala.EnhetEnum)
            };
            var trinnBag = new ConcurrentBag<TrinnDto>();
            if (registertTrinnIds == null) { registertTrinnIds = new List<int>(); }
            Parallel.ForEach(maaleskala.Trinn.ToList(), g => trinnBag.Add(Map(g, registertTrinnIds.Contains(g.Id))));
            MaaleskalaDto.Trinn = trinnBag.ToList();
            return MaaleskalaDto;
        }

        public TrinnDto Map(Trinn trinn, bool registert = false)
        {
            //var verdi = null;
            var verdi = trinn.Verdi.Contains("_") ? trinn.Verdi.Split("_")[1] : trinn.Verdi;
            var kode = trinn.Verdi;
            var TrinnDto = new TrinnDto()
            {
                Beskrivelse = trinn.Beskrivelse,
                Verdi = verdi,
                Kode = kode,
                Registert = registert
            };
            return TrinnDto;
        }

        public KlasseDto Map(AlleKortkoder alleKortkoderForType)
        {
            var TypeKlasseDto = new KlasseDto()
            {
                KlasseEnum = alleKortkoderForType.TypeKlasseEnum,
                KlasseNavn = EnumUtil.ToDescription(alleKortkoderForType.TypeKlasseEnum)
            };
            return TypeKlasseDto;
        }

        public KodeoversiktDto Map(Kodeoversikt kodeoversikt)
        {
            var KodeoversiktDto = new KodeoversiktDto()
            {
                Langkode = kodeoversikt.Langkode,
                Kortkode = kodeoversikt.Kortkode,
                Navn = kodeoversikt.Navn,
                Klasse = kodeoversikt.Klasse
            };
            return KodeoversiktDto;
        }



        /// <summary>
        /// Helper method to get the endpoint name for a given langkode.
        /// </summary>
        /// <param name="langkode"></param>
        /// <returns></returns>
        public string GetEndpointnameByLangkode(string langkode)
        {
            var endpointname = "unknown";
            ArrayList langkodeList = new ArrayList(langkode.Split('-'));
            var trinncount = langkodeList.Count;
            var langkodeType = langkodeList[2];
            if (langkodeList[2].ToString() == "T")// langkode is from Type tree 
            {
                if (trinncount == 6) { endpointname = "kodeforType"; }
                else if (trinncount == 8) { endpointname = "kodeforHovedtypegruppe"; }
                else if (trinncount == 11) { endpointname = "kodeforGrunntype"; }
                else if (trinncount == 9 && Regex.IsMatch(langkodeList[8].ToString(), @"^[a-zA-Z]+$")) { endpointname = "kodeforHovedtypegruppe"; }
                else if (trinncount == 10 && langkodeList[8].ToString().StartsWith("M0")) { endpointname = "kodeforKartleggingsenhet"; }
                else if (trinncount == 9 && Regex.IsMatch(langkodeList[8].ToString(), @"^0|[1-9]\d*$")) { endpointname = "kodeforHovedtype"; }
                else if (trinncount == 10 && (langkodeList[6].ToString() == "MB" || langkodeList[6].ToString() == "VM")) { endpointname = "kodeforHovedtype"; }
                else if (trinncount == 10 && (langkodeList[4].ToString() == "LV" || langkodeList[4].ToString() == "PE")) { endpointname = "kodeforGrunntype"; }
                else if (trinncount == 10 && (langkodeList[4].ToString() != "LV" || langkodeList[4].ToString() != "PE")) { endpointname = "kodeforHovedtype"; }
            }
            else
            {// langkode is from Variabel tree 
                endpointname = "kodeForVariabelnavn";
                if (trinncount == 5)
                {
                    endpointname = "kodeforVariabel";
                }
            }
            return endpointname;
        }
    }
}
