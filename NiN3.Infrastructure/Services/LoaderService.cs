using NiN3.Core.Models;
using NiN3.Infrastructure.in_data;
//using Type = NiN3.Core.Models.Type;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NiN3KodeAPI.in_data;
using NiN3.Infrastructure.Services;
using NiN3.Infrastructure.DbContexts;
using NiN3.Core.Models.Enums;
using System.Text;
//using NiN3.Infrastructure.in_data.csvfiles;

namespace NiN.Infrastructure.Services
{
    public class LoaderService : ILoaderService
    {
        private readonly ILogger<LoaderService> _logger;
        private readonly NiN3DbContext _context;
        private IConfiguration _conf;
        //public List<CsvdataImporter_htg_ht_gt_mapping> csvdataImporter_Htg_Ht_Gt_Mappings { get; set; }//TODO: remove this property after new mapper for HT<>GT
        public List<CsvdataImporter_hovedtypegruppe_hovedtype_mapping> csvdataImporter_Hovedtypegruppe_Hovedtype_Mappings { get; set; }
        public List<CsvdataImporter_hovedtype_grunntype_mapping> csvdataImporter_Hovedtype_Grunntype_Mappings { get; set; }   
        public List<CsvdataImporter_Type_Htg_mapping> csvdataImporter_Type_Htg_Mappings { get; set; }
        string logpath = @"C:\temp\nin3LoaderLogg_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
        public List<NiN3.Core.Models.Type> _typer { get; set; }

        private Versjon? _versjon;

        private Versjon Versjon
        {
            get
            {
                if (_versjon != null) return _versjon;
                _versjon = _context.Versjon.FirstOrDefault(s => s.Navn == "3.0"); // denne kan og gjøres konfigurerbar - var en kommentar om det lengre nede
                return _versjon;
            }
        }

        //private List<CsvDataImporter_typeklasser_langkode> Langkoder_typeklasser;
        private readonly Dictionary<string, dynamic> EntitiesTypeDict = new Dictionary<string, dynamic>();

        public LoaderService(IConfiguration configuration, NiN3DbContext context, ILogger<LoaderService> logger)
        {
            _context = context;
            _logger = logger;
            _conf = configuration;
            LoadTypeklasser_langkoder();
        }

        private bool HasTable(string tableName)
        {
            var tableNames = _context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();
            return tableNames.Contains(tableName);
        }
        public List<string?> Tabeller()
        {
            var tableNames = _context.Model.GetEntityTypes()
                            .Select(t => t.GetTableName())
                            .Distinct()
                            .ToList();
            return tableNames;
        }


        public IEnumerable<Versjon> HentDomener()
        {
            return _context.Versjon.OrderBy(c => c.Navn).ToList();
        }


        //public bool OpprettInitDbAsync()
        public bool OpprettInitDb()
        {
            LoadType_HTG_Mappings();
            LoadHtg_Ht_Gt_Mappings();
            try
            {
                LoadTypeData();
                _logger.LogInformation("Import of Types. Done");
                LoadHovedtypeGruppeData();
                _logger.LogInformation("Import of HTGdata. Done");
                LoadHovedtypeData();
                _logger.LogInformation("Import of HTdata. Done");

                LoadGrunntypedata();
                _logger.LogInformation("Import of GTdata. Done");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error importing data from csv ; " + ex.Message);
            }
            return true;
        }


        public void load_all_data()
        {

            SeedLookupData();
            LoadTypeData();
            LoadType_HTG_Mappings();
            LoadHovedtypeGruppeData();
            LoadHovedtypegruppeHovedoekosystemer();
            LoadHtg_Ht_Gt_Mappings();
            LoadHovedtypeData();
            LoadGrunntypedata();
            LoadKartleggingsenhet_m005();
            LoadKartleggingsenhet_M005_Grunntype();
            LoadKartleggingsenhet_M005_hovedtype();
            LoadKartleggingsenhet_m020();
            LoadKartleggingsenhet_M020_Grunntype();
            LoadKartleggingsenhet_M020_hovedtype();
            LoadKartleggingsenhet_M050();
            LoadKartleggingsenhet_M050_Grunntype();
            LoadKartleggingsenhet_M050_Hovedtype();
            LoadVariabel();
            LoadVariabelnavn();
            LoadMaaleskala();
            LoadTrinn();
            MakeMaaleskalaMappingForVariabelnavn();

            LoadGrunntypeVariabeltrinnMapping();
            LoadHovedtypeVariabeltrinnMapping();
            LoadKonverteringHovedtypegruppe();
            LoadKonverteringHovedtype();
            LoadKonverteringGrunntype();
            LoadKonverteringVariabelnavn();

            // ** Reports ** //
            LoadAlleKortkoder();
            LoadEnumoppslag();
            _context.SaveChanges();

            // ** Views, for overview/insight ** //
            CreateMaaleskalaView();
            CreateGrunntypeVariabeltrinnView();
            CreateHovedtypeVariabeltrinnView();
            CreateSjekkUnikeHovedklasserView();
            CreateHovedtypeKleView();
            CreateAlleLangkoderView();
            CreateDuplikateLangkoderView();
            CreateDBInfoTable();
            SetBuildTimeInDb_infoTable();

            WriteToFile($"END: loading is FINISHED :) (Log is written to {logpath})");
        }

        // Rewritten code with comments

        /// <summary>
        /// Loads the Type_HTG_Mapping from the csv file and logs a message to indicate that the Type_HTG_Mapping has been loaded.
        /// </summary>
        public void LoadType_HTG_Mappings()
        {
            // Create an instance of the CsvdataImporter_Type_Htg_mapping class
            csvdataImporter_Type_Htg_Mappings = CsvdataImporter_Type_Htg_mapping.ProcessCSV("in_data/csvfiles/type_htg_mapping.csv");
            // Log a message to indicate that the Type_HTG_Mapping has been loaded
            //_logger.LogInformation("Type_HTG_Mapping lastet");
        }

        /// <summary>
        /// Creates an instance of the CsvdataImporter_htg_ht_gt_mapping class and logs a message to indicate that the Htg_Ht_Gt_Mapping has been loaded.
        /// </summary>
        public void LoadHtg_Ht_Gt_Mappings()
        {
            // Create an instance of the CsvdataImporter_htg_ht_gt_mapping class
            //csvdataImporter_Htg_Ht_Gt_Mappings = CsvdataImporter_htg_ht_gt_mapping.ProcessCSV("in_data/htg_ht_gt_mapping.csv");
            csvdataImporter_Hovedtypegruppe_Hovedtype_Mappings = CsvdataImporter_hovedtypegruppe_hovedtype_mapping.ProcessCSV("in_data/csvfiles/hovedtypegruppe_hovedtype_mapping.csv");
            csvdataImporter_Hovedtype_Grunntype_Mappings = CsvdataImporter_hovedtype_grunntype_mapping.ProcessCSV("in_data/csvfiles/hovedtype_grunntype_mapping.csv");
            // Log a message to indicate that the Htg_Ht_Gt_Mapping has been loaded
            //_logger.LogInformation("Htg_Ht_Gt_Mapping lastet");
        }


        /// <summary>
        /// Seeds the lookup data.
        /// </summary>
        public void SeedLookupData()
        {
            //List<Versjon> domenes = new List<Versjon>(); //TODO: delete if all tests ok
            _context.Add(new Versjon() { Navn = "3.0" }); // current version
            _context.Add(new Versjon() { Navn = "2.3" });  // for 'konvertering' purposeshttps://open.spotify.com/playlist/46twe0c8AJIV3SKxBxqwxd
            _context.SaveChanges();
        }


