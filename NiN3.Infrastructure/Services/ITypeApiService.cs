using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.type;

namespace NiN3.Infrastructure.Services
{
    public interface ITypeApiService
    {
        public Task<VersjonDto> AllCodesAsync(string versjon);
        public KlasseDto GetTypeklasse(string kortkode, string versjon);

        public TypeDto GetTypeByKortkode(string kortkode, string versjon);
        public HovedtypegruppeDto GetHovedtypegruppeByKortkode(string kode, string versjon);

        public HovedtypeDto GetHovedtypeByKortkode(string kode, string versjon);
        public GrunntypeDto GetGrunntypeByKortkode(string kode, string versjon);
        public KartleggingsenhetDto GetKartleggingsenhetByKortkode(string kode, string versjon);
    }
}
