namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NinKode.Common.Models.Variety;

    public interface IVarietyService
    {
        IEnumerable<VarietyAllCodes> GetAll(string host);
        VarietyCode GetByKode(string id, string host);
    }
}