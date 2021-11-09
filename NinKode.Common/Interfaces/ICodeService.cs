namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NiN.Database;
    using NinKode.Common.Models.Code;

    public interface ICodeService
    {
        IEnumerable<Codes> GetAll(NiNContext context, string host, string version = "");
        Codes GetByKode(NiNContext context, string id, string host, string version = "");
        Codes GetCode(string id);
    }
}
