namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeService
    {
        IEnumerable<Codes> GetAll(string host, string version = "");
        Codes GetByKode(string id, string host, string version = "");
        Codes GetCode(string id);
    }
}