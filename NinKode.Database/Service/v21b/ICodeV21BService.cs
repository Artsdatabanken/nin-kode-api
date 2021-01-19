namespace NinKode.Database.Service.v21b
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeV21BService
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}