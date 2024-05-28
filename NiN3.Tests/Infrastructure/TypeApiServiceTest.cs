using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NiN.Infrastructure.Services;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Mapping;
using NiN3.Infrastructure.Services;

namespace NiN3.Tests.Infrastructure
{
    [Collection("Sequential")]
    public class TypeApiServiceTest
    {
        private IMapper _mapper;
        private ILogger<TypeApiService> _logger;
        private NiN3DbContext inmemorydb;


        public TypeApiServiceTest()
        {
            _logger = new Mock<ILogger<TypeApiService>>().Object;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reloadDB"></param>
        /// <returns></returns>

        private TypeApiService GetPrepearedTypeApiService(bool reloadDB = false)
        {
            inmemorydb = InMemoryDbContextFactory.GetInMemoryDb(reloadDB);
            var mapper = NiNkodeMapper.Instance;
            mapper.SetConfiguration(CreateConfiguration());
            if (inmemorydb.Type.Count() == 0)
            {//if data is not allready loaded 
                var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
                loader.load_all_data();
            }
            var service = new TypeApiService(inmemorydb, _logger);
            //loader.OpprettInitDb();
            return service;
        }


        public IConfiguration CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "root_url", "http://localhost:5001/v3.0" }
                })
                .Build();

            return configuration;
        }


        ///<summary>
        ///Tests that all NiN-koder for "TypeDto" in version 3.0 can be loaded and parsed correctly.
        ///</summary>
        [Fact]
        public async Task TestAllTypeCodesInVersjon()
        {
            //rigMapper();
            TypeApiService service = GetPrepearedTypeApiService();
            var v3allCodes = await service.AllCodesAsync("3.0");
            Assert.Equal("3.0", v3allCodes.Navn);
            Assert.NotNull(v3allCodes);
            Assert.Equal(10, v3allCodes.Typer.Count);
            var firstType = v3allCodes.Typer.First();
            Assert.Equal("A-LV-BM", firstType.Kode.Id);
            Assert.Equal("Bremassiv", firstType.Navn);
            Assert.StartsWith("http", firstType.Kode.Definisjon);
            Assert.EndsWith("/v3.0/typer/kodeforType/A-LV-BM", firstType.Kode.Definisjon);
            var hovedtypegruppe_BM_A = firstType.Hovedtypegrupper.Where(htg => htg.Kode.Id == "BM-A").FirstOrDefault();
            Assert.NotNull(hovedtypegruppe_BM_A);

            Assert.Equal("BM-A", hovedtypegruppe_BM_A.Kode.Id);
            Assert.Equal("Bremassiv", hovedtypegruppe_BM_A.Navn);
            Assert.Null(hovedtypegruppe_BM_A.Typekategori3Navn);
            Assert.Equal(9, hovedtypegruppe_BM_A.Hovedtyper.Count);
        }


        [Fact]
        public void TestGetTypeKlasse()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var typeklasse = service.GetTypeklasse("A-LV-BM", "3.0");
            Assert.NotNull(typeklasse);
            Assert.Equal("Type", typeklasse.KlasseNavn);
            Assert.Equal(KlasseEnum.T, typeklasse.KlasseEnum);
        }

        [Fact]
        public void TestGetTypeByKortkode()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var type = service.GetTypeByKortkode("A-LV-BM", "3.0");
            Assert.NotNull(type);
        }

        [Fact]
        public void TestGetHovedtypegruppeByKortkode()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var hovedtypegruppe = service.GetHovedtypegruppeByKortkode("FL-G", "3.0");
            Assert.NotNull(hovedtypegruppe);
        }

        [Fact]
        public void TestGetHovedtypeByKortkode()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var hovedtype = service.GetHovedtypeByKortkode("NA-MA06", "3.0");
            Assert.NotNull(hovedtype);
        }


        [Fact]
        public void TestGetGrunntypeByKortkode()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var grunntype = service.GetGrunntypeByKortkode("K02-006", "3.0");
            Assert.NotNull(grunntype);
        }


        [Fact]
        public void TestGetKartleggingsenhetByKortkode()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var kartleggingsenhet = service.GetKartleggingsenhetByKortkode("LA01-M005-13", "3.0");
            Assert.NotNull(kartleggingsenhet);
        }


        ///<summary>
        ///Tests that a Kartleggingsenhet exists under a Hovedtype 
        ///and is placed correctly in the Type hierarchy.
        ///</summary>
        [Fact]
        public async Task TestAllCodes_kartleggingsenhet_exist_under_hovedtype_m005()
        {
            var service = GetPrepearedTypeApiService();
            var v3allCodesTask = service.AllCodesAsync("3.0");

            var v3allCodes = await v3allCodesTask.ConfigureAwait(false);
            var type_C_PE_NA = v3allCodes.Typer.FirstOrDefault(t => t.Kode.Id == "C-PE-NA");
            //assert not null
            Assert.NotNull(type_C_PE_NA);
            var htg_NA_I = type_C_PE_NA.Hovedtypegrupper.FirstOrDefault(htg => htg.Kode.Id == "NA-I");
            //assert not null
            Assert.NotNull(htg_NA_I);
            var ht_NA_IA01 = htg_NA_I.Hovedtyper.FirstOrDefault(ht => ht.Kode.Id == "NA-IA01");
            //assert not null
            Assert.NotNull(ht_NA_IA01);
            var kl_IA01_M005_03 = ht_NA_IA01.Kartleggingsenheter.SingleOrDefault(ke => ke.Kode.Langkode == "NiN-3.0-T-C-PE-NA-MB-IA01-M005-03");
            Assert.NotNull(kl_IA01_M005_03);
            Assert.Equal("Kryokonitt-preget breoverflate", kl_IA01_M005_03.Navn);
            Assert.Equal("Kartleggingsenhet", kl_IA01_M005_03.Kategori);
            Assert.Equal("Kartleggingsenhet tilpasset 1:5000", kl_IA01_M005_03.MaalestokkNavn);
        }

        /// <summary>
        /// This is a unit test for TypeApiService.
        /// </summary>|
        [Fact]
        public async Task TestAllCodes_kartleggingsenhet_exist_under_hovedtype_m020()
        {
            //arrange
            var service = GetPrepearedTypeApiService();

            //act
            var v3allCodes = await service.AllCodesAsync("3.0");
            var type_C_PE_NA = v3allCodes.Typer.FirstOrDefault(t => t.Kode.Id == "C-PE-NA");
            var htg_NA_I = type_C_PE_NA.Hovedtypegrupper.FirstOrDefault(htg => htg.Kode.Id == "NA-I");
            var ht_NA_IA01 = htg_NA_I.Hovedtyper.FirstOrDefault(ht => ht.Kode.Id == "NA-IA01");
            var kl_IA01_M020_02 = ht_NA_IA01.Kartleggingsenheter.SingleOrDefault(ke => ke.Kode.Langkode == "NiN-3.0-T-C-PE-NA-MB-IA01-M020-02");

            //assert
            Assert.NotNull(type_C_PE_NA);
            Assert.NotNull(htg_NA_I);
            Assert.NotNull(ht_NA_IA01);
            Assert.NotNull(kl_IA01_M020_02);
            Assert.Equal("Polar havis-overside", kl_IA01_M020_02.Navn);
            Assert.Equal("Kartleggingsenhet", kl_IA01_M020_02.Kategori);
            Assert.Equal("Kartleggingsenhet tilpasset 1:20 000", kl_IA01_M020_02.MaalestokkNavn);
        }




        [Fact]
        public void TestGetGrunntypeByKodeAndVariabeltrinn()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var grunntype = service.GetGrunntypeByKortkode("TE05-01", "3.0");
            Assert.NotNull(grunntype);
            var variabeltrinn = grunntype.Variabeltrinn;
            Assert.Equal(3, variabeltrinn.Count());
            var single_variabeltrinn = variabeltrinn.Where(vn=>vn.Maaleskala.MaaleskalaNavn=="KA-SO").First();
            Assert.Equal(10, single_variabeltrinn.Maaleskala.Trinn.Count());
            Assert.NotNull(single_variabeltrinn.Variabelnavn);
        }

        [Fact]
        public void TestGetHovedtypeByKodeAndVariabeltrinn()
        {
            //S-C-01
            TypeApiService service = GetPrepearedTypeApiService();
            var hovedtype = service.GetHovedtypeByKortkode("NA-SC01", "3.0");
            Assert.Equal(1, hovedtype.Variabeltrinn.Count());
        }

        [Fact]
        public void TestGetHovedtypegruppeByKode_w_konvertering()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var htg_BM_A = service.GetHovedtypegruppeByKortkode("BM-A", "3.0");
            Assert.NotNull(htg_BM_A);
            Assert.Equal(1, htg_BM_A.konverteringer.Count());//1 just now
        }

        [Fact]
        public void TestGetTypeLivsmediumAndMarineVannmasser()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var type_livsmedium = service.GetTypeByKortkode("C-LI-0", "3.0");
            Assert.NotNull(type_livsmedium);
            Assert.Equal("Livsmedium", type_livsmedium.Navn);
            Assert.Equal("C-LI-0", type_livsmedium.Kode.Id);

            var type_marine_vannmasser = service.GetTypeByKortkode("A-MV-0", "3.0");
            Assert.NotNull(type_marine_vannmasser);
            Assert.Equal("Marine vannmasser", type_marine_vannmasser.Navn);
            Assert.Equal("A-MV-0", type_marine_vannmasser.Kode.Id);
        }

        [Fact]
        public void TestGetHovedtypeByKode_w_konvertering()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var ht_NA_MM01 = service.GetHovedtypeByKortkode("NA-MM01", "3.0");
            Assert.NotNull(ht_NA_MM01);
            Assert.Equal(2, ht_NA_MM01.Konverteringer.Count());
        }


        [Fact]
        public void TestGetHovedtype_CheckKartleggingsenheterFor_O_C_01()
        {
            TypeApiService service = GetPrepearedTypeApiService();
            var ht_NA_IA01 = service.GetHovedtypeByKortkode("NA-IA01", "3.0");
            Assert.NotNull(ht_NA_IA01);
            //Assert count of all kartleggingsenheter for hovedtype O-C-01
            Assert.Equal(8, ht_NA_IA01.Kartleggingsenheter.Count());
            //Assert count of M005 kartleggingsenheter for hovedtype O-C-01
            var m005_on_ht = ht_NA_IA01.Kartleggingsenheter.Where(ke => ke.MaalestokkEnum == MaalestokkEnum.M005).ToList();
            Assert.Equal(4,m005_on_ht.Count());
        }


        [Fact]
        public void TestGetHovedtype_CheckKartleggingenheter_M050_For_hovedtype_M_A_06(){
            TypeApiService service = GetPrepearedTypeApiService();
            var htg_NA_MA06 = service.GetHovedtypeByKortkode("NA-MA06", "3.0");
            Assert.NotNull(htg_NA_MA06);
            //Assert count of all kartleggingsenheter for hovedtype M-A-06
            Assert.Equal(55, htg_NA_MA06.Kartleggingsenheter.Count());
            //Assert count of M050 kartleggingsenheter for hovedtype M-A-06
            var htg_NM_A_06_M050_list = htg_NA_MA06.Kartleggingsenheter.Where(ke => ke.MaalestokkEnum == MaalestokkEnum.M050).ToList();
            Assert.Equal(12, htg_NM_A_06_M050_list.Count());
        }


        [Fact]
        public void TestGetGrunntypeByKode_w_konvertering()
        {
            //M-A-06-19  6 stk
            TypeApiService service = GetPrepearedTypeApiService();
            var gt_MA06_19 = service.GetGrunntypeByKortkode("MA06-19", "3.0");
            Assert.NotNull(gt_MA06_19);
            Assert.Equal(6, gt_MA06_19.Konverteringer.Count());//6 just now
        }
    }
}

