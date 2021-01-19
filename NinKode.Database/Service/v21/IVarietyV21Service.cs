namespace NinKode.Database.Service.v21
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Variety;

    public interface IVarietyV21Service
    {
        IEnumerable<VarietyAllCodes> GetAll(string host);
        VarietyCode GetByKode(string id, string host);
    }
}