namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NiN.Database;

    public interface IVersionService
    {
        IEnumerable<string> GetVersions(NiNDbContext dbContext);
    }
}
