namespace NinKode.Common.Interfaces
{
    using System.Collections.Generic;
    using NiN.Database;
    using NinKode.Common.Models.Variety;

    public interface IVarietyService
    {
        IEnumerable<VarietyAllCodes> GetAll(NiNDbContext dbContext, string host, string version = "");
        VarietyCode GetByKode(NiNDbContext dbContext, string id, string host, string version = "");
        VarietyCode GetVariety(string id);
    }
}