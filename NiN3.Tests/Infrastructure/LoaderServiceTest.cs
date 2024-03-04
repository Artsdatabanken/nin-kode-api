using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NiN.Infrastructure.Services;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.DbContexts;

namespace NiN3.Tests.Infrastructure
{

    [Collection("Sequential")]
    public class LoaderServiceTest
    {
        private LoaderService _loaderService;
        private ILogger<LoaderService> _logger;

        //Methods that alter data must call InMemoryDbContextFactory.Dispose() after test is done
        public LoaderServiceTest()
        {
            _logger = new Mock<ILogger<LoaderService>>().Object;
        }
        private static NiN3DbContext GetInMemoryDb(bool loadAll=true)//out SqliteConnection connection, out DbContextOptions<NiN3DbContext> options)
        {
            var inmemorydb = InMemoryDbContextFactory.GetInMemoryDb();
            if (inmemorydb.Type.Count() == 0)
            {//if data is not allready loaded 
                var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
                if (loadAll) { loader.load_all_data(); }  
            }
            return inmemorydb;
        }

        /*  ONLY FOR SPECIAL TESTING PURPOSES
        [Fact]
        public void TestLoadUntilType()
        {
            var inmemorydb = InMemoryDbContextFactory.GetInMemoryDb();
            var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
            loader.SeedLookupData();
            loader.LoadLookupData();
            loader.LoadTypeData();
            var typer = inmemorydb.Type.ToList();
            Assert.Equal(10, typer.Count);
        }
        
        [Fact]
        public void TestLoadUntilHTG() { 
                        var inmemorydb = InMemoryDbContextFactory.GetInMemoryDb();
            var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
            loader.SeedLookupData();
            loader.LoadLookupData();
            loader.LoadTypeData();
            loader.LoadType_HTG_Mappings();
            loader.LoadHovedtypeGruppeData();
            var numHTG = inmemorydb.Hovedtypegruppe.Count();
            Assert.Equal(65, numHTG);
        }

        [Fact]
        public void TestLoadUntilHT() {
            var inmemorydb = InMemoryDbContextFactory.GetInMemoryDb();
            var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
            loader.SeedLookupData();
            loader.LoadLookupData();
            loader.LoadTypeData();
            loader.LoadType_HTG_Mappings();
            loader.LoadHovedtypeGruppeData();
            loader.LoadHovedtypegruppeHovedoekosystemer();
            loader.LoadHtg_Ht_Gt_Mappings();
            loader.LoadHovedtypeData();
            var numHT = inmemorydb.Hovedtype.Count();
            Assert.Equal(412, numHT);
        }*/

        [Fact]
        public void TestLoadHtg_Ht_Gt_Mappings() {
            //hovedtype "U-0-01" finner ikke hovedtypegruppe (0-U) under loading
            var inmemorydb = InMemoryDbContextFactory.GetInMemoryDb();
            var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
            loader.SeedLookupData();
            loader.LoadHtg_Ht_Gt_Mappings();
            loader.csvdataImporter_Hovedtypegruppe_Hovedtype_Mappings.Count.Should().Be(430);
            //loader.csvdataImporter_Hovedtype_Grunntype_Mappings.Count.Should().Be(1901); //20240220
            loader.csvdataImporter_Hovedtype_Grunntype_Mappings.Count.Should().Be(1401);
        }


        [Fact]
        public void TestLoadType()
        {
            // Get an instance of the InMemoryDb class
            var inmemorydb = GetInMemoryDb();
            // Get the number of Type records
            var numOfTD = inmemorydb.Type.Count();
            var typer = inmemorydb.Type.ToList();
            // Assert that the number of records is 10
            Assert.Equal(10, numOfTD);

            //Test a type langkode within ecosystnivaa = C
            var type_eco_C = inmemorydb.Type.Where(t => t.Kode == "C-PE-NA").FirstOrDefault(); //Get Type with Kode = "C-PE-NA"
            Assert.Equal("NIN-3.0-T-C-PE-NA", type_eco_C.Langkode);
            Assert.Equal("Natursystem", type_eco_C.Navn);

            //Test a type langkode within ecosystnivaa = A 
            var type_eco_A = inmemorydb.Type.Where(t => t.Kode == "A-MV-0").FirstOrDefault(); //Get Type with Kode = "CA-MV-0"
            Assert.Equal("NIN-3.0-T-A-MV-0", type_eco_A.Langkode);
            Assert.Equal("Marine vannmasser", type_eco_A.Navn); //Har ikke typekategori2 dermed blankt navn
        }

