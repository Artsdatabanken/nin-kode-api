namespace NinKode.Database.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using NiN.Database;
    using NinKode.Common.Interfaces;

    public class VersionService : IVersionService
    {
        public IEnumerable<string> GetVersions(NiNDbContext dbContext)
        {
            return dbContext.NinVersion.Select(x => $"v{x.Navn}");
        }
    }
}
