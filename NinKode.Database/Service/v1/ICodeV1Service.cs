namespace NinKode.Database.Service.v1
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeV1Service
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}