        public void LoadTypeData()
        {
            WriteToFile("\n\n********  LoadTypeData");
            var tbls = Tabeller();
            if (_context.Type.Count() == 0)
            {
                var typer = CsvdataImporter_Type.ProcessCSV("in_data/csvfiles/type.csv");
                foreach (var type in typer)
                {
                    //var langkodeForType 
                    var t = new NiN3.Core.Models.Type()
                    {
                        Navn = type.Typekategori2!=null?EnumUtil.ToDescriptionBlankIfNull(type.Typekategori2): EnumUtil.ToDescriptionBlankIfNull(type.Typekategori),
                        Kode = type.Kode,
                        //Langkode = langkodeForType,
                        Ecosystnivaa = type.Ecosystnivaa,
                        Typekategori = type.Typekategori,
                        Typekategori2 = type.Typekategori2,
                        Versjon = Versjon
                    };
                    t.Langkode = LangkodeForTypeObject(TypeklasseTypeEnum.T, type.Kode, t);
                    _context.Add(t);
                }
                _context.SaveChanges();
            }
            else
            {
                WriteToFile("Objecttype <<Type>> allready has data!");
            }
        }


        /// <summary>
        /// Loads data of hovedtypegruppe from CSV file to database, if data not already present.
        /// </summary>
        public void LoadHovedtypeGruppeData()
        {
            WriteToFile("\n\n********  LoadHovedtypeGruppeData");
            //q: fetch typer from _context
            var typer = _context.Type.ToList();
            var htg_count = _context.Hovedtypegruppe.Count();
            if (_context.Hovedtypegruppe.Count() == 0)
            {
                var hovedtypegrupper = CsvdataImporter_Hovedtypegruppe.ProcessCSV("in_data/csvfiles/hovedtypegrupper.csv");
                var domene = Versjon;// todo-sat: get this from config or even better, get from request parameter -value.
                foreach (var htg in hovedtypegrupper)
                {
                    var typeKode = csvdataImporter_Type_Htg_Mappings.Where(x => x.Hovedtypegruppe_kode == htg.Kode).Select(x => x.Type_kode).FirstOrDefault();
                    var type = typer.FirstOrDefault(s => s.Kode == typeKode);
                    //var langkode_grunntype = Langkoder_typeklasser.FirstOrDefault(s => s.kode_hovedtypegruppe == htg.Kode).langkode;
                    var hovedtg = new Hovedtypegruppe()
                    {
                        Kode = htg.Kode,
                        Typekategori2 = htg.Typekategori2,
                        Typekategori3 = htg.Typekategori3,
                        Versjon = domene,
                        Delkode = htg.Hovedtypegruppe,
                        Navn = htg.Hovedtypegruppenavn,
                        Type = type       /// <summary>
                    };
                    //Setting Langkode here  
                    hovedtg.Langkode = LangkodeForTypeObject(TypeklasseTypeEnum.HTG, htg.Kode, hovedtg.Type, hovedtg);
                    _context.Add(hovedtg);
                }
                _context.SaveChanges();
            }
            else
            {
                WriteToFile("Objecttype <<Hovedtypegruppe>> allready has data!");
            }
        }

        /// <summary>
        /// Loads the data for Hovedtype if there is no data available in the context.
        /// </summary>
        public void LoadHovedtypeData()
        {
            WriteToFile("\n\n********  LoadHovedtypeData");
            if (_context.Hovedtype.Count() == 0)
            {
                var hovedtyper = CsvdataImporter_Hovedtype.ProcessCSV("in_data/csvfiles/hovedtype.csv");
                var domene = Versjon;// todo-sat: get this from config or even better, get from request parameter -value.
                foreach (var ht in hovedtyper)
                {
                    var psk = ht.Prosedyrekategori;
                    var htg_ht = csvdataImporter_Hovedtypegruppe_Hovedtype_Mappings.FirstOrDefault(s => s.Hovedtype_kode == ht.Kode); // finn hovedtypegruppe koden gitt hovedtypekode fra mapping HTG<>HT
                    var hovedtypegruppe = _context.Hovedtypegruppe.FirstOrDefault(s => s.Kode == htg_ht.Hovedtypegruppe_kode);
                    //fetching type-parent for hovedtype via hovedtypegruppe
                    NiN3.Core.Models.Type? type = _context.Type.FirstOrDefault(s => s.Kode == hovedtypegruppe.Type.Kode);
                    //var langkode_grunntype = Langkoder_typeklasser.FirstOrDefault(s => s.kode_hovedtype == ht.Kode);
                    //var langkodeForType = 
                    var hovedtype = new Hovedtype()
                    {
                        //Id = Guid.NewGuid(),
                        Kode = ht.Kode,
                        //Langkode = langkodeForType,
                        Hovedtypegruppe = hovedtypegruppe,
                        Versjon = domene,
                        Delkode = ht.Hovedtype,
                        Navn = ht.Hovedtypenavn,
                        Prosedyrekategori = ht.Prosedyrekategori
                    };
                    hovedtype.Langkode = LangkodeForTypeObject(TypeklasseTypeEnum.HT, ht.Kode, type, hovedtype);
                    _context.Add(hovedtype);
                }
                _context.SaveChanges();
            }
            else
            {
                WriteToFile("Objecttype <<Hovedtype>> allready has data!");
            }
        }

        /// <summary>
        /// Loads data for the Grunntype entity type. 
        /// It imports Grunntype data if the table is empty and saves it to the database.
        /// </summary>
        public void LoadGrunntypedata()
        {
            WriteToFile("\n\n********  LoadGrunntypedata");
            if (_context.Grunntype.Count() == 0)
            {
                //todo-sat: do impl. 
                var grunntyper = CsvdataImporter_Grunntype.ProcessCSV("in_data/csvfiles/grunntyper.csv");
                var domene = Versjon;// todo-sat: get this from config or even better, get from request parameter -value.
                foreach (var gt in grunntyper)
                {

                    var ht_gt = csvdataImporter_Hovedtype_Grunntype_Mappings.FirstOrDefault(s => s.Grunntype_kode == gt.Kode); 
                    var hovedtype = ht_gt!=null?_context.Hovedtype.FirstOrDefault(s => s.Kode == ht_gt.Hovedtype_kode):null;                    
                    var hovedtypegruppe = hovedtype!=null?_context.Hovedtypegruppe.FirstOrDefault(s => s.Kode == hovedtype.Hovedtypegruppe.Kode):null;
                    //var hovedtypegruppe = _context.hovedtypegruppe.FirstOrDefault(s => s.Kode == htg_ht.Hovedtypegruppe_kode);//htg_ht.Hove
                    if (ht_gt != null && hovedtype != null && hovedtypegruppe != null)
                    {
                        var grunntype = new Grunntype()
                        {
                            /* Id = Guid.NewGuid(), */
                            Kode = gt.Kode,
                            //Langkode = gt.Langkode,
                            Navn = gt.Grunntypenavn,
                            Versjon = domene,
                            Delkode = gt.Grunntype,
                            //Hovedtypegruppe = hovedtypegruppe,
                            Hovedtype = hovedtype,
                            Prosedyrekategori = gt.Prosedyrekategori
                        };
                        //bygger riktig langkode for grunntype
                        NiN3.Core.Models.Type? type = _context.Type.FirstOrDefault(s => s.Kode == hovedtypegruppe.Type.Kode);
                        grunntype.Langkode = LangkodeForTypeObject(TypeklasseTypeEnum.GT, grunntype.Kode, type, grunntype);
                        _context.Add(grunntype);
                    }
                    else {
                        var htkode = hovedtype != null ? hovedtype.Kode : "";
                        var htgkode = hovedtypegruppe != null ? hovedtypegruppe.Kode : "";
                        WriteToFile($"Grunntype: Relations missing; grunntype:{gt.Kode}, hovedtype:{htkode}, hovedtypegruppe: {htgkode}, grunntype not added!");
                    }
                }
                _context.SaveChanges();
            }
            else
            {
                WriteToFile("Objecttype <<Grunntype>> allready has data!");
            }
        }

