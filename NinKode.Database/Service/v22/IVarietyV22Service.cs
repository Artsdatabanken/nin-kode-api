namespace NinKode.Database.Service.v22
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Variety;

    public interface IVarietyV22Service
    {
        IEnumerable<VarietyAllCodes> GetAll(string host);
        VarietyCode GetByKode(string id, string host);
    }
}