        //This code tests the LoadHovedtypeGruppeData() method of the LoaderService class. 
        //It creates an InMemoryDb object and passes it to the LoaderService constructor. 
        //It then calls the LoadHovedtypeGruppeData() method and checks the number of Hovedtypegruppe objects in the InMemoryDb object. 
        //It then asserts that the number of Hovedtypegruppe objects is equal to 70. 
        [Fact]
        public void TestLoadHovedtypeGruppe()
        {
            //Create an InMemoryDb object
            var inmemorydb = GetInMemoryDb();
            //Get the number of Hovedtypegruppe objects in the InMemoryDb object
            var numOfHGD = inmemorydb.Hovedtypegruppe.Count();
            //Assert that the number of Hovedtypegruppe objects is equal to 71
            Assert.Equal(68, numOfHGD); // 70 after #174


            //Testing langkode for Hovedtypegruppe that should have typekategori3 embedded in langkode
            var HTG_NA_T = inmemorydb.Hovedtypegruppe.Where(x => x.Kode == "NA-T").FirstOrDefault();
            //Assert.Equal("NIN-3.0-T-C-PE-NA-MB-NA-T", HTG_NA_I.Langkode);
            Assert.Equal("NIN-3.0-T-C-PE-NA-MB-NA-T", HTG_NA_T.Langkode);

        }

        [Fact]
        public void TestLoadhovedtypeGruppeAndCheckHovedoekosystem() {
            //Create an InMemoryDb object
            var inmemorydb = GetInMemoryDb();

            //Testing a Hovedtypegruppe with one Hovedoekosystem
            var HTG_NA_I = inmemorydb.Hovedtypegruppe.Where(x => x.Kode == "NA-I").FirstOrDefault();
            Assert.Equal("NIN-3.0-T-C-PE-NA-MB-NA-I", HTG_NA_I.Langkode);
            Assert.Equal("Snø- og issystemer", HTG_NA_I.Navn);
            Assert.Equal(2, HTG_NA_I.Hovedoekosystemer.Count);
            var Hovedtypgruppe_Hovedoekosystem_First = HTG_NA_I.Hovedoekosystemer.FirstOrDefault();
            var Hovedtypgruppe_Hovedoekosystem_Second = HTG_NA_I.Hovedoekosystemer.LastOrDefault();
            Assert.Equal(HovedoekosystemEnum.H, Hovedtypgruppe_Hovedoekosystem_First.HovedoekosystemEnum);//HAV and
            Assert.Equal(HovedoekosystemEnum.L, Hovedtypgruppe_Hovedoekosystem_Second.HovedoekosystemEnum);//LAND

            //Testing a Hovedtypegruppe with multiple Hovedoekosystemer
            var HTG_LI_MU = inmemorydb.Hovedtypegruppe.Where(x => x.Kode == "LI-MU").FirstOrDefault();
            Assert.Equal(1, HTG_LI_MU.Hovedoekosystemer.Count);
            var Hovedtypgruppe_Hovedoekosystem = HTG_LI_MU.Hovedoekosystemer.FirstOrDefault();
            Assert.Equal(HovedoekosystemEnum.H, Hovedtypgruppe_Hovedoekosystem.HovedoekosystemEnum);//HAV
        }

        [Fact]
        //This code tests the LoadHovedtype method by creating an in-memory database,
        //instantiating a LoaderService object, and then calling the LoadHovedtypeGruppeData, LoadHtg_Ht_Gt_Mappings,
        //and LoadHovedtypeData methods. 

