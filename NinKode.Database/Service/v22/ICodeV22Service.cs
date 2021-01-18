namespace NinKode.Database.Service.v22
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Code;

    public interface ICodeV22Service
    {
        IEnumerable<Codes> GetAll(string host);
        Codes GetByKode(string id, string host);
    }
}