        /// <summary>
        /// Loads data for Type object if not already present in the database.
        /// </summary>
        private void LoadTypeklasser_langkoder()
        {
            WriteToFile("\n\n********  LoadTypeklasser_langkoder");
            //Langkoder_typeklasser = CsvDataImporter_typeklasser_langkode.ProcessCSV("in_data/csvfiles/typeklasser_langkode_mapping.csv");
        }


        public void LoadKartleggingsenhet_m005()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_m005");
            var m005list = CsvdataImporter_m005.ProcessCSV("in_data/csvfiles/m005.csv");
            // If M005 table is empty
            if (_context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M005).Count() == 0)
            {
                foreach(var m005 in m005list){
                    var k = new NiN3.Core.Models.Kartleggingsenhet()
                    {
                        Langkode = m005.Kode,
                        Navn = m005.Navn,
                        Kode = m005.Kortkode,
                        Maalestokk = NiN3.Core.Models.Enums.MaalestokkEnum.M005,
                        Versjon = this.Versjon
                    };
                    _context.Add(k);
                }
                _context.SaveChanges();  
            }
        }

        public void LoadKartleggingsenhet_M005_Grunntype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M005_Grunntype");
            var m005list = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M005).ToList();
            var gtList = _context.Grunntype.ToList();
            var m005_gtList = CsvdataImporter_m005_grunntype_mapping.ProcessCSV("in_data/csvfiles/m005_grunntype_mapping.csv");
            foreach (var kl_gt in m005_gtList) {
                var gt = gtList.FirstOrDefault(g => g.Kode == kl_gt.Grunntype_kode);
                var KLE_m005 = m005list.FirstOrDefault(m005 => m005.Langkode == kl_gt.m005kode);
                if (gt != null && KLE_m005 != null)
                {
                    var kl_gt_obj = new Kartleggingsenhet_Grunntype()
                    {
                        Kartleggingsenhet = KLE_m005,
                        Grunntype = gt,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_gt_obj);
                }
                else {
                    WriteToFile($"Cant find both GT:'{kl_gt.Grunntype_kode}' and KLE: {kl_gt.m005kode}");
                }
                //Find M005
            }
            _context.SaveChanges();
        }

        public void LoadKartleggingsenhet_M005_hovedtype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M005_hovedtype");
            //Getting csv with unique combinations of Hovedtype.kortkode and m050.Langkode
            var m005_gtList = CsvdataImporter_m005_hovedtype_mapping.ProcessCSV("in_data/csvfiles/m005_hovedtype_mapping.csv");
            var hovedtypeList = _context.Hovedtype.ToList();
            var m005List = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M005).ToList();
            //Get a unique list of Hovedtype ids based on 
            foreach (var kl_ht in m005_gtList)
            {
                //get m050
                var m005 = m005List.FirstOrDefault(m005 => m005.Langkode == kl_ht.m005kode);
                //get gt.hovedtype
                var ht = hovedtypeList.FirstOrDefault(ht => ht.Kode == kl_ht.Hovedtype_kode);
                if (ht != null && m005 != null)
                {
                    var kl_ht_obj = new Hovedtype_Kartleggingsenhet()
                    {
                        Kartleggingsenhet = m005,
                        Hovedtype = ht,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_ht_obj);
                }
                else
                {
                    WriteToFile($"Cant find both HT:'{kl_ht.Hovedtype_kode}'[{ht?.Navn}] and KLE(M005): {kl_ht.m005kode}");
                }
            }
            _context.SaveChanges();
        }

        public void LoadKartleggingsenhet_m020()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_m020");
            var m020list = CsvdataImporter_m020.ProcessCSV("in_data/csvfiles/m020.csv");
            // If M005 table is empty
            if (_context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M020).Count() == 0)
            {
                foreach (var m020 in m020list)
                {
                    var k = new NiN3.Core.Models.Kartleggingsenhet()
                    {
                        Langkode = m020.Kode,
                        Navn = m020.Navn,
                        Kode = m020.Kortkode,
                        Maalestokk = NiN3.Core.Models.Enums.MaalestokkEnum.M020,
                        Versjon = this.Versjon
                    };
                    _context.Add(k);
                }
                _context.SaveChanges();
            }
        }


        public void LoadKartleggingsenhet_M020_Grunntype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M020_Grunntype");
            var m020list = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M020).ToList();
            var gtList = _context.Grunntype.ToList();
            var m020_gtList = CsvdataImporter_m020_grunntype_mapping.ProcessCSV("in_data/csvfiles/m020_grunntype_mapping.csv");
            foreach (var kl_gt in m020_gtList)
            {
                var gt = gtList.FirstOrDefault(g => g.Kode == kl_gt.Grunntype_kode);
                var KLE_m020 = m020list.FirstOrDefault(m020 => m020.Langkode == kl_gt.m020kode);
                if (gt != null && KLE_m020 != null)
                {
                    var kl_gt_obj = new Kartleggingsenhet_Grunntype()
                    {
                        Kartleggingsenhet = KLE_m020,
                        Grunntype = gt,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_gt_obj);
                }
                else
                {
                    WriteToFile($"Cant find both GT:'{kl_gt.Grunntype_kode}' and KLE(M020): {kl_gt.m020kode}");
                }
            }
            _context.SaveChanges();
        }

        public void LoadKartleggingsenhet_M020_hovedtype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M020_hovedtype");
            //Getting csv with unique combinations of Hovedtype.kortkode and m050.Langkode
            var m020_gtList = CsvDataImporter_m020_hovedtype_mapping.ProcessCSV("in_data/csvfiles/m020_hovedtype_mapping.csv");
            var hovedtypeList = _context.Hovedtype.ToList();
            var m020List = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M020).ToList();
            //Get a unique list of Hovedtype ids based on 
            foreach (var kl_ht in m020_gtList)
            {
                //get m050
                var m020 = m020List.FirstOrDefault(m020 => m020.Langkode == kl_ht.m020kode);
                //get gt.hovedtype
                var ht = hovedtypeList.FirstOrDefault(ht => ht.Kode == kl_ht.Hovedtype_kode);
                if (ht != null && m020 != null)
                {
                    var kl_ht_obj = new Hovedtype_Kartleggingsenhet()
                    {
                        Kartleggingsenhet = m020,
                        Hovedtype = ht,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_ht_obj);
                }
                else
                {
                    WriteToFile($"Cant find both HT:'{kl_ht.Hovedtype_kode}'[{ht?.Navn}] and KLE (M020): {kl_ht.m020kode}");
                }
            }
            _context.SaveChanges();
        }

        public void LoadKartleggingsenhet_M050()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M050");
            var m050list = CsvdataImporter_m050.ProcessCSV("in_data/csvfiles/m050.csv");
            if (_context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M050).Count() == 0)
            {
                foreach (var m050 in m050list)
                {
                    var k = new NiN3.Core.Models.Kartleggingsenhet()
                    {
                        Langkode = m050.Kode,
                        Navn = m050.Navn,
                        Kode = m050.Kortkode,
                        Maalestokk = NiN3.Core.Models.Enums.MaalestokkEnum.M050,
                        Versjon = this.Versjon
                    };
                    _context.Add(k);
                }
                _context.SaveChanges();
            }
        }


        public void LoadKartleggingsenhet_M050_Grunntype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M050_Grunntype");
            var m050list = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M050).ToList();
            var gtList = _context.Grunntype.ToList();
            var m050_gtList = CsvdataImporter_m050_grunntype_mapping.ProcessCSV("in_data/csvfiles/m050_grunntype_mapping.csv");
            foreach (var kl_gt in m050_gtList)
            {
                var gt = gtList.FirstOrDefault(g => g.Kode == kl_gt.Grunntype_kode);
                var KLE_m050 = m050list.FirstOrDefault(m050 => m050.Langkode == kl_gt.m050kode);
                if (gt != null && KLE_m050 != null)
                {
                    var kl_gt_obj = new Kartleggingsenhet_Grunntype()
                    {
                        Kartleggingsenhet = KLE_m050,
                        Grunntype = gt,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_gt_obj);
                }
                else
                {
                    WriteToFile($"Cant find both GT:'{kl_gt.Grunntype_kode}' and KLE (M050): {kl_gt.m050kode}");
                }
            }
            _context.SaveChanges();
        }

        public void LoadKartleggingsenhet_M050_Hovedtype()
        {
            WriteToFile("\n\n********  LoadKartleggingsenhet_M050_Hovedtype");
            //Getting csv with unique combinations of Hovedtype.kortkode and m050.Langkode
            var m050_gtList = CsvdataImporter_m050_hovedtype_mapping.ProcessCSV("in_data/csvfiles/m050_hovedtype_mapping.csv");
            var hovedtypeList = _context.Hovedtype.ToList();
            var m050List = _context.Kartleggingsenhet.Where(k => k.Maalestokk == NiN3.Core.Models.Enums.MaalestokkEnum.M050).ToList();
            //Get a unique list of Hovedtype ids based on 
            foreach (var kl_ht in m050_gtList)
            {
                //get m050
                var m050 = m050List.FirstOrDefault(m050 => m050.Langkode == kl_ht.m050kode);
                //get gt.hovedtype
                var ht = hovedtypeList.FirstOrDefault(ht => ht.Kode == kl_ht.Hovedtype_kode);
                if (ht != null && m050 != null)
                {
                    var kl_ht_obj = new Hovedtype_Kartleggingsenhet()
                    {
                        Kartleggingsenhet = m050,
                        Hovedtype = ht,
                        Versjon = this.Versjon
                    };
                    _context.Add(kl_ht_obj);
                }
                else
                {
                    WriteToFile($"Cant find both HT:'{kl_ht.Hovedtype_kode}'[{ht?.Navn}] and KLE (M050): {kl_ht.m050kode}");
                }
            }
            _context.SaveChanges();
        }


        /// <summary>
        /// Loads hovedtypegruppe-hovedoekosystem data into the database.
        /// 
        /// H = Hav
        /// F = Ferskvann
        /// L = Land
        /// </summary>
        public void LoadHovedtypegruppeHovedoekosystemer()
        {
            WriteToFile("\n\n********  LoadHovedtypegruppeHovedoekosystemer");
            //Setting Hovedoekosystem for hovedtypegrupper that is part of the Natursystem and Livsmedium type (skip other hovedtypegrupper)
            var hovedtypegrupper = _context.Hovedtypegruppe
                .Where(h => h.Type.Navn.Contains("Natursystem") || h.Type.Navn.Contains("Livsmedium"))
                .ToList();
            //loop list and fetch oekosystekode
            Dictionary<string, string> HTG_DelkodeOekosyskodeMap = new Dictionary<string, string>()
            {
                {"M", "H"},
                {"L", "F"},
                {"O", "F"},
                {"T", "L"},
                {"V", "L"},
                {"I", "HL"},
                {"S", "H"},
                {"F", "F"},
                {"U", "F"},
                {"MS", "H"},
                {"MU", "H"},
                {"FS", "F"},
                {"FU", "F"},
                {"TS", "L"},
                {"TU", "L"},
            };

            foreach (var hovedtypegruppe in hovedtypegrupper)
            {
                var hovedtypegruppeDelimiter = hovedtypegruppe?.Delkode;
                if (HTG_DelkodeOekosyskodeMap.TryGetValue(hovedtypegruppeDelimiter, out var oekosystemKode))
                {
                    /*if (hovedtypegruppeDelimiter == 'I')
                    {
                        Console.WriteLine("I");
                    }*/
                    var oekoCollection = oekosystemKode.Select(c => c.ToString()).ToList(); //splitting letters into collection of one-letter strings
                    foreach (var oeko in oekoCollection)
                    {
                        var htg_hovedoekosystem = new Hovedtypegruppe_Hovedoekosystem()
                        {
                            Hovedtypegruppe = hovedtypegruppe,
                            HovedoekosystemEnum = EnumUtil.ParseEnum<HovedoekosystemEnum>(oeko)
                        };
                        _context.Add(htg_hovedoekosystem);
                    }
                }
            }
            _context.SaveChanges();
        }

        //<summary>
        //1.TypeklasseTypeEnum typeklasseType - this is an enumeration value that determines the type of parent. It can be one of the three possible values: T, HTG, or HT.
        //2.string kortkode - this parameter is a string that represents the kortkode value of the parent.
        //3.NiN3.Core.Models.Type typeForObject - this is an optional parameter of type NiN3.Core.Models.Type.It represents the Type object associated with the parent.
        //4.object typeobject - this is another optional parameter that can represent the parent object if it is available.
        //</summary>
        public string LangkodeForTypeObject(TypeklasseTypeEnum typeklasseType, string kortkode, NiN3.Core.Models.Type typeForObject = null, object typeobject = null)
        {
            // switch case to check type of parent and return corresponding langkode
            var initKodeArray = new List<string> { "NIN", "3.0", "T" }; //kodeledd 1, 2, 3
            //return string.Join("-", initKodeArray);//TODO: remove after debugging!
            /*
            if (typeForObject.Typekategori.ToString() == "LI") { 
                Console.WriteLine("LI");
            }*/
            switch (typeklasseType)
            {
                case TypeklasseTypeEnum.T:
                    //if (kodeledd_list.Length == 1)//join correct kodeledd from kodeledd_list and return
                    //return string.Join("-", initKodeArray) + typeForObject.Typekategori.ToString() + "-" + kortkode;
                    var sb = new StringBuilder();
                    sb.Append(string.Join("-", initKodeArray));
                    sb.Append("-");
                    sb.Append(kortkode);
                    return sb.ToString();
                //return string.Join("-", initKodeArray) + "-" + kortkode;
                case TypeklasseTypeEnum.HTG:
                    //if (kodeledd_list.Length == 1)//join correct kodeledd from kodeledd_list and return
                    var htg = typeobject as Hovedtypegruppe;
                    var kodeArray = new List<string> { };
                    kodeArray.Add(typeForObject.Ecosystnivaa.ToString());//kodeledd 4
                    kodeArray.Add(typeForObject.Typekategori.ToString());//kodeledd 5
                    kodeArray.Add(htg.Typekategori2 != null ? htg.Typekategori2.ToString() : "0");//kodeledd 6 , sett ledd 6 ="0" hvis typekategori2 er null
                    kodeArray.Add(htg.Typekategori3 != null ? htg.Typekategori3.ToString() : "0"); //kodeledd 7, sett ledd 7 = "0" hvis typekategori3 er null
                    /*                                            //embed of typekategori3 in Langkode bedre kortkode shall only happen on typekategori2= NA
                    if (htg.Typekategori2.Equals(Typekategori2Enum.NA))
                    {
                        if (typeForObject.Ecosystnivaa.Equals(EcosystnivaaEnum.C)
                            && ((htg.Typekategori3.ToString().Equals("VM") || htg.Typekategori3.ToString().Equals("MB"))))
                        {
                            kodeArray.Add(htg.Typekategori3.ToString()); //kodeledd 7
                        }
                    }*/
                    kodeArray.Add(htg.Kode);//kodeledd 8
                    var mergedKodeArrayHTG = initKodeArray.Concat(kodeArray).ToList();
                    return string.Join("-", mergedKodeArrayHTG);
                case TypeklasseTypeEnum.HT:
                    //if (kodeledd_list.Length == 1)//join correct kodeledd from kodeledd_list and return
                    var ht = typeobject as Hovedtype;
                    var typekategori2ForHovedtype = ht.Hovedtypegruppe.Typekategori2;
                    var typekategori3ForHovedtype = ht.Hovedtypegruppe.Typekategori3;
                    var kodeArrayForHT = new List<string> { "NIN", "3.0", "T" };
                    kodeArrayForHT.Add(typeForObject.Ecosystnivaa.ToString()); //kodeledd 4
                    kodeArrayForHT.Add(typeForObject.Typekategori.ToString()); //kodeledd 5
                    kodeArrayForHT.Add(typekategori2ForHovedtype != null ? typekategori2ForHovedtype.ToString() : "0");  //kodeledd 6
                    kodeArrayForHT.Add(typekategori3ForHovedtype != null ? typekategori3ForHovedtype.ToString() : "0");
                    /*                                                         //embed of typekategori3 in Langkode bedre kortkode shall only happen on typekategori2= NA
                    if (typekategori2ForHovedtype.Equals(Typekategori2Enum.NA))
                    {
                        if (typeForObject.Ecosystnivaa.Equals(EcosystnivaaEnum.C)
                            && (ht.Hovedtypegruppe.Typekategori3.ToString().Equals("VM") || ht.Hovedtypegruppe.Typekategori3.ToString().Equals("MB")))
                        {
                            kodeArrayForHT.Add(ht.Hovedtypegruppe.Typekategori3.ToString()); //kodeledd 7
                        }
                    }*/
                    //get ht.Kode but remove first character
                    var htKodeForLangkode = ht.Kode.Substring(1);
                    kodeArrayForHT.Add(htKodeForLangkode);//kodeledd 8(++)
                    return string.Join("-", kodeArrayForHT);
                // add additional cases for other types
                case TypeklasseTypeEnum.GT:
                    //if (kodeledd_list.Length == 1)//join correct kodeledd from kodeledd_list and return
                    var gt = typeobject as Grunntype;
                    var typekategori2ForGrunntype = gt.Hovedtype.Hovedtypegruppe.Typekategori2;
                    var typekategori3ForGrunntype = gt.Hovedtype.Hovedtypegruppe.Typekategori3;
                    var kodeArrayForGT = new List<string> { "NIN", "3.0", "T" };
                    kodeArrayForGT.Add(typeForObject.Ecosystnivaa.ToString()); //kodeledd 4
                    kodeArrayForGT.Add(typeForObject.Typekategori.ToString()); //kodeledd 5
                    kodeArrayForGT.Add(typekategori2ForGrunntype != null ? typekategori2ForGrunntype.ToString() : "0");  //kodeledd 6
                    kodeArrayForGT.Add(typekategori3ForGrunntype != null ? typekategori3ForGrunntype.ToString() : "0"); //kodeledd 7
                    kodeArrayForGT.Add(gt.Kode);//kodeledd 8(++)
                    return string.Join("-", kodeArrayForGT);
                default:
                    throw new ArgumentException("Invalid parent type!");
            }
        }

        public string LangkodeForVariabelType(VariabelklasseTypeEnum variabelklasseType, NiN3.Core.Models.Variabel VariabelForObject = null, object variabelobject = null)
        {
            var initKodeArray = new List<string> { "NIN", "3.0", "V" };
            switch (variabelklasseType)
            {
                case VariabelklasseTypeEnum.V:
                    var sb = new StringBuilder();
                    sb.Append(string.Join("-", initKodeArray)); // kodeledd 4
                    sb.Append("-");
                    sb.Append(VariabelForObject.Kode); // kodeledd 5 ++
                    return sb.ToString();
                default:
                    throw new ArgumentException("Invalid parent type!");
            }
        }

       
        public List<object> Tabelldata(string tablename)
        {
            throw new NotImplementedException();
        }


        //load variabel 
        public List<Variabel> LoadVariabel()
        {
            WriteToFile("\n\n********  LoadVariabel");
            //parse csv file
            var variabelList = CsvDataImporter_Variabel.ProcessCSV("in_data/csvfiles/variabel.csv");
            var loadedVariabels = new List<Variabel>();

            //load variabel data to model class
            foreach (var v in variabelList.OrderBy(v => v.Kode))
            {
                var _navn = $"{EnumUtil.ToDescription(v.Ecosystnivaa)} {EnumUtil.ToDescription(v.Variabelkategori)}".ToLower().Replace("_", " ");
                _navn = char.ToUpper(_navn[0]) + _navn.Substring(1);
                var variabel = new Variabel()
                {
                    Kode = v.Kode,
                    Ecosystnivaa = v.Ecosystnivaa,
                    Variabelkategori = v.Variabelkategori, // No semicolon here
                    Navn = _navn,
                    Versjon = this.Versjon
                };
                variabel.Langkode = LangkodeForVariabelType(VariabelklasseTypeEnum.V, variabel);
                _context.Add(variabel);
                loadedVariabels.Add(variabel);
            }

            _context.SaveChanges();
            return loadedVariabels;
        }

        public void LoadVariabelnavn()
        {
            WriteToFile("\n\n********  LoadVariabelnavn");
            //parse csv file
            var variabelList = CsvdataImporter_Variabelnavn.ProcessCSV("in_data/csvfiles/variabelnavn_variabel_mapping.csv");
            var loadedVariabelnavn = new List<Variabelnavn>();
            var parents = _context.Variabel.ToList();
            //load variabel data to model class
            foreach (var v in variabelList.OrderBy(v => v.Kode))
            {
                var parent = parents.FirstOrDefault(p => p.Kode == v.VariabelKode);
                var variabel = new Variabelnavn()
                {
                    Kode = v.Kode,
                    Navn = v.Navn,
                    Versjon = this.Versjon,
                    Variabelkategori2 = v.Variabelkategori2, // No semicolon here
                    Variabeltype = v.Variabeltype,
                    Variabelgruppe = v.Variabelgruppe,
                    Variabel = parent,
                    Langkode = v.Langkode
                };
                _context.Add(variabel);
            }

            _context.SaveChanges();
        }

        public void LoadMaaleskala()
        {
            WriteToFile("\n\n********  LoadMaaleskala");
            var MaaleskalaList = CsvdataImporter_maaleskala_enhet.ProcessCSV("in_data/csvfiles/maaleskala_enhet.csv");
            foreach (var m in MaaleskalaList)
            {
                var maaleskala = new Maaleskala()
                {
                    EnhetEnum = m.EnhetEnum,
                    MaaleskalatypeEnum = m.MaaleskalatypeEnum,
                    MaaleskalaNavn = m.Maaleskalanavn
                };
                _context.Add(maaleskala);
            }
            _context.SaveChanges();
        }

        public void LoadTrinn()
        {
            WriteToFile("\n\n********  LoadTrinn");
            //TODO: change lookup on maaleskala with maaleskalanavn instead of enum
            //Loop csvdata and add trinn to trinn-class/db
            var trinnList = CsvDataImporter_MaaleskalaTrinn.ProcessCSV("in_data/csvfiles/maaleskala_trinn.csv");
            List<Maaleskala> MaaleskalaList = _context.Maaleskala.ToList();
            List<string> TrinnsAdded = new List<string>();
            foreach (var t in trinnList)
            {
                var maaleskala = MaaleskalaList.Where(m => m.MaaleskalaNavn == t.Maaleskalanavn).FirstOrDefault();

                var trinn = new Trinn()
                {
                    Verdi = t.Trinn,
                    Beskrivelse = t.Trinnverdi,
                    Maaleskala = maaleskala
                    //Versjon = _versjon
                };

                if (trinn.Maaleskala != null)
                {
                    _context.Add(trinn);
                }
                else
                {
                    Console.WriteLine(t.Maaleskalanavn + " not found " + $"trinn was {t.Trinn}: {t.Trinnverdi}");
                }
                _context.SaveChanges();
            }
        }

        public void MakeMaaleskalaMappingForVariabelnavn()
        {
            WriteToFile("\n\n********  MakeMaaleskalaMappingForVariabelnavn");
            var variabelnavn_maaleskalaList = CsvdataImporter_Variabelnavn_maaleskala.ProcessCSV("in_data/csvfiles/variabelnavn_maaleskala_mapping.csv");
            foreach (var vm in variabelnavn_maaleskalaList)
            {
                // find variabelnavn by kode
                var variabelnavn = _context.Variabelnavn.FirstOrDefault(vn => vn.Kode == vm.VaribelnavnKode);
                // find maaleskala by navn
                var maaleskala = _context.Maaleskala.FirstOrDefault(m => m.MaaleskalaNavn == vm.Maaleskalanavn);

                if (maaleskala != null && variabelnavn != null)
                {
                    var vn_ms = new VariabelnavnMaaleskala()
                    {
                        Maaleskala = maaleskala,
                        Variabelnavn = variabelnavn
                    };
                    _context.Add(vn_ms);
                }
                else
                {
                    Console.WriteLine($"Maaleskala or Variabelnavn not found for vn: {vm.VaribelnavnKode}, ms: {vm.Maaleskalanavn}");
                }
                // create VariabelnavnMaaleskala object
                // add to db
                _context.SaveChanges();
            }
            //savechanges
        }

        public void LoadGrunntypeVariabeltrinnMapping()
        {
            WriteToFile("\n\n********  LoadGrunndataVariabeltrinnMapping");
            var grunntypeVariabeltrinnList = CsvDataImporter_grunntype_variabeltrinn.ProcessCSV("in_data/csvfiles/grunntype_variabeltrinn_mapping.csv");
            foreach (var grunntypeVariabeltrinn in grunntypeVariabeltrinnList)
            {
                //find grunntype
                var grunntype = _context.Grunntype.FirstOrDefault(gt => gt.Kode == grunntypeVariabeltrinn.grunntype_kode);
                var maaleskala = _context.Maaleskala.FirstOrDefault(m =>
                    m.MaaleskalaNavn == $"{grunntypeVariabeltrinn.varkode2}-SO" ||
                    m.MaaleskalaNavn == $"{grunntypeVariabeltrinn.varkode2}-SI");
                var trinn = _context.Trinn.FirstOrDefault(t => t.Verdi == grunntypeVariabeltrinn.trinn && t.Maaleskala == maaleskala);
                var variabelnavn = grunntypeVariabeltrinn.variabelnavnKode != null ? _context.Variabelnavn.FirstOrDefault(vn => vn.Kode == grunntypeVariabeltrinn.variabelnavnKode) : null;
                if (grunntype != null && maaleskala != null)
                {
                    var grunntypeVariabeltrinnMapping = new GrunntypeVariabeltrinn()
                    {
                        Variabelnavn = variabelnavn,
                        Grunntype = grunntype,
                        Maaleskala = maaleskala,
                        Trinn = trinn
                    };
                    _context.Add(grunntypeVariabeltrinnMapping);
                }
                else
                {
                    var msg = $@"Could not find one of the following:
                    grunntype: {grunntypeVariabeltrinn.grunntype_kode}, maaleskala: {grunntypeVariabeltrinn.varkode2}-SO or {grunntypeVariabeltrinn.varkode2}-SI, trinn: {grunntypeVariabeltrinn.trinn}";
                    WriteToFile(msg);
                }
            }
            _context.SaveChanges();
        }

        public void LoadHovedtypeVariabeltrinnMapping()
        {
            WriteToFile("\n\n********  LoadHovedtypeVariabeltrinnMapping");
            var hovedtypeVariabeltrinnList = CsvdataImporter_Hovedtype_variabeltrinn.ProcessCSV("in_data/csvfiles/hovedtype_variabeltrinn_mapping.csv");
            foreach (var hovedtypeVariabeltrinn in hovedtypeVariabeltrinnList)
            {
                var hovedtype = _context.Hovedtype.FirstOrDefault(ht => ht.Kode == hovedtypeVariabeltrinn.hovedtype_kode);
                var maaleskala = _context.Maaleskala.FirstOrDefault(m =>
                        m.MaaleskalaNavn == $"{hovedtypeVariabeltrinn.varkode2}-SO" ||
                        m.MaaleskalaNavn == $"{hovedtypeVariabeltrinn.varkode2}-SI");
                var trinn = _context.Trinn.FirstOrDefault(t => t.Verdi == hovedtypeVariabeltrinn.trinn && t.Maaleskala == maaleskala);
                var variabelnavn = hovedtypeVariabeltrinn.variabelnavnKode != null ? _context.Variabelnavn.FirstOrDefault(vn => vn.Kode == hovedtypeVariabeltrinn.variabelnavnKode) : null;
                if (hovedtype != null && maaleskala != null)
                {
                    var hovedtypeVariabeltrinnMapping = new HovedtypeVariabeltrinn()
                    {
                        Variabelnavn = variabelnavn,
                        Hovedtype = hovedtype,
                        Maaleskala = maaleskala,
                        Trinn = trinn
                    };
                    _context.Add(hovedtypeVariabeltrinnMapping);
                }
                else
                {
                    var msg = $@"Could not find one of the following:
                    hovedtype: {hovedtypeVariabeltrinn.hovedtype_kode}[{hovedtype.Navn}], maaleskala: {hovedtypeVariabeltrinn.varkode2}-SO or {hovedtypeVariabeltrinn.varkode2}-SI,trinn: {hovedtypeVariabeltrinn.trinn}";
                    WriteToFile(msg);
                }
            }
        }

        public void LoadKonverteringHovedtypegruppe()
        {
            WriteToFile("\n\n********  LoadKonverteringHovedtypegruppe");
            var forrigeVersjon = _context.Versjon.FirstOrDefault(v => v.Navn == "2.3");
            var versjon = _context.Versjon.FirstOrDefault(v => v.Navn == "3.0");
            var htgKonvList = CsvDataImporter_konvertering_hovedtypegruppe.ProcessCSV("in_data/csvfiles/konvertering_htg_v30.csv");
            foreach (var htgk in htgKonvList)
            {
                //var htg = _context.Hovedtypegruppe.FirstOrDefault(h => h.Kode == htk.Kode);
                var konvert = new Konvertering()
                {
                    Klasse = KlasseEnum.HTG,
                    Kode = htgk.Kode,
                    ForrigeKode = htgk.ForrigeKode,
                    ForrigeVersjon = forrigeVersjon,
                    Versjon = versjon,
                    Url = htgk.Url,
                    FoelsomhetsPresisjon = htgk.FoelsomhetsPresisjon,
                    Spesifiseringsevne = htgk.Spesifiseringsevne
                };
                _context.Add(konvert);
            }
            _context.SaveChanges();
        }


        public void LoadKonverteringHovedtype()
        {
            WriteToFile("\n\n********  LoadKonverteringHovedtype");
            var forrigeVersjon = _context.Versjon.FirstOrDefault(v => v.Navn == "2.3");
            var versjon = _context.Versjon.FirstOrDefault(v => v.Navn == "3.0");
            var htKonvList = CsvDataImporter_konvertering_hovedtype.ProcessCSV("in_data/csvfiles/konvertering_ht_v30.csv");
            foreach (var htk in htKonvList)
            {
                //var htg = _context.Hovedtypegruppe.FirstOrDefault(h => h.Kode == htk.Kode);
                var konvert = new Konvertering()
                {
                    Klasse = KlasseEnum.HT,
                    Kode = htk.Kode,
                    ForrigeKode = htk.ForrigeKode,
                    ForrigeVersjon = forrigeVersjon,
                    Versjon = versjon,
                    Url = htk.Url,
                    FoelsomhetsPresisjon = htk.FoelsomhetsPresisjon,
                    Spesifiseringsevne = htk.Spesifiseringsevne
                };
                _context.Add(konvert);
            }
            _context.SaveChanges();
        }


        public void LoadKonverteringGrunntype()
        {
            WriteToFile("\n\n********  LoadKonverteringGrunntype");
            var forrigeVersjon = _context.Versjon.FirstOrDefault(v => v.Navn == "2.3");
            var versjon = _context.Versjon.FirstOrDefault(v => v.Navn == "3.0");
            var gtKonvList = CsvDataImporter_konvertering_hovedtype.ProcessCSV("in_data/csvfiles/konvertering_gt_v30.csv");
            foreach (var gtk in gtKonvList)
            {
                //var htg = _context.Hovedtypegruppe.FirstOrDefault(h => h.Kode == htk.Kode);
                var konvert = new Konvertering()
                {
                    Klasse = KlasseEnum.GT,
                    Kode = gtk.Kode,
                    ForrigeKode = gtk.ForrigeKode,
                    ForrigeVersjon = forrigeVersjon,
                    Versjon = versjon,
                    Url = gtk.Url,
                    FoelsomhetsPresisjon = gtk.FoelsomhetsPresisjon,
                    Spesifiseringsevne = gtk.Spesifiseringsevne
                };
                _context.Add(konvert);
            }
            _context.SaveChanges();
        }


        public void LoadKonverteringVariabelnavn() {
            WriteToFile("\n\n********  LoadKonverteringVariabelnavn");
            var forrigeVersjon = _context.Versjon.FirstOrDefault(v => v.Navn == "2.3");
            var versjon = _context.Versjon.FirstOrDefault(v => v.Navn == "3.0");
            var vnKonvList = CsvDataImporter_konvertering_hovedtype.ProcessCSV("in_data/csvfiles/konvertering_vn_v30.csv");
            foreach (var vnk in vnKonvList)
            {
                //var htg = _context.Hovedtypegruppe.FirstOrDefault(h => h.Kode == htk.Kode);
                var konvert = new Konvertering()
                {
                    Klasse = KlasseEnum.VN,
                    Kode = vnk.Kode,
                    ForrigeKode = vnk.ForrigeKode,
                    ForrigeVersjon = forrigeVersjon,
                    Versjon = versjon,
                    Url = vnk.Url,
                    FoelsomhetsPresisjon = vnk.FoelsomhetsPresisjon,
                    Spesifiseringsevne = vnk.Spesifiseringsevne
                };
                _context.Add(konvert);
            }
            _context.SaveChanges();
        }


        /********* Loadings for RapportService *********/

        public void LoadAlleKortkoder()
        {
            // load kortkoder from type
            foreach (var t in _context.Type.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = t.Kode,
                    TypeKlasseEnum = KlasseEnum.T,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var htg in _context.Hovedtypegruppe.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = htg.Kode,
                    TypeKlasseEnum = KlasseEnum.HTG,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var ht in _context.Hovedtype.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = ht.Kode,
                    TypeKlasseEnum = KlasseEnum.HT,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var gt in _context.Grunntype.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = gt.Kode,
                    TypeKlasseEnum = KlasseEnum.GT,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var ke in _context.Kartleggingsenhet.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = ke.Kode,
                    TypeKlasseEnum = KlasseEnum.KE,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var v in _context.Variabel.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = v.Kode,
                    TypeKlasseEnum = KlasseEnum.V,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            foreach (var vn in _context.Variabelnavn.ToList())
            {
                var kortkode = new AlleKortkoder()
                {
                    Kortkode = vn.Kode,
                    TypeKlasseEnum = KlasseEnum.VN,
                    Versjon = this.Versjon
                };
                _context.Add(kortkode);
            }
            _context.SaveChanges();
        }




        /********* Convenience tables/views *********/

        public void LoadEnumoppslag()
        {
            foreach (EcosystnivaaEnum value in Enum.GetValues(typeof(EcosystnivaaEnum)))
            {
                _context.Add(MakeEnumoppslag("EcosysnivaaEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (EnhetEnum value in Enum.GetValues(typeof(EnhetEnum)))
            {
                _context.Add(MakeEnumoppslag("EnhetEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (HovedoekosystemEnum value in Enum.GetValues(typeof(HovedoekosystemEnum)))
            {
                _context.Add(MakeEnumoppslag("HovedoekosystemEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (KlasseEnum value in Enum.GetValues(typeof(KlasseEnum)))
            {
                _context.Add(MakeEnumoppslag("KlasseEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (MaaleskalatypeEnum value in Enum.GetValues(typeof(MaaleskalatypeEnum)))
            {
                _context.Add(MakeEnumoppslag("MaaleskalatypeEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (MaalestokkEnum value in Enum.GetValues(typeof(MaalestokkEnum)))
            {
                _context.Add(MakeEnumoppslag("MaalestokkEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (ProsedyrekategoriEnum value in Enum.GetValues(typeof(ProsedyrekategoriEnum)))
            {
                _context.Add(MakeEnumoppslag("ProsedyrekategoriEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (TypekategoriEnum value in Enum.GetValues(typeof(TypekategoriEnum)))
            {
                _context.Add(MakeEnumoppslag("TypekategoriEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (Typekategori2Enum value in Enum.GetValues(typeof(Typekategori2Enum)))
            {
                _context.Add(MakeEnumoppslag("Typekategori2Enum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (Typekategori3Enum value in Enum.GetValues(typeof(Typekategori3Enum)))
            {
                _context.Add(MakeEnumoppslag("Typekategori3Enum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (VariabelgruppeEnum value in Enum.GetValues(typeof(VariabelgruppeEnum)))
            {
                _context.Add(MakeEnumoppslag("VariabelgruppeEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (VariabelkategoriEnum value in Enum.GetValues(typeof(VariabelkategoriEnum)))
            {
                _context.Add(MakeEnumoppslag("VariabelkategoriEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (Variabelkategori2Enum value in Enum.GetValues(typeof(Variabelkategori2Enum)))
            {
                _context.Add(MakeEnumoppslag("Variabelkategori2Enum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            foreach (VariabeltypeEnum value in Enum.GetValues(typeof(VariabeltypeEnum)))
            {
                _context.Add(MakeEnumoppslag("VariabeltypeEnum", (int)value, value.ToString(), EnumUtil.ToDescription(value), Versjon));
            }
            _context.SaveChanges();
        }
        public Enumoppslag MakeEnumoppslag(string enumtype, int ordinal, string verdi, string beskrivelse, Versjon versjon)
        {
            return new Enumoppslag()
            {
                Ordinal = ordinal,
                Verdi = verdi,
                Enumtype = enumtype,
                Beskrivelse = beskrivelse,
                Versjon = versjon
            };
        }

        public void CreateMaaleskalaView()
        {//TODO: funker ikke ...feil med query
            var sql = @"Create view MaaleskalaView
                        AS
                        select Maaleskala.Id as MaaleskalaId, op1.Ordinal, op1.Verdi as Maaleskalaverdi, op1.Beskrivelse as Maaleskalabeskrivelse, 
                               op2.Verdi as Enhetverdi, op2.Beskrivelse as Enhetbeskrivelse 
                        from Maaleskala, Enumoppslag as op1, Enumoppslag as op2 
                        where op1.Ordinal = Maaleskala.MaaleskalatypeEnum and op1.Enumtype = 'MaaleskalatypeEnum' and 
                              op2.Ordinal =Maaleskala.EnhetEnum and op2.Enumtype = 'EnhetEnum';";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void CreateGrunntypeVariabeltrinnView()
        {
            var sql = @"Create view GrunntypeVariabeltrinnView
                        AS
                        select gt.kode as GTkode, vn.Kode as VNkode, ms.MaaleskalaNavn,t.Verdi as Trinnkode 
                        from 
                            GrunntypeVariabeltrinn gtvt, 
                            Variabelnavn vn,
                            Grunntype gt,
                            Maaleskala ms,
                            Trinn t
                        where gtvt.VariabelnavnId = vn.Id 
                              and gtvt.GrunntypeId = gt.Id
                              and gtvt.MaaleskalaId = ms.Id
                              and gtvt.TrinnId = t.Id";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void CreateHovedtypeVariabeltrinnView()
        {
            var sql = @"Create view HovedtypeVariabeltrinnView
                        AS
                       SELECT ht.kode AS HTkode, vn.Kode AS VNkode, ms.MaaleskalaNavn, t.Verdi AS Trinnkode 
                        FROM HovedtypeVariabeltrinn htvt
                        LEFT JOIN Variabelnavn vn ON htvt.VariabelnavnId = vn.Id
                        LEFT JOIN Hovedtype ht ON htvt.HovedtypeId = ht.Id
                        LEFT JOIN Maaleskala ms ON htvt.MaaleskalaId = ms.Id
                        LEFT JOIN Trinn t ON htvt.TrinnId = t.Id";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void CreateSjekkUnikeHovedklasserView()
        {
            var sql = @"Create view SjekkUnikeHovedklasserView
                        AS
                        Select 'Type' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Type
                        UNION
                        Select 'Hovedtypegruppe' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Hovedtypegruppe
                        UNION
                        Select 'Hovedtype' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Hovedtype
                        UNION
                        Select 'Grunntype' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Grunntype
                        UNION
                        Select 'Variabel' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Variabel
                        UNION
                        Select 'Variabelnavn' as klasse , count(distinct(kode)) as unique_count, count(*) as count  from Variabelnavn";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void CreateHovedtypeKleView() {
            var sql = @"Create view HovedtypeKleView 
                        AS
                        select H.Kode as HTKode, K.Kode as KLKode, E.Verdi as Maalestokk
                        from Hovedtype_Kartleggingsenhet hk,
                             Hovedtype H,
                             Kartleggingsenhet K,
                             Enumoppslag E
                        Where KartleggingsenhetId = K.Id AND HovedtypeId = H.Id
                             and E.Ordinal = K.Maalestokk and E.Enumtype = 'MaalestokkEnum'
                             Order by HTKode, K.Maalestokk";
            _context.Database.ExecuteSqlRaw(sql);
        }

        /*
        public void CreateAlleLangkoderView() {
            var sql = @"Create view AlleLangkoderView AS
                        select 'Type' as Klasse, Kode, Langkode, Navn from Type
                        UNION ALL
                        select 'Hovedtypegruppe' as Klasse, Kode, Langkode, Navn from Hovedtypegruppe
                        UNION ALL
                        select 'Hovedtype' as Klasse, Kode, Langkode, Navn from Hovedtype
                        UNION ALL
                        select 'Grunntype' as Klasse, Kode, Langkode, Navn from Grunntype
                        UNION ALL
                        select 'Kartleggingsenhet' as Klasse, Kode, Langkode, Navn from Kartleggingsenhet
                        UNION ALL
                        select 'Variabel' as Klasse, Kode, Langkode, Navn from Variabel
                        UNION ALL
                        select 'Variabelnavn' as Klasse, Kode, Langkode, Navn from Variabelnavn";
            _context.Database.ExecuteSqlRaw(sql);
        }*/
        public void CreateAlleLangkoderView()
        {
            var sql = @"INSERT INTO AlleLangkoderView (Klasse, Kode, Langkode, Navn)
            SELECT 'Type' as Klasse, Kode, Langkode, Navn FROM Type
            UNION ALL
            SELECT 'Hovedtypegruppe' as Klasse, Kode, Langkode, Navn FROM Hovedtypegruppe
            UNION ALL
            SELECT 'Hovedtype' as Klasse, Kode, Langkode, Navn FROM Hovedtype
            UNION ALL
            SELECT 'Grunntype' as Klasse, Kode, Langkode, Navn FROM Grunntype
            UNION ALL
            SELECT 'Kartleggingsenhet' as Klasse, Kode, Langkode, Navn FROM Kartleggingsenhet
            UNION ALL
            SELECT 'Variabel' as Klasse, Kode, Langkode, Navn FROM Variabel
            UNION ALL
            SELECT 'Variabelnavn' as Klasse, Kode, Langkode, Navn FROM Variabelnavn";
            _context.Database.ExecuteSqlRaw(sql);
            /*using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                _context.Database.OpenConnection();
                command.ExecuteNonQuery();
            }*/
        }

public void CreateDuplikateLangkoderView() {
            var sql = @"create view DuplikateLangkoderView AS
                        SELECT Langkode, Klasse, COUNT(*) AS DuplicateCount
                        FROM AlleLangkoderView
                        GROUP BY Langkode, Klasse
                        HAVING COUNT(*) > 1";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void CreateDBInfoTable() { 
            var sql = @"CREATE TABLE db_info (
                            Tittel TEXT,
                            Verdi TEXT
                        )";
            _context.Database.ExecuteSqlRaw(sql);
        }

        public void SetBuildTimeInDb_infoTable()
        {
            var sql = @"INSERT INTO db_info (Tittel, Verdi) VALUES ('Opprettet', strftime('%Y%m%d_%H%M%S', 'now', 'localtime'))";
            _context.Database.ExecuteSqlRaw(sql);
        }

        //function that receives string and writes to file on C:\temp
        public void WriteToFile(string text)
        {
            Console.WriteLine(text);
            //getstring from current datetime in format yyyyMMddHHmmss
            //string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");            
            // This text is added only once to the file.
            if (!File.Exists(logpath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(logpath))
                {
                    sw.WriteLine(text);
                }
            }
            else
            {
                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(logpath))
                {
                    sw.WriteLine(text);
                }
            }   
        }
    }
}
