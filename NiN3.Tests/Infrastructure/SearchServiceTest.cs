
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
    public class SearchServiceTest
    {
        private IMapper _mapper;
        private NiN3DbContext inmemorydb;

    private SearchService GetPrepearedSearchService(bool reloadDB = false)
    {
        inmemorydb = InMemoryDbContextFactory.GetInMemoryDb(reloadDB);
        var mapper = NiNkodeMapper.Instance;
        mapper.SetConfiguration(CreateConfiguration());
        if (inmemorydb.Type.Count() == 0)
        {//if data is not allready loaded 
            var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
            loader.load_all_data();
        }
        var service = new SearchService(inmemorydb);
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

        [Fact]
        public void SimpleSearchTest()
        {
            var service = GetPrepearedSearchService();
            var result = service.SimpleSearch("Livs", KlasseEnum.T, SearchMethodEnum.SW);
            Assert.True(result.Count > 0);
            Assert.Equal("Livsmedium", result[0].Navn);
            var result2 = service.SimpleSearch("limnisk", KlasseEnum.ALL, SearchMethodEnum.C);
            Assert.Equal(7, result2.Count);
        }
    }
}