        public void TestLoadHovedtype()
        {
            //Create an in-memory database
            var inmemorydb = GetInMemoryDb();

            //Check that the number of Hovedtype objects in the in-memory database is equal to 445
            var numOfHD = inmemorydb.Hovedtype.Count();
            var hovedtyper = inmemorydb.Hovedtype.ToList();
            Assert.True(numOfHD>400); //flux between 412 and 421
            var HT_NK_C01 = inmemorydb.Hovedtype.Where(x => x.Kode == "NK-C01").FirstOrDefault();
            Assert.NotNull(HT_NK_C01);
            Assert.Equal("Varmkildekompleks", HT_NK_C01.Navn);
            Assert.NotNull(HT_NK_C01.Hovedtypegruppe);
            //Assert.Equal("NIN-3.0-T-C-SE-NK-0-C-01", HT_NK_C01.Langkode);
            Assert.Equal("NIN-3.0-T-C-SE-NK-0-K-C01", HT_NK_C01.Langkode);

            //testing that hovedtype 'T-M-01' has typekategori3 embedded in langkode, (child of HTG "NA-T")
            var HT_NA_TM01 = inmemorydb.Hovedtype.Where(x => x.Kode == "NA-TM01").FirstOrDefault();
            Assert.Equal("NIN-3.0-T-C-PE-NA-MB-A-TM01", HT_NA_TM01.Langkode);
            Assert.Equal("Hard sterkt endret fastmark", HT_NA_TM01.Navn);
        }

        [Fact]
        public void TestHovedtype_langkoder() // For #185 & #201
        {
            var inmemorydb = GetInMemoryDb();
            var ht_LK_0_02 = inmemorydb.Hovedtype.Where(x => x.Kode == "LA-K02").FirstOrDefault();
            Assert.Equal("NIN-3.0-T-C-PE-LA-0-A-K02", ht_LK_0_02.Langkode);//NIN-3.0-T-C-PE-FL-0-LK-0-02?
            Assert.Equal("Fjordlandskap", ht_LK_0_02.Navn);

            var ht_LK_0_01 = inmemorydb.Hovedtype.Where(x => x.Kode == "LA-K01").FirstOrDefault();
            Assert.Equal("NIN-3.0-T-C-PE-LA-0-A-K01", ht_LK_0_01.Langkode);
            Assert.Equal("Kystås- og kystfjellandskap", ht_LK_0_01.Navn);

            var ht_LK_0_03 = inmemorydb.Hovedtype.Where(x => x.Kode == "LA-K03").FirstOrDefault();
            Assert.Equal("NIN-3.0-T-C-PE-LA-0-A-K03", ht_LK_0_03.Langkode);
            Assert.Equal("Kystslettelandskap", ht_LK_0_03.Navn);
        }

        [Fact]
        public void TestHovedtypeFromHovedtypegruppe()
        {
            var inmemorydb = GetInMemoryDb();
            var c = inmemorydb.Hovedtypegruppe.Count();
            var hovedtypegruppe = inmemorydb.Hovedtypegruppe.Where(x => x.Kode == "NA-M")
                .Include(h => h.Hovedtyper).FirstOrDefault();
            //Q: get hovedtypegruppe with kode NA-I and fetch hovedtyper that has relation to it

            //Answer: 
            //var firstHGD = inmemorydb.Hovedtypegruppe.OrderBy(x => x.Kode).First();
            Assert.NotNull(hovedtypegruppe);
            Assert.Equal(23, hovedtypegruppe.Hovedtyper.Count);
        }

