namespace NinKode.Database.Service.v21b
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Variety;

    public interface IVarietyV21BService
    {
        IEnumerable<VarietyAllCodes> GetAll(string host);
        VarietyCode GetByKode(string id, string host);
    }
}