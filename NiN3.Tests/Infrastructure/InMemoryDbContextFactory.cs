using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NiN3.Infrastructure.DbContexts;

namespace NiN3.Tests.Infrastructure
{
    public static class InMemoryDbContextFactory
    {
        private static NiN3DbContext inmemorydb { get; set; }
        private static NiN3DbContext context { get; set; }
        private static bool created = false;
        public static NiN3DbContext GetInMemoryDb(bool reloadDB = false)
        {
            if (!reloadDB && created)
            {
                return context;
            }

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<NiN3DbContext>()
            .UseSqlite(connection)
            .Options;
            context = new NiN3DbContext(options);
            context.Database.EnsureCreated();
            created = true;
            return context;
        }

        public static void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
            created = false;
        }
    }
}