        // Rewritten code with comments
        [Fact]
        public void TestLoadGrunntype()
        {
            // Get an instance of the InMemoryDb class
            var inmemorydb = GetInMemoryDb();
            // Get the number of Grunntype records
            var numOfGD = inmemorydb.Grunntype.Count();
            // Assert that the number of records is 166
            Assert.Equal(1401, numOfGD);
            // Get the Grunntype record with the code "M-A-01-05"
            var grunntype = inmemorydb.Grunntype.Where(gt => gt.Kode == "MA01-05").FirstOrDefault();
            // Assert that the name of the record is "sukkertareskog"
            Assert.Equal("Spiraltangbunn", grunntype.Navn);
            // Assert that the delkode of the record is "05"
            Assert.Equal("05", grunntype.Delkode);
            //Testing langkode for M-A-01-05
            Assert.Equal("NIN-3.0-T-C-PE-NA-MB-MA01-05", grunntype.Langkode);
        }                


        [Fact]
        public void TestKartleggingsenhet_ALL_m005_has_GT() {
            var inmemorydb = GetInMemoryDb();
            //Fetch number of KLE_M005
            var num005M = inmemorydb.Kartleggingsenhet.Where(x => x.Maalestokk == MaalestokkEnum.M005).Count();
            //Fetch number of KLE_M005 that has GT
            var num005MGT = inmemorydb.Kartleggingsenhet_Grunntype.Where(x => x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M005).Select(x=>x.Kartleggingsenhet).Distinct().Count();            
            Assert.Equal(657, num005M);
            Assert.Equal(654, num005MGT);
            var M005_TB01_M005_09 = inmemorydb.Kartleggingsenhet.Where(x => x.Kode == "TB01-M005-09").FirstOrDefault();
            Assert.NotNull(M005_TB01_M005_09);//Test for #175
            Assert.Equal(MaalestokkEnum.M005, M005_TB01_M005_09.Maalestokk);
            var KLE_GT = inmemorydb.Kartleggingsenhet_Grunntype.Where(x => x.Kartleggingsenhet == M005_TB01_M005_09).ToList();
            Assert.Equal(1, KLE_GT.Count());

        }

        [Fact]
        public void TestKartleggingsenhet_Hovedtype_M005() {
            var inmemorydb = GetInMemoryDb();
            //Fetch number of KLE_M005
            var num005M = inmemorydb.Kartleggingsenhet.Where(x => x.Maalestokk == MaalestokkEnum.M005).Count();
            // Fetch number of KLE_M005 that has HT
            var num005MHT = inmemorydb.Hovedtype_Kartleggingsenhet.Where(x => x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M005).Select(x => x.Kartleggingsenhet).Distinct().Count();
            // Fetch number of Hovedtype that has KLE_M005
            var numHTM005 = inmemorydb.Hovedtype_Kartleggingsenhet.Where(x => x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M005).Select(x => x.Hovedtype).Distinct().Count();
            //Number of unique Hovedtype
            var numHT = inmemorydb.Hovedtype.Count();
            Assert.Equal(657, num005M);//647 before #175
            Assert.Equal(654, num005MHT);
            Assert.Equal(123, numHTM005);
            Assert.Equal(430, numHT);
        }

        [Fact]
        public void TestKartleggingsenhet_Hovedtype_NA_TB01() {
            var inmemorydb = GetInMemoryDb();
            var HT_NA_TB01 = inmemorydb.Hovedtype.Where(x => x.Kode == "NA-TB01").FirstOrDefault();
            var HT_NA_TB01_Kartleggingsenheter = inmemorydb.Hovedtype_Kartleggingsenhet.Where(x => x.Hovedtype == HT_NA_TB01 && x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M005).Select(x => x.Kartleggingsenhet).Distinct().ToList();
            Assert.Equal(15, HT_NA_TB01_Kartleggingsenheter.Count);
        }


        //Test HT T-B-01 skal ha 15 KLE_M005
        [Fact]
        public void TestKartleggingsenhet_Hovedtype_NT_B_01() {
            var inmemorydb = GetInMemoryDb();
            var HT_NA_TB01 = inmemorydb.Hovedtype.Where(x => x.Kode == "NA-TB01").FirstOrDefault();
            var HT_NA_TB01_Kartleggingsenheter = inmemorydb.Hovedtype_Kartleggingsenhet.Where(x => x.Hovedtype == HT_NA_TB01 && x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M005).Select(x => x.Kartleggingsenhet).Distinct().ToList();
            Assert.Equal(15, HT_NA_TB01_Kartleggingsenheter.Count);
        }

