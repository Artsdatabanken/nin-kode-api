namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NiN.Database;
    using NinKode.Common.Models.Variety;

    public interface IVarietyService
    {
        IEnumerable<VarietyAllCodes> GetAll(NiNDbContext context, string host, string version = "");
        VarietyCode GetByKode(NiNDbContext context, string id, string host, string version = "");
        VarietyCode GetVariety(string id);
    }
}