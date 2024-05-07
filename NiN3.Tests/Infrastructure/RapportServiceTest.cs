using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
    public class RapportServiceTest
    {
        private IMapper _mapper;
        private ILogger<RapportService> _logger;
        private NiN3DbContext inmemorydb;

        public RapportServiceTest()
        {
            _logger = new Mock<ILogger<RapportService>>().Object;
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

        private RapportService GetPrepearedRapportService(bool reloadDB = false)
        {
            inmemorydb = InMemoryDbContextFactory.GetInMemoryDb(reloadDB);
            var mapper = NiNkodeMapper.Instance;
            mapper.SetConfiguration(CreateConfiguration());
            if (inmemorydb.Type.Count() == 0)
            {//if data is not allready loaded 
                var loader = new LoaderService(null, inmemorydb, new Mock<ILogger<LoaderService>>().Object);
                loader.load_all_data();
            }
            var service = new RapportService(inmemorydb, _logger);
            return service;
        }

        private static NiN3DbContext GetInMemoryDb()//out SqliteConnection connection, out DbContextOptions<NiN3DbContext> options)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<NiN3DbContext>()
            .UseSqlite(connection)
            .Options;
            var context = new NiN3DbContext(options);
            context.Database.EnsureCreated();
            return context;
        }


        [Fact]
        public void TestGetKodeSummary()
        {
            var service = GetPrepearedRapportService();
            var result = service.GetKodeSummary("3.0");
            Assert.NotNull(result);
            var kartleggingsenheter = result.Where(result => result.Klasse == "Kartleggingsenhet").ToList();
            Assert.Equal(1290, kartleggingsenheter.Count);//test for #133
            Assert.Equal(3572, result.Count);
        }
    }
}