        [Fact]
        public void TestKartleggingsenhet_ALL_m020_has_GT()
        {
            var inmemorydb = GetInMemoryDb();
            //Fetch number of KLE_M005
            var num020M = inmemorydb.Kartleggingsenhet.Where(x => x.Maalestokk == MaalestokkEnum.M020).Count();
            //Fetch number of KLE_M005 that has GT
            var num020MGT = inmemorydb.Kartleggingsenhet_Grunntype.Where(x => x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M020).Select(x => x.Kartleggingsenhet).Distinct().Count();
            Assert.Equal(397, num020M);
            Assert.Equal(397, num020MGT);
        }

        [Fact]
        public void TestKartleggingsenhet_ALL_m050_has_GT()
        {
            var inmemorydb = GetInMemoryDb();
            //Fetch number of KLE_M005
            var num050M = inmemorydb.Kartleggingsenhet.Where(x => x.Maalestokk == MaalestokkEnum.M050).Count();
            //Fetch number of KLE_M005 that has GT
            var num050MGT = inmemorydb.Kartleggingsenhet_Grunntype.Where(x => x.Kartleggingsenhet.Maalestokk == MaalestokkEnum.M050).Select(x => x.Kartleggingsenhet).Distinct().Count();
            Assert.Equal(242, num050M);
            Assert.Equal(238, num050MGT);
        }


        //create test for loadvariabel
        [Fact]
        public void TestLoadVariabel()
        {
            var inmemorydb = GetInMemoryDb();
            // Create a new LoaderService instance
            var numOfV = inmemorydb.Variabel.Count();
            var firstV = inmemorydb.Variabel.OrderBy(x => x.Kode).First();
            Assert.NotNull(firstV);
            Assert.Equal("A-M", firstV.Kode);
            Assert.Equal("Abiotisk menneskebetinget", firstV.Navn);
            Assert.Equal(4, numOfV);
            Assert.Equal("NIN-3.0-V-A-M", firstV.Langkode);
        }

        [Fact]  
        public void TestLoadVariabelnavn()
        {
            var inmemorydb = GetInMemoryDb();
            var VariabelnavnList = inmemorydb.Variabelnavn.ToList();
            var numOfVN = VariabelnavnList.Count();
            var firstVN = VariabelnavnList.OrderBy(x => x.Kode).First();
            Assert.NotNull(firstVN);
            Assert.Equal(367, numOfVN);
            Assert.True(firstVN.Kode == "AD-FA");
            Assert.True(firstVN.Navn == "Fremmedartsantall");
            Assert.True(firstVN.Variabelkategori2.ToString() == "AD");
            Assert.True(firstVN.Variabelgruppe.ToString() == "FA");
            Assert.True(firstVN.Variabeltype.ToString() == "GE");
            Assert.True(firstVN.Versjon.Navn == "3.0");
        }


        /// <summary>
        /// Test method for loading of the Variabeltrinn.
        /// </summary>
        [Fact]
        public void TestLoadMaaleskala()
        {
            // prepare test
            var inmemorydb = GetInMemoryDb();
            var numOfMS = inmemorydb.Maaleskala.Count();
            Assert.Equal(158, numOfMS);
            var maaleskalaSO = inmemorydb.Maaleskala.FirstOrDefault(m => m.MaaleskalatypeEnum == MaaleskalatypeEnum.SO);
            var maaleskalaSI = inmemorydb.Maaleskala.FirstOrDefault(m => m.MaaleskalatypeEnum == MaaleskalatypeEnum.SI);
            Assert.NotNull(maaleskalaSO);
            Assert.True(EnhetEnum.VSO == maaleskalaSO.EnhetEnum);
            Assert.True(EnhetEnum.VSI == maaleskalaSI.EnhetEnum);
        }

