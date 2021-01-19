namespace NinKode.Database.Service.v21
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeV21Service
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}