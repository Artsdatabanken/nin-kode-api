namespace NinKode.Database
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using Microsoft.Extensions.Configuration;
    using NinKode.Database.Model.common;
    using NinKode.Database.Services.v22;
    using Raven.Abstractions.Data;
    using Raven.Abstractions.Indexing;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Json.Linq;

    internal class Program
    {
        private const bool NoLog = true;
        private static IDictionary<string, IList<string>> _keyMapping;
        private static IConfiguration _configuration;

        private const string PrimaryIndex = "Raven/DocumentsByEntityName";
        private static string _dbUrl = "http://localhost:8080/";
        private static string _dbName = "SOSINiNv2.2";

        public static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Log2Console("\nUsage:", true);
                Log2Console("------", true);
                Log2Console("NinKode.Database.exe {param} [{options}]", true);
                Log2Console("\nPossible parameters....", true);
                Log2Console("\timport", true);
                Log2Console("\tdelete", true);
                Log2Console("\tconvert", true);
                Log2Console("\reset", true);
                Log2Console("\nOptions....", true);
                Log2Console("\n\t--url <url-to-raven>\n\t--name <ravendb name>", true);

                return;
            }

            if (args.Length > 1)
            {
                for (var i = 1; i < args.Length; i++)
                {
                    if (args[i].Equals("--url", StringComparison.OrdinalIgnoreCase) && args.Length >= (i + 1))
                    {
                        _dbUrl = args[++i];
                    }
                    else if (args[i].Equals("--name", StringComparison.OrdinalIgnoreCase) && args.Length >= (i + 1))
                    {
                        _dbName = args[++i];
                    }
                }
            }

            var config = new ConfigurationBuilder().AddEnvironmentVariables();
            _configuration = config.Build();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            //if (true)
            //{
            //    //CopyToServer();
            //    //ConvertDocuments();
            //    //ConvertVariasjonerDocuments();
            //    stopWatch.Stop();

            //    Thread.Sleep(10);
            //    Log2Console($"\n{AnyDocuments()} documents", true);

            //    Log2Console($"\nCommand executed in {stopWatch.Elapsed.TotalSeconds:##.00} seconds", true);

            //    return;
            //}

            switch (args[0].ToLowerInvariant())
            {
                case "delete":
                    DeleteDocuments();
                    break;
                case "import":
                    ImportCsvIntoRavenDb();
                    break;
                case "convert":
                    ConvertDocuments();
                    ConvertVariasjonerDocuments();
                    break;
                case "reset":
                    ImportCsvIntoRavenDb();
                    ConvertDocuments();
                    ConvertVariasjonerDocuments();
                    break;
            }
            stopWatch.Stop();

            Thread.Sleep(10);
            Log2Console($"\n{AnyDocuments()} documents", true);

            Log2Console($"\nCommand executed in {stopWatch.Elapsed.TotalSeconds:##.00} seconds", true);
        }

        private static void CopyToServer()
        {
            var store = GetStore();

            store.Initialize(true);

            var serverStore = new DocumentStore
            {
                Url = "http://it-webadb01.it.ntnu.no:8180",
                DefaultDatabase = _dbName
                //MaxNumberOfCachedRequests = 9999
            };
            serverStore.Initialize(true);


            using var session = store.OpenSession();
            var query = session.Query<object>(PrimaryIndex);
            using var enumerator = session.Advanced.Stream(query);
            var i = 0;
            using var bulkServer = serverStore.BulkInsert(null, null);
            while (enumerator.MoveNext())
            {
                var name = enumerator.Current.Key.StartsWith("Natur")
                    ? "Naturtypes"
                    : "Variasjons";
                Log2Console($"{i++:#0000}\t{name}\t{enumerator.Current.Key}", true);
                ////bulkServer.Store(enumerator.Current.Document, enumerator.Current.Key);
                bulkServer.Store(
                    RavenJObject.FromObject(enumerator.Current.Document),
                    RavenJObject.Parse($"{{'Raven-Entity-Name': '{name}'}}"),
                    enumerator.Current.Key);
            }
        }

        private static void ConvertVariasjonerDocuments()
        {
            Log2Console("Starting variasjoner-import", true);

            DeleteByIndex("Variasjons/ByKode");

            using var store = GetStore();
            using var session = store.OpenSession();

            RavenQueryStatistics stats;
            session.Query<object>(PrimaryIndex).Statistics(out stats).Take(0).ToArray();
            Log2Console($"{stats.TotalResults} documents", true);

            var importVariasjonService = new ImportVariasjonService(Log2Console);

            importVariasjonService.Import(store);
        }

        private static Miljovariabel GetMiljovariabler()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Substring(0, path.IndexOf("bin\\", StringComparison.Ordinal));
            path = Path.Combine(path, "db");

            foreach (var filename in Directory.EnumerateFiles(path))
            {
                if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) continue;
                var name = filename.Substring(filename.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                name = name.Substring(0, name.IndexOf(".json", StringComparison.OrdinalIgnoreCase));
                if (!name.Equals("miljovariabler_metadata", StringComparison.OrdinalIgnoreCase)) continue;

                return JsonSerializer.Deserialize<Miljovariabel>(File.ReadAllText(filename, Encoding.UTF8));
            }

            return null;
        }

        private static void ConvertDocuments()
        {
            DeleteByIndex("NaturTypes/ByKode");
            
            using var store = GetStore();
            using var session = store.OpenSession();

            //var query = session.Query<object>(PrimaryIndex).Take(9999).ToList();
            var count = 0;

            RavenQueryStatistics stats;
            session.Query<object>(PrimaryIndex).Statistics(out stats).Take(0).ToArray();
            Log2Console($"{stats.TotalResults} documents", true);

            //var test = session.Advanced.DocumentStore.DatabaseCommands.Query(PrimaryIndex,new IndexQuery
            //{
            //    k
            //})

            //Console.ReadLine();

            // Create root elements
            var importNaturtypeService = new ImportNaturtypeService(_configuration, Log2Console);

            importNaturtypeService.Import(store, GetMiljovariabler(), "NA");
            
            //var documents = session.Advanced.DocumentStore.DatabaseCommands.GetDocuments(count, 5, false);
            //while (documents.Length > 0)
            //{
            //    foreach (var document in documents)
            //    {
            //        var data = document.DataAsJson;
            //        Log2Console($"{++count:#0000} key: '{document.Key}' docId: '{data["docId"]}'", true);
            //    }
            //    return;

            //    documents = session.Advanced.DocumentStore.DatabaseCommands.GetDocuments(count, 100, true);
            //}

            //var docs = result.Results;

        }

        private static DocumentStore GetStore()
        {
            var store = new DocumentStore
            {
                Url = _dbUrl,
                DefaultDatabase = _dbName
                //MaxNumberOfCachedRequests = 9999
            };
            store.Initialize(true);
            
            return store;
        }

        private static int AnyDocuments()
        {
            Thread.Sleep(100);

            using var store = GetStore();
            using var session = store.OpenSession();
            
            session
                .Query<object>(PrimaryIndex)
                .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)));

            RavenQueryStatistics stats;
            session.Query<object>(PrimaryIndex).Statistics(out stats).Take(0).ToArray();

            return stats.TotalResults;
        }
        
        private static void DeleteByIndex(string indexName)
        {
            using var store = GetStore();
            if (!GetIndexes(store).Contains(indexName)) return;
            
            using var session = store.OpenSession();

            session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex(indexName, new IndexQuery());
            session.Advanced.DocumentStore.DatabaseCommands.DeleteIndex(indexName);
            session.SaveChanges();

            Log2Console($"Deleted {indexName} and documents", true);
            Thread.Sleep(200);
        }

        private static void DeleteDocuments()
        {
            Log2Console("Deleting files...", true);
            Thread.Sleep(100);

            using var store = GetStore();
            using var session = store.OpenSession();
            
            session
                .Query<object>(PrimaryIndex)
                .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(505)));

            RavenQueryStatistics stats;
            session.Query<object>(PrimaryIndex).Statistics(out stats).Take(0).ToArray();

            session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex(PrimaryIndex,
                new IndexQuery());
            foreach (var indexName in GetIndexes(store))
            {
                Log2Console($"Deleting index: {indexName}", true);
                session.Advanced.DocumentStore.DatabaseCommands.DeleteIndex(indexName);
            }
            session.SaveChanges();

            Log2Console($"Deleted {stats.TotalResults} documents", true);
        }

        private static IEnumerable<string> GetIndexes(IDocumentStore store)
        {
            return store.DatabaseCommands
                .GetIndexNames(0, int.MaxValue)
                .Where(x => !x.Equals(PrimaryIndex, StringComparison.OrdinalIgnoreCase));
        }

        private static void ImportCsvIntoRavenDb()
        {
            Log2Console($"Starting import", true);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            _keyMapping = new Dictionary<string, IList<string>>
            {
                {"0_type_Grunntype", new List<string> { "Nivå&Sammensatt_Kode", "Nivå&Hovedtype_kode"}},
                {"0_type_LKM hovedtypetrinn", new List<string> { "Trinn1&LKM kategori", "HT"}},
                {"0_type_NiN definisjon hovedtyper", new List<string> {"HTK"}},
                {"1_variasjon_Beskrivelsessystem", new List<string> {"Nivå 1 Kode&Nivå 2 kode"}},
                {"1_variasjon_Artssammensetning", new List<string> {"Sammensatt_kode|Besys1&Nivå 1 kode&Nivå 2 kode"}},
                {"1_variasjon_Geologisk sammensetning", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Landform", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Menneskeskapte objekt", new List<string> {"Nivå2_kode&Trinn&Nivå 3 kode|Sammensatt_kode"}},
                {"1_variasjon_Naturgitte objekt", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Regional naturvariasjon", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Romlig strukturvariasjon", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Terrengformvariasjon", new List<string> {"Sammensatt_kode"}},
                {"1_variasjon_Tilstandsvariasjon", new List<string> {"Sammensatt_kode"}},
                {"2_type_Kartleggingsenheter 1-2500", new List<string> {"Sammensatt_kode"}},
                {"2_type_Kartleggingsenheter 1-5000", new List<string> {"Sammensatt_kode", "Nivå|Hovedtype_kode"}},
                {"2_type_Kartleggingsenheter 1-10000", new List<string> {"Sammensatt_kode"}},
                {"2_type_Kartleggingsenheter 1-20000", new List<string> {"Sammensatt_kode"}}
            };

            // need to delete all files if there is documents in the database
            var docCount = AnyDocuments();
            if (docCount > 0)
            {
                Log2Console($"Database has {docCount} documents", true);
                DeleteDocuments();
            }

            using (var store = GetStore())
            {
                var path = Assembly.GetExecutingAssembly().Location;
                path = path.Substring(0, path.IndexOf("bin\\", StringComparison.Ordinal));
                path = Path.Combine(path, "db");

                var i = 0;
                foreach (var filename in Directory.EnumerateFiles(path))
                {
                    if (filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) continue;
                    if (true)
                    {
                        var name = filename.Substring(filename.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                        name = name.Substring(0, name.IndexOf(".csv", StringComparison.OrdinalIgnoreCase));

                        var docType = name;
                        //if (docType.StartsWith("0_type_")) docType = $"Naturtype_{docType.Substring("0_type_".Length)}".Replace(" ", "_");
                        //else if (docType.StartsWith("2_type_")) docType = $"Naturtype_{docType.Substring("2_type_".Length)}".Replace(" ", "_");
                        //else if (docType.StartsWith("1_variasjon_")) docType = $"Variasjon_{docType.Substring("1_variasjon_".Length)}".Replace(" ", "_");
                        var prefix = docType;
                        if (prefix.StartsWith("0_type_") || prefix.StartsWith("2_type_"))
                            prefix = $"_Naturtype_{prefix.Substring("2_type_".Length)}".Replace(" ", "_");
                        else if (prefix.StartsWith("1_variasjon_"))
                            prefix = $"_Variasjon_{prefix.Substring("1_variasjon_".Length)}".Replace(" ", "_");

                        if (docType.StartsWith("0_type_") || docType.StartsWith("2_type_")) docType = $"Naturtype_{i}";
                        else if (docType.StartsWith("1_variasjon_")) docType = $"Variasjon_{i}";
                        
                        ImportFileIntoRavenDb(store, filename, name, docType, prefix.Replace("-", "_"));
                    }

                    i++;
                }
            }

            stopWatch.Stop();

            docCount = AnyDocuments();
            Log2Console($"\nImported {docCount} documents", true);

            Log2Console($"\nImport done in {stopWatch.Elapsed.TotalSeconds:##.00} seconds", true);
        }

        private static void ImportFileIntoRavenDb(IDocumentStore store, string path, string name, string docType, string prefix)
        {
            //// ignore målestokk-files
            //if (name.StartsWith("1_", StringComparison.Ordinal)) return;

            Log2Console($"Importing {name} ({docType})", true);

            string[] keys = { "" };
            var elements = new Dictionary<string, IList<string>>();
            var lines = File.ReadAllLines(path);

            var separator = '\t';
            for (var i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    if (!NoLog) Log2Console($"{i:#0000}:\t{lines[i]}");
                    keys = lines[i].Split(separator);
                    continue;
                }

                var line = lines[i].Split(separator);
                var j = 0;
                foreach (var element in line)
                {
                    if (j < keys.Length)
                    {
                        if (elements.ContainsKey(keys[j]))
                        {
                            elements[keys[j]].Add(element);
                        }
                        else
                        {
                            elements.Add(keys[j], new List<string> {element});
                        }
                    }

                    j++;
                }

                if (!NoLog) Log2Console($"{i:#0000}:\t{lines[i]}");
            }

            if (!NoLog) Log2Console("done");

            using (var bulk = store.BulkInsert(null, new BulkInsertOptions{OverwriteExisting = false}))
            {
                var usedKeys = new Dictionary<string, string>();
                var firstElement = elements.FirstOrDefault();
                for (var i = 0; i < firstElement.Value.Count; ++i)
                {
                    var typePrefix = "";
                    var documentname = GetDocName(i, elements, _keyMapping[name]);// elements.ContainsKey(keyMapping[name]) ? elements[keyMapping[name]][i] : "";
                    var dictionary = new Dictionary<string, object>();
                    var t = 0;
                    while (usedKeys.ContainsKey(documentname))
                    {
                        // illegalkey
                        typePrefix = "_duplicate";
                        if (!documentname.Contains("_duplicate", StringComparison.OrdinalIgnoreCase))
                            documentname += "_duplicate";
                        else
                        {
                            if (!documentname.EndsWith("_duplicate", StringComparison.OrdinalIgnoreCase))
                            {
                                documentname = documentname.Substring(0, documentname.Length - 2);
                            }
                            documentname += $"_{t}";
                            t++;
                        }
                    }
                    usedKeys.Add(documentname, $"{i}");

                    dictionary.Add("docId", documentname);

                    var j = 0;
                    foreach (var element in elements)
                    {
                        //if (element.Key.Equals("sammensatt_kode", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(element.Value[i])) documentname = $"{name}/{element.Value[i]}";
                        ////else if (element.Key.Equals("sammensatt-kode", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(element.Value[i])) documentname = $"{name}/{element.Value[i]}";
                        //else if (element.Key.Equals("sammensatt kode", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(element.Value[i])) documentname = $"{name}/{element.Value[i]}";
                        //else if (element.Key.Equals("sammensat kode", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(element.Value[i])) documentname = $"{name}/{element.Value[i]}";
                        //if (element.Key.Equals("hovedtype_kode", StringComparison.OrdinalIgnoreCase)) documentname = $"{name}/{element.Value[i]}";
                        //else if (element.Key.Equals("hovedtype-kode", StringComparison.OrdinalIgnoreCase)) documentname = $"{name}/{element.Value[i]}";
                        //else if (element.Key.Equals("hovedtype kode", StringComparison.OrdinalIgnoreCase)) documentname = $"{name}/{element.Value[i]}";
                        ////else if (element.Key.Equals("trinn1", StringComparison.OrdinalIgnoreCase)) documentname = $"{name}/{element.Value[i]}";
                        var key = element.Key;
                        //if (string.IsNullOrEmpty(element.Key)) key = $"{Guid.NewGuid()}";
                        if (string.IsNullOrEmpty(element.Key)) key = $"col_{j}";
                        var value = i >= element.Value.Count ? "" : element.Value[i];
                        dictionary.Add(key, value);
                        j++;
                    }
                    dictionary.Add("DTG_Timestamp", DateTime.Now);
                    dictionary.Add("DTG_Ticks", DateTime.Now.Ticks);

                    var ravenDocument = RavenJObject.FromObject(dictionary);
                    if (string.IsNullOrEmpty(documentname))
                    {
                        var ravenMetadata = RavenJObject.Parse($"{{'Raven-Entity-Name': 'illegal{prefix}'}}");
                        bulk.Store(ravenDocument, ravenMetadata, $"{docType}/a_{Guid.NewGuid()}");
                    }
                    else
                    {
                        var ravenMetadata = RavenJObject.Parse($"{{'Raven-Entity-Name': '{prefix}{typePrefix}'}}");
                        bulk.Store(ravenDocument, ravenMetadata, $"{docType}/{documentname}");
                    }
                }
            }

            try
            {
                Log2Console($"Creating index: {prefix}", true);
                store.DatabaseCommands.PutIndex(
                    $"{prefix}/ByKode",
                    new IndexDefinition
                    {
                        Map = $"from doc in docs.{prefix}\nselect new\n{{\n\tdocId = doc.docId\n}}"

                    }
                );
            }
            catch (Exception e)
            {
                Log2Console(e, true);
                Log2Console("Press <enter> to continue...", true);
                Console.ReadLine();
            }
        }

        private static string GetDocName(int i, IDictionary<string, IList<string>> elements, IEnumerable<string> keys)
        {
            var name = "";
            foreach (var key in keys)
            {
                if (key.Contains("|"))
                {
                    var keyElements = key.Split('|', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyElement in keyElements)
                    {
                        if (keyElement.Contains("&"))
                        {
                            foreach (var subKey in keyElement.Split('&', StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (!elements.ContainsKey(subKey)) continue;
                                var separator = string.IsNullOrEmpty(name) ? "" : " ";
                                if (i >= elements[subKey].Count) continue;
                                name = $"{name}{separator}{elements[subKey][i]}".TrimEnd();
                            }
                            if (!string.IsNullOrEmpty(name)) return name.TrimEnd().Replace(" ", "_");
                        }
                        else
                        {
                            if (!elements.ContainsKey(keyElement)) continue;
                            var separator = string.IsNullOrEmpty(name) ? "" : " ";
                            if (i >= elements[keyElement].Count) continue;
                            name = $"{name}{separator}{elements[keyElement][i]}".TrimEnd();
                        }
                        if (!string.IsNullOrEmpty(name)) return name.TrimEnd().Replace(" ", "_");
                    }
                    if (!string.IsNullOrEmpty(name)) return name.TrimEnd().Replace(" ", "_");
                }
                else
                {
                    if (key.Contains("&"))
                    {
                        foreach (var subKey in key.Split('&', StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!elements.ContainsKey(subKey)) continue;
                            var separator = string.IsNullOrEmpty(name) ? "" : " ";
                            if (i >= elements[subKey].Count) continue;
                            if (string.IsNullOrEmpty(elements[subKey][i])) name = "";
                            else name = $"{name}{separator}{elements[subKey][i]}".TrimEnd();
                        }

                        if (!string.IsNullOrEmpty(name)) return name.TrimEnd().Replace(" ", "_");
                    }
                    else
                    {
                        if (!elements.ContainsKey(key)) continue;
                        if (i >= elements[key].Count) continue;
                        name = elements[key][i];
                        if (!string.IsNullOrEmpty(name)) return name.TrimEnd().Replace(" ", "_");
                    }
                }
            }

            return name.TrimEnd();
        }

        private static void Log2Console(string arg, bool force = false)
        {
            if (NoLog && !force) return;
            Console.WriteLine(arg);
        }

        private static void Log2Console(Exception exception, bool force = false)
        {
            if (NoLog && !force) return;
            Console.WriteLine(exception);
        }
    }
}