        [Fact]
        public void TestTotalTrinn()
        {
            var inmemorydb = GetInMemoryDb();
            var numOfTrinn = inmemorydb.Trinn.Count();
            Assert.Equal(1024, numOfTrinn);
        }


        [Fact]
        public void TestLoadTrinn_NH_B() {
            var inmemorydb = GetInMemoryDb();
            var trinnNhB = inmemorydb.Trinn.Where(trinn => trinn.Verdi == "NH_B").FirstOrDefault();
            var bMaaleskala = inmemorydb.Maaleskala.Where(m=> m.MaaleskalaNavn == "B").FirstOrDefault();
            Assert.Equal(2, bMaaleskala.Trinn.Count);//Checking that Binær-måleskala is loaded and has its trinn
            Assert.Equal("Barentshavet og polhavet", trinnNhB.Beskrivelse);
            Assert.Equal(MaaleskalatypeEnum.SI, trinnNhB.Maaleskala.MaaleskalatypeEnum);
        }

        [Fact]
        public void TestMakeMaalestokkMappingForVariabelnavn() {
            var inmemorydb = GetInMemoryDb();

            var vn_LO_KA = inmemorydb.Variabelnavn.Where(x => x.Kode == "LO-KA").FirstOrDefault();
            Assert.NotNull(vn_LO_KA);
            Assert.Equal("LO-KA", vn_LO_KA.Kode);
            Assert.Equal(2, vn_LO_KA.VariabelnavnMaaleskala.Count);
            var maaleskalasInVn = vn_LO_KA.VariabelnavnMaaleskala.Select(x => x.Maaleskala).OrderBy(x => x.MaaleskalaNavn).ToList();
            Assert.Equal(2, maaleskalasInVn.Count);
            Assert.Equal("D0", maaleskalasInVn[0].MaaleskalaNavn);
            Assert.Equal(11, maaleskalasInVn[0].Trinn.Count);
            Assert.Equal("T0", maaleskalasInVn[1].MaaleskalaNavn);
            Assert.Equal(16, maaleskalasInVn[1].Trinn.Count);
        }

        [Fact]
        public void TestFetchTrinnFromVariabelnavn()
        {
            // prepare test
            var inmemorydb = GetInMemoryDb();
            // Testing Trinn mapping
            var variabelnavn = inmemorydb.Variabelnavn.Where(x => x.Kode == "RM-MS").FirstOrDefault();
            //var variabelnavnRMMS = inmemorydb.Variabelnavn
            //                     .Where(v => v.Kode == "RM-MS")
            //                     .Include(v => v.VariabelnavnMaaleskala).FirstOrDefault();
           
            
            Assert.Equal(1, variabelnavn.VariabelnavnMaaleskala.Count);
            /*Assert.Equal("marine bioklimatiske soner", trinnMapping.Variabelnavn.Navn);
            Assert.Equal("MS-a", trinnMapping.Trinn.Navn);
            Assert.Equal("Nordsjøen og Skagerrak", trinnMapping.Trinn.Verdi);
            Assert.Equal(MaaleskalatypeEnum.SO, trinnMapping.Variabeltrinn.MaaleskalatypeEnum);*/
        }

        [Fact]
        public void TestLoadGrunntypeVariabelTrinnMapping() {
            // prepare test
            var inmemorydb = GetInMemoryDb();
            var gtvt = inmemorydb.GrunntypeVariabeltrinn.Count();
            Assert.True(12000<gtvt);
            var grunntype_TE05_01 = inmemorydb.Grunntype.Where(x => x.Kode == "TE01-05").FirstOrDefault(); 
            Assert.NotNull(grunntype_TE05_01);
            Assert.Equal(15, grunntype_TE05_01.GrunntypeVariabeltrinn.Count());
        }

