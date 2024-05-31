using NiN3.Core.Models;

namespace NiN3.Infrastructure.Services
{
    public interface ILoaderService
    {
        public IEnumerable<Versjon> HentDomener();

        List<string> Tabeller();

        List<object> Tabelldata(string tablename);

        void load_all_data();

        public void LoadKonverteringHovedtypegruppe();

    }
}
