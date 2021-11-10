namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NiN.Database;
    using NinKode.Common.Models.Code;

    public interface ICodeService
    {
        IEnumerable<Codes> GetAll(NiNDbContext dbContext, string host, string version = "");
        Codes GetByKode(NiNDbContext dbContext, string id, string host, string version = "");
        Codes GetCode(string id);
    }
}