        [Fact]
        public void TestLoadHovedtypeVariabelTrinnMapping() {
            var inmemorydb = GetInMemoryDb();
            var htvt = inmemorydb.HovedtypeVariabeltrinn.Count();
            Assert.True(100 < htvt);
            var ht_NK_C01 = inmemorydb.Hovedtype.Where(x => x.Kode == "NA-SC01").FirstOrDefault();
            Assert.NotNull(ht_NK_C01);
            Assert.Equal(5, ht_NK_C01.HovedtypeVariabeltrinn.Count());
        }


        [Fact]
        public void TestLoadAlleKortkoderForType() {
            var inmemorydb = GetInMemoryDb();
            //service.LoadAlleKortkoder();
            var numOfKortkoder = inmemorydb.AlleKortkoder.Count();
            Assert.Equal(3576, numOfKortkoder);//3557 before #175
            var kortkode = inmemorydb.AlleKortkoder.Where(x => x.Kortkode == "IB-F").FirstOrDefault();            
            Assert.NotNull(kortkode);
            Assert.Equal("IB-F", kortkode.Kortkode);
            Assert.Equal(KlasseEnum.HTG, kortkode.TypeKlasseEnum);
            Assert.Null(kortkode.KortkodeV2); // not yet impl.
            var numOfTyper= inmemorydb.Type.Count();
            var numOfHovedtypegrupper = inmemorydb.Hovedtypegruppe.Count();
            var numOfHovedtyper = inmemorydb.Hovedtype.Count();
            var numOfGrunntyper = inmemorydb.Grunntype.Count();
            var numOfKartleggingsenheter = inmemorydb.Kartleggingsenhet.Count();
            var countHTinAlleKortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.HT);
            var countTypeinAlleKortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.T);
            var countHtginAlleKortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.HTG);
            var countGtinAlleKortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.GT);
            var countKeinAlleKortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.KE);
            Assert.True(numOfTyper == countTypeinAlleKortkoder);
            Assert.True(numOfHovedtypegrupper == countHtginAlleKortkoder);
            Assert.Equal(numOfHovedtyper,countHTinAlleKortkoder);
            Assert.True(numOfGrunntyper == countGtinAlleKortkoder);
            Assert.True(numOfKartleggingsenheter == countKeinAlleKortkoder);
        }

        [Fact]
        public void TestLoadAlleKortkoder_Variabler() {
            var inmemorydb = GetInMemoryDb();
            var numOfVariabelAllekortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.V);
            var numOfVariabelnavnAllekortkoder = inmemorydb.AlleKortkoder.Count(x => x.TypeKlasseEnum == KlasseEnum.VN);
            Assert.Equal(4, numOfVariabelAllekortkoder);
            Assert.Equal(367,numOfVariabelnavnAllekortkoder);
        }

        [Fact]
        public void TestLoadEnumoppslag() {
            var inmemorydb = GetInMemoryDb();
            var EnumoppslagEcosystnivaa = inmemorydb.Enumoppslag.Where(e => e.Enumtype== "EcosysnivaaEnum").ToList();
            Assert.Equal(3, EnumoppslagEcosystnivaa.Count);
            var EnumoppslagEnhetEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "EnhetEnum").ToList();
            Assert.Equal(8, EnumoppslagEnhetEnum.Count);
            var EnumoppslagHovedoekosystemEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "HovedoekosystemEnum").ToList();
            Assert.Equal(3, EnumoppslagHovedoekosystemEnum.Count);
            var EnumoppslagKlasseEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "KlasseEnum").ToList(); 
            Assert.Equal(9, EnumoppslagKlasseEnum.Count);
            var EnumoppslagMaaleskalatypeEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "MaaleskalatypeEnum").ToList();
            Assert.Equal(18, EnumoppslagMaaleskalatypeEnum.Count);
            var EnumoppslagProsedyrekategoriEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "ProsedyrekategoriEnum").ToList();
            Assert.Equal(15, EnumoppslagProsedyrekategoriEnum.Count);
            var EnumoppslagMaalestokkEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "MaalestokkEnum").ToList();
            Assert.Equal(5, EnumoppslagMaalestokkEnum.Count);
            var EnumoppslagTypekategoriEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "TypekategoriEnum").ToList();
            Assert.Equal(5, EnumoppslagTypekategoriEnum.Count);
            var EnumoppslagTypekategori2Enum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "Typekategori2Enum").ToList();
            Assert.Equal(8, EnumoppslagTypekategori2Enum.Count);
            var EnumoppslagTypekategori3Enum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "Typekategori3Enum").ToList();
            Assert.Equal(2, EnumoppslagTypekategori3Enum.Count);
            var EnumoppslagVariabelgruppeEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "VariabelgruppeEnum").ToList(); 
            Assert.Equal(32, EnumoppslagVariabelgruppeEnum.Count);
            var EnumoppslagVariabeltypeEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "VariabeltypeEnum").ToList();
            Assert.Equal(4, EnumoppslagVariabeltypeEnum.Count);
            var EnumoppslagVariabelkategoriEnum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "VariabelkategoriEnum").ToList();
            Assert.Equal(2, EnumoppslagVariabelkategoriEnum.Count);
            var EnumoppslagVariabelkategori2Enum = inmemorydb.Enumoppslag.Where(e => e.Enumtype == "Variabelkategori2Enum").ToList();
            Assert.Equal(14, EnumoppslagVariabelkategori2Enum.Count);
        }

        [Fact]
        public void TestLoadKonvertering_hovedtypegruppe() { 
            var db = GetInMemoryDb();
            //var loader = new LoaderService(null, db, new Mock<ILogger<LoaderService>>().Object);
            //loader.SeedLookupData();
            //loader.LoadKonverteringHovedtypegruppe();
            var versjon = db.Versjon.Where(v => v.Navn == "3.0").FirstOrDefault();
            var konvertering = db.Konvertering.Where(k => k.Klasse == KlasseEnum.HTG && k.Versjon ==versjon)
                .ToList();
            Assert.Equal(4, konvertering.Count);
        }

        [Fact]
        public void TestLoadKonvertering_hovedtype() {
            var db = GetInMemoryDb();
            //var loader = new LoaderService(null, db, new Mock<ILogger<LoaderService>>().Object);
            //loader.SeedLookupData();
            //loader.LoadKonverteringHovedtype();
            var versjon = db.Versjon.Where(v => v.Navn == "3.0").FirstOrDefault();
            var konvertering = db.Konvertering.Where(k => k.Klasse == KlasseEnum.HT && k.Versjon == versjon)
                .ToList();
            Assert.Equal(60, konvertering.Count);
            var konv4_NA_SA01 = db.Konvertering.Where(k => k.Kode == "NA-SA01" && k.Versjon == versjon).ToList();
            Assert.Equal(2, konv4_NA_SA01.Count);
        }

        [Fact]
        public void TestLoadKonvertering_grunntype() {
            var db = GetInMemoryDb();
            //var loader = new LoaderService(null, db, new Mock<ILogger<LoaderService>>().Object);
            //loader.SeedLookupData();
            //loader.LoadKonverteringGrunntype();
            var versjon = db.Versjon.Where(v => v.Navn == "3.0").FirstOrDefault();
            var konvertering = db.Konvertering.Where(k => k.Klasse == KlasseEnum.GT && k.Versjon == versjon)
                .ToList();
            Assert.Equal(425, konvertering.Count);
        }

        [Fact]
        public void TestLoadKonvertering_variabelnavn()
        {
            var db = GetInMemoryDb();
            //var loader = new LoaderService(null, db, new Mock<ILogger<LoaderService>>().Object);
            //loader.SeedLookupData();
            //loader.LoadKonverteringVariabelnavn();
            var versjon = db.Versjon.Where(v => v.Navn == "3.0").FirstOrDefault();
            var konvertering = db.Konvertering.Where(k => k.Klasse == KlasseEnum.VN && k.Versjon == versjon)
                .ToList();
            Assert.Equal(119, konvertering.Count);
        }


    }
}