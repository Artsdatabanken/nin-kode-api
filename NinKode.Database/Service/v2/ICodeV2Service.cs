namespace NinKode.Database.Service.v2
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeV2Service
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}