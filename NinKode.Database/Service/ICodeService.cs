namespace NinKode.Database.Service
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeService
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}