namespace NiN.ExportImport
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.EntityFrameworkCore;
    using NiN.Database;
    using NiN.Database.Converters;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Code.Enums;
    using NiN.Database.Models.Common;
    using NiN.ExportImport.Model;

    public class NinCodeImport
    {
        private readonly NiNDbContext _context;
        private readonly string _version;
        private readonly CsvConfiguration _csvConfiguration;

        public NinCodeImport(NiNDbContext ninContext, string version)
        {
            _context = ninContext;
            _version = version;
            _csvConfiguration = new CsvConfiguration(new CultureInfo("nb-NO"));
        }

        public IEnumerable<GrunntypeKartleggingRecord> FixKartleggingConnections(string path)
        {
            if (!File.Exists(path)) yield break;

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, _csvConfiguration);

            var records = csv.GetRecords<GrunntypeKartleggingRecord>();
            foreach (var record in records)
            {
                yield return record;
            }
        }

        public IEnumerable<GrunntypeBasistrinnRecord> GetGrunntypeBasistrinnRecords(string path)
        {
            if (!File.Exists(path)) yield break;

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, _csvConfiguration);

            var records = csv.GetRecords<GrunntypeBasistrinnRecord>();
            foreach (var record in records)
            {
                yield return record;
            }
        }

        public void FixLkm(string basePath)
        {
            var path = Path.Combine(basePath, $"ht_info_LKM_v{_version}.csv");
            if (!File.Exists(path)) return;

            var ninVersion = _context.NinVersion.FirstOrDefault(x => x.Navn.Equals(_version));
            if (ninVersion == null)
            {
                Console.WriteLine($"NiN-code version {_version} doesn't exist. Skipping...");
                return;
            }

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, _csvConfiguration);

            var records = csv.GetRecords<HtLkmRecords>();
            var i = 0;
            foreach (var record in records)
            {
                i++;
                var dLkm = CreateStringArray(record.dLKM);
                var hLkm = CreateStringArray(record.hLKM);
                var tLkm = CreateStringArray(record.tLKM);
                var uLkm = CreateStringArray(record.uLKM);

                var hovedtype = _context.Hovedtype
                    .Include(x => x.Kode)
                    .Include(x => x.Miljovariabler)
                    .FirstOrDefault(x =>
                        x.Version.Id == ninVersion.Id &&
                        x.Kode.KodeName.Equals($"NA {record.HovedtypeKode}"));

                if (hovedtype == null) throw new Exception($"Could not find NA {record.HovedtypeKode}");

                if (!hovedtype.Miljovariabler.Any()) continue;

                foreach (var m in hovedtype.Miljovariabler)
                {
                    var multiple = false;
                    var miljovariabel = _context.Miljovariabel
                        .Include(x => x.Kode)
                        .FirstOrDefault(x => x.Id == m.Id);

                    if (miljovariabel == null) throw new Exception("error");
                    miljovariabel.Kode.LkmKategori = LkmKategoriEnum._null;

                    if (uLkm != null && uLkm.Contains(miljovariabel.Kode.Kode.Substring(0, 2)))
                    {
                        if (miljovariabel.Kode.LkmKategori != LkmKategoriEnum._null) multiple = true;
                        miljovariabel.Kode.LkmKategori = LkmKategoriEnum.uLKM;
                    }
                    if (tLkm != null && tLkm.Contains(miljovariabel.Kode.Kode.Substring(0, 2)))
                    {
                        if (miljovariabel.Kode.LkmKategori != LkmKategoriEnum._null) multiple = true;
                        miljovariabel.Kode.LkmKategori = LkmKategoriEnum.tLKM;
                    }
                    if (hLkm != null && hLkm.Contains(miljovariabel.Kode.Kode.Substring(0, 2)))
                    {
                        if (miljovariabel.Kode.LkmKategori != LkmKategoriEnum._null) multiple = true;
                        miljovariabel.Kode.LkmKategori = LkmKategoriEnum.hLKM;
                    }
                    if (dLkm != null && dLkm.Contains(miljovariabel.Kode.Kode.Substring(0, 2)))
                    {
                        if (miljovariabel.Kode.LkmKategori != LkmKategoriEnum._null) multiple = true;
                        miljovariabel.Kode.LkmKategori = LkmKategoriEnum.dLKM;
                    }

                    if (multiple)
                    {
                        //Console.WriteLine($"dLKM: {JoinStrings(dLkm)}\thLKM: {JoinStrings(hLkm)}\ttLKM: {JoinStrings(tLkm)}\tuLKM: {JoinStrings(uLkm)}");
                        //Console.WriteLine($"{hovedtype.Kode.KodeName}:\t{miljovariabel.Kode.Kode}\t{miljovariabel.LkmKategori}");
                    }

                    if (miljovariabel.Kode.LkmKategori == LkmKategoriEnum._null)
                    {
                        Console.WriteLine($"{hovedtype.Kode.KodeName}:\t{miljovariabel.Kode.Kode}");
                        //continue;
                    }

                    //Console.WriteLine($"{i:#000} dLKM: {JoinStrings(dLkm)}\thLKM: {JoinStrings(hLkm)}\ttLKM: {JoinStrings(tLkm)}\tuLKM: {JoinStrings(uLkm)}");
                    //Console.WriteLine($"{i:#000} {hovedtype.Kode.KodeName}\t{miljovariabel.Kode.Kode}\t{miljovariabel.LkmKategori}");
                    
                    //Console.WriteLine($"{hovedtype.Kode.KodeName}:\t{miljovariabel.Kode.Kode}");

                    if (HasUnsavedChanges()) _context.SaveChanges();
                }
            }
        }

        private List<string> CreateStringArray(string value)
        {
            value = value.Trim();
            var stringArray = string.IsNullOrWhiteSpace(value)
                ? null
                : value.Split(new[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (stringArray == null) return null;

            for (var index = 0; index < stringArray.Count; index++)
            {
                stringArray[index] = stringArray[index].Trim();
            }

            return stringArray;
        }

        private string JoinStrings(List<string> strings)
        {
            return strings == null ? "" : string.Join(",", strings);
        }

        public void ImportCompleteNin(string basePath, bool allowUpdate = false)
        {
            var i = 0;
            Natursystem natursystem = null;

            var ninVersion = _context.NinVersion.FirstOrDefault(x => x.Navn.Equals(_version));
            if (ninVersion != null)
            {
                if (!allowUpdate)
                {
                    Console.WriteLine($"NiN-code version {ninVersion.Navn} exists. Skipping...");
                    return;
                }
            }
            else
            {
                ninVersion = new NinVersion { Navn = _version };
                _context.NinVersion.Add(ninVersion);
                _context.SaveChanges();

                ninVersion = _context.NinVersion.FirstOrDefault(x => x.Id == ninVersion.Id);
            }

            Console.WriteLine("\nGrunntyper");

            var path = Path.Combine(basePath, $"import_grunntyper_v{_version}.csv");
            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, _csvConfiguration);

                var records = csv.GetRecords<GrunntyperRecord>().ToList();
                var count = records.Count;
                var pos = 0;
                double percent;
                foreach (var record in records)
                {
                    percent = Math.Round(100 * ((double)++pos / count));
                    if (natursystem == null)
                    {
                        natursystem = _context.Natursystem
                            .Include(x => x.Kode)
                            .FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                    }
                    if (natursystem == null)
                    {
                        natursystem = CreateNatursystem(record, ninVersion);
                        Console.WriteLine($"{percent}% {++i:0####} natursystem\t{natursystem.Kode.KodeName} {natursystem.Navn}");
                        _context.Natursystem.Add(natursystem);
                    }

                    var hovedtypegruppe = _context.Hovedtypegruppe
                        .Include(x => x.Natursystem)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Natursystem.Id == natursystem.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypegruppeKode));
                    if (hovedtypegruppe == null)
                    {
                        hovedtypegruppe = CreateHovedtypegruppe(record, ninVersion, natursystem);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtypegruppe\t{hovedtypegruppe.Kode.KodeName} {hovedtypegruppe.Navn}");
                        _context.Hovedtypegruppe.Add(hovedtypegruppe);
                    }

                    var hovedtype = _context.Hovedtype
                        .Include(x => x.Hovedtypegruppe)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtypegruppe.Id == hovedtypegruppe.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypeKode));
                    if (hovedtype == null)
                    {
                        hovedtype = CreateHovedtype(record, ninVersion, natursystem, hovedtypegruppe);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtype\t{hovedtype.Kode.KodeName} {hovedtype.Navn}");
                        _context.Hovedtype.Add(hovedtype);
                    }

                    var grunntype = _context.Grunntype
                        .Include(x => x.Hovedtype)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtype.Id == hovedtype.Id &&
                            x.Kode.KodeName.Equals(record.GrunntypeKode));
                    if (grunntype == null)
                    {
                        grunntype = CreateGrunntype(record.GrunntypeNavn, record.GrunntypeKode, ninVersion, natursystem, hovedtype);
                        Console.WriteLine($"{percent}% {++i:0####} grunntype\t{grunntype.Kode.KodeName} {grunntype.Navn}");
                        _context.Grunntype.Add(grunntype);
                    }

                    if (HasUnsavedChanges())
                    {
                        _context.SaveChanges();
                    }
                    else
                    {
                        Console.Write($"\r{Math.Round(100 * ((double)pos / count))}%\t");
                    }
                }
            }

            Console.WriteLine("\rKartleggingsenheter");

            path = Path.Combine(basePath, $"import_kartleggingsenheter_v{_version}.csv");
            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, _csvConfiguration);

                var records = csv.GetRecords<KartleggingsenheterRecord>().ToList();
                var count = records.Count;
                var pos = 0;
                double percent;
                foreach (var record in records)
                {
                    percent = Math.Round(100 * ((double)++pos / count));
                    if (natursystem == null)
                    {
                        natursystem = _context.Natursystem
                            .Include(x => x.Kode)
                            .FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                    }
                    if (natursystem == null)
                    {
                        natursystem = CreateNatursystem(record, ninVersion);
                        Console.WriteLine($"{percent}% {++i:0####} natursystem\t{natursystem.Kode.KodeName} {natursystem.Navn}");
                        _context.Natursystem.Add(natursystem);
                    }

                    var hovedtypegruppe = _context.Hovedtypegruppe
                        .Include(x => x.Natursystem)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Natursystem.Id == natursystem.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypegruppeKode));
                    if (hovedtypegruppe == null)
                    {
                        hovedtypegruppe = CreateHovedtypegruppe(record, ninVersion, natursystem);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtypegruppe\t{hovedtypegruppe.Kode.KodeName} {hovedtypegruppe.Navn}");
                        _context.Hovedtypegruppe.Add(hovedtypegruppe);
                    }

                    var hovedtype = _context.Hovedtype
                        .Include(x => x.Hovedtypegruppe)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtypegruppe.Id == hovedtypegruppe.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypeKode));
                    if (hovedtype == null)
                    {
                        hovedtype = CreateHovedtype(record, ninVersion, natursystem, hovedtypegruppe);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtype\t{hovedtype.Kode.KodeName} {hovedtype.Navn}");
                        _context.Hovedtype.Add(hovedtype);
                    }

                    var kartleggingsenhet = _context.Kartleggingsenhet
                        .Include(x => x.Hovedtype)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtype.Id == hovedtype.Id &&
                            x.Kode.KodeName.Equals(record.KartleggingsenhetKode));
                    if (kartleggingsenhet == null)
                    {
                        kartleggingsenhet = CreateKartleggingsenhet(record, ninVersion, hovedtype);
                        Console.WriteLine($"{percent}% {++i:0####} kartleggingsenhet\t{kartleggingsenhet.Malestokk} {kartleggingsenhet.Kode.KodeName} {kartleggingsenhet.Definisjon}");
                        hovedtype.Kartleggingsenheter.Add(kartleggingsenhet);
                    }

                    if (HasUnsavedChanges())
                    {
                        _context.SaveChanges();
                    }
                    else
                    {
                        Console.Write($"\r{Math.Round(100 * ((double)pos / count))}%\t");
                    }
                }
            }

            Console.WriteLine("\rMiljøvariabler");

            path = Path.Combine(basePath, $"import_miljovariabler_v{_version}.csv");
            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, _csvConfiguration);

                var records = csv.GetRecords<MiljovariablerRecord>().ToList();
                var count = records.Count;
                var pos = 0;
                double percent;
                foreach (var record in records)
                {
                    percent = Math.Round(100 * ((double)++pos / count));
                    if (natursystem == null)
                    {
                        natursystem = _context.Natursystem
                            .Include(x => x.Kode)
                            .FirstOrDefault(x => x.Version.Id == ninVersion.Id);
                    }
                    if (natursystem == null)
                    {
                        natursystem = CreateNatursystem(record, ninVersion);
                        Console.WriteLine($"{percent}% {++i:0####} natursystem\t{natursystem.Kode.KodeName} {natursystem.Navn}");
                        _context.Natursystem.Add(natursystem);
                    }

                    var hovedtypegruppe = _context.Hovedtypegruppe
                        .Include(x => x.Natursystem)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Natursystem.Id == natursystem.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypegruppeKode));
                    if (hovedtypegruppe == null)
                    {
                        hovedtypegruppe = CreateHovedtypegruppe(record, ninVersion, natursystem);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtypegruppe\t{hovedtypegruppe.Kode.KodeName} {hovedtypegruppe.Navn}");
                        _context.Hovedtypegruppe.Add(hovedtypegruppe);
                    }

                    var hovedtype = _context.Hovedtype
                        .Include(x => x.Hovedtypegruppe)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtypegruppe.Id == hovedtypegruppe.Id &&
                            x.Kode.KodeName.Equals(record.HovedtypeKode));
                    if (hovedtype == null)
                    {
                        hovedtype = CreateHovedtype(record, ninVersion, natursystem, hovedtypegruppe);
                        Console.WriteLine($"{percent}% {++i:0####} hovedtype\t{hovedtype.Kode.KodeName} {hovedtype.Navn}");
                        _context.Hovedtype.Add(hovedtype);
                    }

                    var miljovariabel = _context.Miljovariabel
                        .Include(x => x.Hovedtype)
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Hovedtype.Id == hovedtype.Id &&
                            x.Kode.Kode.Equals(record.MiljovariabelKode));
                    if (miljovariabel == null)
                    {
                        miljovariabel = CreateMiljovariabel(record, ninVersion, hovedtype);
                        Console.WriteLine($"{percent}% {++i:0####} miljovariabel\t{miljovariabel.Kode.Kode} {miljovariabel.Navn}");
                        hovedtype.Miljovariabler.Add(miljovariabel);
                    }

                    var trinn = _context.Trinn
                        .FirstOrDefault(x =>
                            x.Version.Id == ninVersion.Id &&
                            x.Kode.KodeName.Equals(record.TrinnKode.Replace(" ", "")));
                    if (trinn == null)
                    {
                        trinn = CreateTrinn(record, ninVersion);
                        Console.WriteLine($"{percent}% {++i:0####} trinn\t{trinn.Kode.KodeName} {trinn.Navn}");
                        miljovariabel.Trinn.Add(trinn);
                    }

                    var basistrinnNavn = record.BasistrinnNavn;
                    if (!string.IsNullOrWhiteSpace(basistrinnNavn))
                    {
                        var basistrinnPrefix = basistrinnNavn.Substring(0, basistrinnNavn.IndexOf("-", StringComparison.Ordinal));
                        var basistrinnListe = basistrinnNavn.Substring($"{basistrinnPrefix}-".Length).ToCharArray();
                        foreach (var s in basistrinnListe)
                        {
                            var basistrinn = _context.Basistrinn
                                .Include(x => x.Trinn)
                                .FirstOrDefault(x =>
                                    x.Version.Id == ninVersion.Id &&
                                    x.Navn.Equals($"{basistrinnPrefix}-{s}"));
                            if (basistrinn == null)
                            {
                                basistrinn = CreateBasistrinn($"{basistrinnPrefix}-{s}", ninVersion);
                                
                                basistrinn.Trinn.Add(trinn);
                                Console.WriteLine($"{percent}% {++i:0####} basistrinn\t{basistrinn.Navn}");
                                trinn.Basistrinn.Add(basistrinn);
                            }
                            else if (!basistrinn.Trinn.Contains(trinn))
                            {
                                basistrinn.Trinn.Add(trinn);
                                Console.WriteLine($"{percent}% {++i:0####} basistrinn\t{basistrinn.Navn}");
                                trinn.Basistrinn.Add(basistrinn);
                            }
                        }
                    }

                    if (HasUnsavedChanges())
                    {
                        _context.SaveChanges();
                    }
                    else
                    {
                        Console.Write($"\r{Math.Round(100 * ((double)pos / count))}%\t");
                    }
                }
            }

            Console.WriteLine("\r    ");
        }

        #region private methods

        private bool HasUnsavedChanges()
        {
            return _context.ChangeTracker.Entries().Any(e =>
                e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted);
        }

        private Natursystem CreateNatursystem(GrunntyperRecord record, NinVersion ninVersion)
        {
            return CreateNatursystem(record.NatursystemNavn, record.NatursystemKode, ninVersion);
        }

        private Natursystem CreateNatursystem(KartleggingsenheterRecord record, NinVersion ninVersion)
        {
            return CreateNatursystem(record.NatursystemNavn, record.NatursystemKode, ninVersion);
        }

        private Natursystem CreateNatursystem(MiljovariablerRecord record, NinVersion ninVersion)
        {
            return CreateNatursystem(record.NatursystemNavn, record.NatursystemKode, ninVersion);
        }

        private Natursystem CreateNatursystem(string name, string code, NinVersion ninVersion)
        {
            return new Natursystem
            {
                Navn = name,
                Version = ninVersion,
                Kode = new NatursystemKode
                {
                    Version = ninVersion,
                    KodeName = code,
                    Definisjon = code
                }
            };
        }

        private Hovedtypegruppe CreateHovedtypegruppe(GrunntyperRecord record,
                                                      NinVersion ninVersion,
                                                      Natursystem natursystem)
        {
            return CreateHovedtypegruppe(record.HovedtypegruppeNavn, record.HovedtypegruppeKode, ninVersion, natursystem);
        }

        private Hovedtypegruppe CreateHovedtypegruppe(KartleggingsenheterRecord record,
                                                      NinVersion ninVersion,
                                                      Natursystem natursystem)
        {
            return CreateHovedtypegruppe(record.HovedtypegruppeNavn, record.HovedtypegruppeKode, ninVersion, natursystem);
        }

        private Hovedtypegruppe CreateHovedtypegruppe(MiljovariablerRecord record,
                                                      NinVersion ninVersion,
                                                      Natursystem natursystem)
        {
            return CreateHovedtypegruppe(record.HovedtypegruppeNavn, record.HovedtypegruppeKode, ninVersion, natursystem);
        }

        private Hovedtypegruppe CreateHovedtypegruppe(string name, string code,
                                                      NinVersion ninVersion,
                                                      Natursystem natursystem)
        {
            return new Hovedtypegruppe
            {
                Version = ninVersion,
                Natursystem = natursystem,
                Navn = name,
                Kode = new HovedtypegruppeKode
                {
                    Version = ninVersion,
                    KodeName = code,
                    Definisjon = code.Replace(natursystem.Kode.KodeName, "").Trim()
                }
            };
        }

        private Hovedtype CreateHovedtype(GrunntyperRecord record,
                                          NinVersion ninVersion,
                                          Natursystem natursystem,
                                          Hovedtypegruppe hovedtypegruppe)
        {
            return CreateHovedtype(record.HovedtypeNavn, record.HovedtypeKode, ninVersion, natursystem, hovedtypegruppe);
        }

        private Hovedtype CreateHovedtype(KartleggingsenheterRecord record,
                                          NinVersion ninVersion,
                                          Natursystem natursystem,
                                          Hovedtypegruppe hovedtypegruppe)
        {
            return CreateHovedtype(record.HovedtypeNavn, record.HovedtypeKode, ninVersion, natursystem, hovedtypegruppe);
        }

        private Hovedtype CreateHovedtype(MiljovariablerRecord record,
                                          NinVersion ninVersion,
                                          Natursystem natursystem,
                                          Hovedtypegruppe hovedtypegruppe)
        {
            return CreateHovedtype(record.HovedtypeNavn, record.HovedtypeKode, ninVersion, natursystem, hovedtypegruppe);
        }

        private Hovedtype CreateHovedtype(string name, string code,
                                          NinVersion ninVersion,
                                          Natursystem natursystem,
                                          Hovedtypegruppe hovedtypegruppe)
        {
            return new Hovedtype
            {
                Version = ninVersion,
                Hovedtypegruppe = hovedtypegruppe,
                Navn = name,
                Kode = new HovedtypeKode
                {
                    Version = ninVersion,
                    KodeName = code,
                    Definisjon = code.Replace(natursystem.Kode.KodeName, "").Trim()
                }
            };
        }

        private Grunntype CreateGrunntype(string name, string code,
                                          NinVersion ninVersion,
                                          Natursystem natursystem,
                                          Hovedtype hovedtype)
        {
            return new Grunntype
            {
                Version = ninVersion,
                Hovedtype = hovedtype,
                Navn = name,
                Kode = new GrunntypeKode
                {
                    Version = ninVersion,
                    KodeName = code,
                    Definisjon = code.Replace(natursystem.Kode.KodeName, "").Trim()
                }
            };
        }

        private Kartleggingsenhet CreateKartleggingsenhet(KartleggingsenheterRecord record,
                                                          NinVersion ninVersion,
                                                          Hovedtype hovedtype)
        {
            return new Kartleggingsenhet
            {
                Version = ninVersion,
                Hovedtype = hovedtype,
                Definisjon = record.KartleggingsenhetDef,
                Kode = new KartleggingsenhetKode
                {
                    Version = ninVersion,
                    KodeName = record.KartleggingsenhetKode,
                    Definisjon = record.KartleggingsenhetDef
                },
                Malestokk = GetKartleggingsenhetmalestokk(record.KartleggingsenhetMalestokk)
            };
        }

        private Miljovariabel CreateMiljovariabel(MiljovariablerRecord record, NinVersion ninVersion, Hovedtype hovedtype)
        {
            return new Miljovariabel
            {
                Version = ninVersion,
                Hovedtype = hovedtype,
                Kode = new LKMKode
                {
                    Version = ninVersion,
                    Kode = record.MiljovariabelKode,
                    LkmKategori = GetLkmKategori("") // ToDo: where is it?!
                },
                Navn = record.MiljovariabelNavn
            };
        }

        private Trinn CreateTrinn(MiljovariablerRecord record, NinVersion ninVersion)
        {
            return new Trinn
            {
                Version = ninVersion,
                Navn = record.TrinnNavn,
                Kode = new TrinnKode
                {
                    Version = ninVersion,
                    KodeName = record.TrinnKode.Replace(" ", ""),
                    Kategori = KategoriEnum.Trinn
                }
            };
        }

        private Basistrinn CreateBasistrinn(string name, NinVersion ninVersion)
        {
            return new Basistrinn
            {
                Version = ninVersion,
                Navn = name
            };
        }

        private MalestokkEnum GetKartleggingsenhetmalestokk(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return MalestokkEnum.MalestokkInvalid;

            return NinEnumConverter.Convert<MalestokkEnum>(value).Value;
        }

        private LkmKategoriEnum GetLkmKategori(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return LkmKategoriEnum._null;

            return NinEnumConverter.Convert<LkmKategoriEnum>(value).Value;
        }

        #endregion
    }
}
