using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NiN.Infrastructure.Services;
using NiN3.Infrastructure.DbContexts;
using NiN3.Infrastructure.Mapping;
using NiN3.Infrastructure.Services;

namespace NiN3.Tests.Infrastructure
{
    [Collection("Sequential")]
    public class VariabelApiServiceTest
    {

        private IMapper _mapper;
        private ILogger<VariabelApiService> _logger;
        private NiN3DbContext inmemorydb;
        private IConfiguration _configuration;


        //Tests that alter data must call InMemoryDbContextFactory.Dispose() after test is done
        public VariabelApiServiceTest()
        {
            _logger = new Mock<ILogger<VariabelApiService>>().Object;
            //_configuration = configuration;
        }


        private VariabelApiService GetPrepearedVariabelApiService(bool reloadDB = false)
        {
            inmemorydb = InMemoryDbContextFactory.GetInMemoryDb(reloadDB);
            var mapper = NiNkodeMapper.Instance;
            mapper.SetConfiguration(CreateConfiguration());
            if (inmemorydb.Type.Count() == 0)
            {//if data is not allready loaded 
                var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
                loader.load_all_data();
            }
            var service = new VariabelApiService(inmemorydb, _logger);
            return service;
        }

        public IConfiguration CreateConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    //{ "root_url", "http://localhost:5001/v3.0" }
                    { "root_url", "http://localhost:5001" }
                })
                .Build();

            return _configuration;
        }


        [Fact]
        public void TestGetAllCodes()
        {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var result = service.AllCodes(versjon);
            var variabler = result.Variabler;
            var firstVariabel = variabler.OrderBy(x => x.Kode.Id).First();
            Assert.Equal(4, variabler.Count);
            Assert.Equal("A-M", firstVariabel.Kode.Id);
            var count = variabler.Count(x => x.Kode.Id == "A-M");
            Assert.Equal(1, count);
            Assert.Equal("Abiotisk menneskebetinget", firstVariabel.Navn);
            Assert.Equal("NIN-3.0-V-A-M", firstVariabel.Kode.Langkode);
        }


        [Fact]
        public void TestGetAllCodesWithMaaleskalaTrinn()
        {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var result = service.AllCodes(versjon);
            var vn_RM_MS = result.Variabler.SelectMany(x => x.Variabelnavn).Where(x => x.Kode.Id == "RM-MS").FirstOrDefault();
            Assert.NotNull(vn_RM_MS);
            Assert.Equal(1, vn_RM_MS.Variabeltrinn.Count);
            var maaleskala = vn_RM_MS.Variabeltrinn.FirstOrDefault();
            Assert.Equal(5, maaleskala.Trinn.Count);
        }

        [Fact]
        public void TestDefinisjonUrlForVariabel() {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var result = service.AllCodes(versjon);
            var variabler = result.Variabler;
            var variabel_B_N = variabler.Where(x => x.Kode.Id== "B-N").First();

            var rootUrl = _configuration.GetValue<string>("root_url");
            var definisjon = variabel_B_N.Kode.Definisjon.Replace(rootUrl, "");
            //evaluate rest of url without rootUrl-part of it
            Assert.Equal("https://nin-kode-api.artsdatabanken.no/v3.0/variabler/kodeforVariabel/B-N", definisjon);
        }


        [Fact]
        public void TestGetKlasseByKortkode() {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var result = service.GetVariabelKlasse("RM-MS", versjon);
        }

        [Fact]
        public void GetMaaleskalaByMaaleskalanavn() { 
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var result = service.GetMaaleskalaByMaaleskalanavn("BK-SI", versjon);
            Assert.NotNull(result);
            Assert.Equal("BK-SI", result.MaaleskalaNavn);
            Assert.True(result.Trinn.Count==5);
        }

        [Fact]
        public void TestFetchVariabelnavnWithKonvertering_KM_AH()
        {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var variabelnavn_KM_AH = service.GetVariabelnavnByKortkode("KM-AH", "3.0");
            Assert.NotNull(variabelnavn_KM_AH);
            Assert.Equal(3, variabelnavn_KM_AH.Konverteringer.Count);
        }
        
        [Fact]
        public void TestFetchTrinnForVariabelnavn_VS_SS_W()
        {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var variabelnavn_VS_SS_W = service.GetVariabelnavnByKortkode("VS-SS_W", "3.0");
            Assert.NotNull(variabelnavn_VS_SS_W);
            var vt = variabelnavn_VS_SS_W.Variabeltrinn;
            Assert.Equal(1, variabelnavn_VS_SS_W.Variabeltrinn.Count);
            Assert.Equal(16, vt.First().Trinn.Count);
        }

        [Fact]
        public void TestTrinnAndKonverteringViaGetMaaleskala() 
        { 
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var maaleskala = service.GetMaaleskalaByMaaleskalanavn("BA-SO", versjon);
            Assert.NotNull(maaleskala);
            var trinn_RM_BA_0 = maaleskala.Trinn.Where(x => x.Kode == "RM-BA_0").FirstOrDefault();
            Assert.NotNull(trinn_RM_BA_0);
            Assert.Equal(1, trinn_RM_BA_0.Konverteringer.Count);
            var konvertering = trinn_RM_BA_0.Konverteringer.First();
            Assert.Equal("RM-BA_0", konvertering.Kode);
            Assert.Equal("6KE�1", konvertering.ForrigeKode);
            Assert.Equal("2.3", konvertering.ForrigeVersjon);
        }

        [Fact]
        public void TestTrinnAndKonverteringViaGetVariabelnavn()
        {
            var service = GetPrepearedVariabelApiService();
            var versjon = "3.0";
            var variabelnavn = service.GetVariabelnavnByKortkode("LM-HM", versjon);
            Assert.NotNull(variabelnavn);
            var maaleskala = variabelnavn.Variabeltrinn.Where(vt => vt.MaaleskalaNavn == "HM-SO").FirstOrDefault();
            Assert.NotNull(maaleskala);
            var trinn_LM_HM_0 = maaleskala.Trinn.Where(x => x.Kode == "LM-HM_0").FirstOrDefault();
            Assert.NotNull(trinn_LM_HM_0);
            // Check konvertering for this trinn
            Assert.Equal(1, trinn_LM_HM_0.Konverteringer.Count);
            var konvertering = trinn_LM_HM_0.Konverteringer.First();
            Assert.Equal("LM-HM_0", konvertering.Kode);
            Assert.Equal("HI�0ab", konvertering.ForrigeKode);
            Assert.Equal("2.3", konvertering.ForrigeVersjon);
        }
    }
}
