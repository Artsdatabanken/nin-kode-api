namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Variety;

    public interface IVarietyService
    {
        IEnumerable<VarietyAllCodes> GetAll(string host, string version = "");
        VarietyCode GetByKode(string id, string host, string version = "");
        VarietyCode GetVariety(string id);
    }
}