namespace NinKode.Database.Service.v22
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using NinKode.Database.Model.v22;
    using Raven.Abstractions.Data;
    using Raven.Abstractions.Extensions;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Json.Linq;

    public class ImportNaturtypeService
    {
        private const string PrimaryIndex = "Raven/DocumentsByEntityName";
        public readonly string _dbUrl;
        public readonly string _dbFromName;
        public readonly string _dbToName;
        public readonly Action<string, bool> _logCallback;
        private readonly CodeV22Service _codeV22Service;

        public ImportNaturtypeService(string dbUrl, string dbFromName, string dbToName, Action<string, bool> logCallback)
        {
            _dbUrl = dbUrl;
            _dbFromName = dbFromName;
            _dbToName = dbToName;
            _logCallback = logCallback;
            _codeV22Service = new CodeV22Service(_dbUrl, _dbToName);
        }

        public void Import(IDocumentStore store, string rootName)
        {
            var codeV21Service = new v21.CodeV21Service(_dbUrl, _dbFromName);
            var naKode = codeV21Service.GetByKode(rootName, _dbUrl);

            var rootNaturtype = new NaturTypeV22
            {
                DatabankId = -1,
                Navn = naKode.Navn,
                Kategori = naKode.Kategori,
                Kode = naKode.Kode.Id,
                ElementKode = naKode.ElementKode,
                OverordnetKode = ""
            };

            var naturTypes = new Dictionary<string, NaturTypeV22>
            {
                {
                    rootNaturtype.Kode,
                    rootNaturtype
                }
            };

            var children = new List<string>();
            using var bulk = store.BulkInsert(null, new BulkInsertOptions { OverwriteExisting = true });

            foreach (var code in naKode.UnderordnetKoder)
            {
                naKode = _codeV22Service.GetByKode(code.Id, _dbUrl);
                if (naKode == null)
                {
                    naKode = codeV21Service.GetByKode(code.Id, _dbUrl);
                    // add to current database
                    var missing = new NaturTypeV22
                    {
                        DatabankId = -1,
                        Navn = naKode.Navn,
                        Kategori = naKode.Kategori,
                        Kode = naKode.Kode.Id,
                        ElementKode = naKode.ElementKode,
                        OverordnetKode = naKode.OverordnetKode.Id
                    };
                    bulk.Store(
                        RavenJObject.FromObject(missing),
                        RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                        $"Naturtype/{missing.Kode.Replace(" ", "_")}"
                    );
                }
                var childNaturtype = new NaturTypeV22
                {
                    DatabankId = -1,
                    Navn = naKode.Navn,
                    Kategori = naKode.Kategori,
                    Kode = naKode.Kode.Id,
                    ElementKode = naKode.ElementKode,
                    OverordnetKode = rootNaturtype.Kode
                };
                children.Add(childNaturtype.Kode);

                naturTypes.Add($"{childNaturtype.Kode}", childNaturtype);

                bulk.Store(
                    RavenJObject.FromObject(childNaturtype),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{childNaturtype.Kode.Replace(" ", "_")}"
                );
            }

            rootNaturtype.UnderordnetKoder = children.ToArray();

            bulk.Store(
                RavenJObject.FromObject(rootNaturtype),
                RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                $"Naturtype/{rootNaturtype.Kode.Replace(" ", "_")}"
            );

            using var session = store.OpenSession();
            var indexes = GetIndexes(session.Advanced.DocumentStore).ToList();

            NaturtypeGrunntypeProcess(naturTypes, bulk, session, indexes);
            NinDefinisjonHovedtyperProcess(naturTypes, bulk, session, indexes);
            
            FixChildrenAndParents(store, naturTypes);

            NaturtypeKartlegging(ref naturTypes, bulk, session, indexes, "2500");
            NaturtypeKartlegging(ref naturTypes, bulk, session, indexes, "5000");
            NaturtypeKartlegging(ref naturTypes, bulk, session, indexes, "10000");
            NaturtypeKartlegging(ref naturTypes, bulk, session, indexes, "20000");

            //FixChildrenAndParents(store, naturTypes);
            FixEnvironment(ref naturTypes, bulk, session, indexes);
        }

        private void FixEnvironment(
            ref Dictionary<string, NaturTypeV22> naturTypes,
            BulkInsertOperation bulk,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Naturtype_LKM_hovedtypetrinn";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            var query = session.Query<HovedtypeTrinn>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var hovedtypeTrinn = enumerator.Current.Document;

                Log2Console($"docId: {hovedtypeTrinn.docId}");

                var parent = naturTypes.ContainsKey($"NA {hovedtypeTrinn.HT}")
                    ? naturTypes[$"NA {hovedtypeTrinn.HT}"]
                    : null;

                if (parent == null) continue;

                var trinn = new TrinnV22
                {
                    Kode = hovedtypeTrinn.Gradientkode,
                    Type = "Miljøvariabel",
                    //Navn = hovedtypeTrinn.HovedtypeNavn
                };

                if (string.IsNullOrEmpty(trinn.Navn))
                {
                    switch (trinn.Kode.ToUpper())
                    {
                        case "":
                        case "BK":
                        case "DD":
                        case "ER":
                        case "FK":
                        case "FR":
                        case "HS":
                        case "KO":
                        case "KT":
                        case "KY":
                        case "LK":
                        case "MF":
                        case "NG":
                        case "OM":
                        case "S3S":
                        case "SE":
                        case "SP":
                        case "SS":
                        case "SY":
                        case "TE":
                        case "TU":
                        case "VT":
                            trinn.Navn = $"ukjent kode ({trinn.Kode})";
                            break;

                        case "DL":
                            trinn.Navn = "Dybderelatert lyssvekking";
                            break;
                        case "DM":
                            trinn.Navn = "Dybderelatert miljøstabilisering";
                            break;
                        case "GS":
                            trinn.Navn = "Grottebetinget skjerming";
                            break;
                        case "HF":
                            trinn.Navn = "Helningsbetinget forstyrrelsesintensitet";
                            break;
                        case "HI":
                            trinn.Navn = "Hevdintensitet";
                            break;
                        case "HU":
                            trinn.Navn = "Humusinnhold";
                            break;
                        case "IF":
                            trinn.Navn = "Isbetinget forstyrrelse";
                            break;
                        case "IO":
                            trinn.Navn = "Innhold av organisk materiale";
                            break;
                        case "JV":
                            trinn.Navn = "Jordvarmeinnflytelse";
                            break;
                        case "KA":
                            trinn.Navn = "Kalkinnhold";
                            break;
                        case "KI":
                            trinn.Navn = "Kildevannspåvirkning";
                            break;
                        case "LA":
                            trinn.Navn = "Langsom primær suksesjon";
                            break;
                        case "OR":
                            trinn.Navn = "Overrisling";
                            break;
                        case "RU":
                            trinn.Navn = "Rasutsatthet";
                            break;
                        case "S1":
                            trinn.Navn = "Dominerende kornstørrelsesklasse";
                            break;
                        case "S3F":
                            trinn.Navn = "Finmaterialinnhold";
                            break;
                        case "SA":
                            trinn.Navn = "Marin salinitet";
                            break;
                        case "S3E":
                            trinn.Navn = "Erosjonsmotstand";
                            break;
                        case "SM":
                            trinn.Navn = "Størrelsesrelatert miljøvariabilitet (i vannsystemer)";
                            break;
                        case "SU":
                            trinn.Navn = "Skredutsatthet";
                            break;
                        case "SV":
                            trinn.Navn = "Snødekkebetinget vekstsesongreduksjon";
                            break;
                        case "TV":
                            trinn.Navn = "Tørrleggingsvarighet";
                            break;
                        case "UE":
                            trinn.Navn = "Uttørkingseksponering";
                            break;
                        case "UF":
                            trinn.Navn = "Uttørkingsfare";
                            break;
                        case "VF":
                            trinn.Navn = "Vannpåvirkningsintensitet";
                            break;
                        case "VI":
                            trinn.Navn = "Vindutsatthet";
                            break;
                        case "VM":
                            trinn.Navn = "Vannmetning";
                            break;
                        case "VR":
                            trinn.Navn = "Vannpåvirkningsregime";
                            break;
                        case "VS":
                            trinn.Navn = "Vannsprutintensitet";
                            break;
                        default:
                            trinn.Navn = $"ukjent kode: {trinn.Kode}";
                            break;
                    }
                }

                var listSubTrinn = new List<SubTrinnV22>();
                for (var i = 1; i < 6; i++)
                {
                    var subTrinn = new SubTrinnV22();
                    switch (i)
                    {
                        case 1:
                            subTrinn.Kode = hovedtypeTrinn.Trinn1;
                            subTrinn.Navn = hovedtypeTrinn.Trinn1Navn;
                            break;
                        case 2:
                            subTrinn.Kode = hovedtypeTrinn.Trinn2;
                            subTrinn.Navn = hovedtypeTrinn.Trinn2Navn;
                            break;
                        case 3:
                            subTrinn.Kode = hovedtypeTrinn.Trinn3;
                            subTrinn.Navn = hovedtypeTrinn.Trinn3Navn;
                            break;
                        case 4:
                            subTrinn.Kode = hovedtypeTrinn.Trinn4;
                            subTrinn.Navn = hovedtypeTrinn.Trinn4Navn;
                            break;
                        case 5:
                            subTrinn.Kode = hovedtypeTrinn.Trinn5;
                            subTrinn.Navn = hovedtypeTrinn.Trinn5Navn;
                            break;
                    }
                    if (string.IsNullOrEmpty(subTrinn.Kode)) continue;
                    listSubTrinn.Add(subTrinn);
                }

                if (listSubTrinn.Count > 0) trinn.Trinn = listSubTrinn.ToArray();

                if (parent.Trinn == null)
                {
                    parent.Trinn = new[] {trinn};
                }
                else if (parent.Trinn.Any(x => x.Kode.Equals(trinn.Kode, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                else
                {
                    var list = new List<TrinnV22>();
                    list.AddRange(parent.Trinn);
                    list.Add(trinn);
                    parent.Trinn = list.ToArray();
                }

                bulk.Store(
                    RavenJObject.FromObject(parent),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{parent.Kode.Replace(" ", "_")}"
                );
                Thread.Sleep(1);
            }
        }

        private void NaturtypeKartlegging(
            ref Dictionary<string, NaturTypeV22> naturTypes,
            BulkInsertOperation bulk,
            IDocumentSession session,
            IEnumerable<string> indexes,
            string malestokk)
        {
            var indexName = $"_Naturtype_Kartleggingsenheter_1_{malestokk}";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            var query = session.Query<Kartlegging>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var kartlegging = enumerator.Current.Document;
                //Log2Console($"json: {JsonSerializer.Serialize(kartlegging, options)}", true);

                Log2Console($"docId: {kartlegging.docId}");

                var naturtype = new NaturTypeV22
                {
                    DatabankId = -1,
                    Kategori = "Kartleggingsenhet",
                    Kode = $"{kartlegging.docId.Replace("_", " ")}",
                    OverordnetKode = $"{kartlegging.Nivaa} {kartlegging.HovedtypeKode}",
                    Malestokk = malestokk
                };
                switch (malestokk)
                {
                    case "2500":
                        naturtype.Navn = kartlegging.NavnKartleggingsenheter2500;
                        break;
                    case "5000":
                        naturtype.Navn = kartlegging.NavnKartleggingsenheter5000;
                        break;
                    case "10000":
                        naturtype.Navn = kartlegging.NavnKartleggingsenheter10000;
                        break;
                    case "20000":
                        naturtype.Navn = kartlegging.NavnKartleggingsenheter20000;
                        break;
                }
                //Log2Console($"Hovedtype: {JsonSerializer.Serialize(naturtype, options)}", true);

                if (naturTypes.ContainsKey(naturtype.Kode))
                {
                    var old = naturTypes[naturtype.Kode];
                    naturtype.Navn = old.Navn;
                    naturtype.Kategori = old.Kategori;
                    naturtype.ElementKode = old.ElementKode;
                    naturtype.OverordnetKode = old.OverordnetKode;
                    naturtype.UnderordnetKoder = old.UnderordnetKoder;
                    naturtype.Kartleggingsenheter = old.Kartleggingsenheter;
                    naturtype.Malestokk = null;

                    naturTypes.Remove(naturtype.Kode);
                    naturTypes.Add(naturtype.Kode, naturtype);
                }

                if (naturTypes.ContainsKey(naturtype.OverordnetKode))
                {
                    var old = naturTypes[naturtype.OverordnetKode];

                    if (old.Kategori.Equals("Hovedtype", StringComparison.OrdinalIgnoreCase))
                    {
                        naturTypes.Remove(old.Kode);

                        if (old.Kartleggingsenheter == null)
                            old.Kartleggingsenheter = new Dictionary<string, string[]>();

                        if (!old.Kartleggingsenheter.ContainsKey(malestokk))
                        {
                            old.Kartleggingsenheter.Add(malestokk, new[] {naturtype.Kode});
                        }
                        else
                        {
                            var oldEnheter = old.Kartleggingsenheter[malestokk];
                            old.Kartleggingsenheter.Remove(malestokk);

                            var newEnheter = new List<string>();
                            newEnheter.AddRange(oldEnheter);
                            newEnheter.Add(naturtype.Kode);
                            old.Kartleggingsenheter.Add(malestokk, newEnheter.ToArray());
                        }

                        naturTypes.Add(old.Kode, old);

                        bulk.Store(
                            RavenJObject.FromObject(old),
                            RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                            $"Naturtype/{old.Kode.Replace(" ", "_")}"
                        );
                    }
                }
                else if (!naturTypes.ContainsKey(naturtype.Kode))
                {
                    naturTypes.Add(naturtype.Kode, naturtype);
                }

                bulk.Store(
                    RavenJObject.FromObject(naturtype),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{naturtype.Kode.Replace(" ", "_")}"
                );
                Thread.Sleep(1);
            }
        }

        private void NinDefinisjonHovedtyperProcess(
            IDictionary<string, NaturTypeV22> naturTypes,
            BulkInsertOperation bulk,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Naturtype_NiN_definisjon_hovedtyper";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            var query = session.Query<Hovedtype>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var hovedtype = enumerator.Current.Document;
                //Log2Console($"json: {JsonSerializer.Serialize(naturtypeGrunntype, options)}", true);

                Log2Console($"docId: {hovedtype.docId}");

                var naturtype = new NaturTypeV22
                {
                    DatabankId = -1,
                    Navn = hovedtype.Navn,
                    Kategori = "Hovedtype",
                    Kode = $"NA {hovedtype.docId.Replace("_", " ")}",
                    OverordnetKode = $"NA {hovedtype.docId.Substring(0, 1)}"
                };
                //Log2Console($"Hovedtype: {JsonSerializer.Serialize(naturtype, options)}", true);

                if (naturTypes.ContainsKey(naturtype.Kode))
                {
                    var old = naturTypes[naturtype.Kode];
                    naturtype.Kartleggingsenheter = old.Kartleggingsenheter;
                    naturtype.Malestokk = old.Malestokk;
                    naturTypes.Remove(old.Kode);
                }
                naturTypes.Add(naturtype.Kode, naturtype);

                bulk.Store(
                    RavenJObject.FromObject(naturtype),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{naturtype.Kode.Replace(" ", "_")}"
                );

            }
        }

        private void NaturtypeGrunntypeProcess(
            IDictionary<string, NaturTypeV22> naturTypes,
            BulkInsertOperation bulk,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Naturtype_Grunntype";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;
            
            Log2Console(index, true);

            var query = session.Query<NaturtypeGrunntype>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var naturtypeGrunntype = enumerator.Current.Document;
                //Log2Console($"json: {JsonSerializer.Serialize(naturtypeGrunntype, options)}", true);

                Log2Console($"docId: {naturtypeGrunntype.docId}");

                var naturtype = new NaturTypeV22
                {
                    DatabankId = -1,
                    Navn = naturtypeGrunntype.NavnGrunntypeLong,
                    Kategori = "Grunntype",
                    Kode = naturtypeGrunntype.docId.Replace("_", " "),
                    OverordnetKode = $"{naturtypeGrunntype.Nivaa} {naturtypeGrunntype.HovedtypeKode}".Trim()
                };
                //Log2Console($"Hovedtype: {JsonSerializer.Serialize(naturtype, options)}", true);

                naturTypes.Add($"{naturtype.Kode}", naturtype);

                bulk.Store(
                    RavenJObject.FromObject(naturtype),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{naturtype.Kode.Replace(" ", "_")}"
                );

            }
        }

        private void FixChildrenAndParents(IDocumentStore store, IDictionary<string, NaturTypeV22> naturTypes)
        {
            using var bulk = store.BulkInsert(null, new BulkInsertOptions {OverwriteExisting = true});

            foreach (var element in naturTypes)
            {
                if (false)
                {
                    if (!element.Value.Kategori.Equals("Hovedtypegruppe", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!element.Value.Kategori.Equals("Naturmangfoldnivå", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!element.Value.Kategori.Equals("Grunntype", StringComparison.OrdinalIgnoreCase))
                                continue;
                        }
                    }
                }

                var parent = naturTypes.ContainsKey(element.Value.OverordnetKode)
                    ? naturTypes[element.Value.OverordnetKode]
                    : null;
                
                if (parent == null) continue;

                if (false){
                    if (!parent.Kategori.Equals("Hovedtypegruppe", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!parent.Kategori.Equals("Naturmangfoldnivå", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!parent.Kategori.Equals("Grunntype", StringComparison.OrdinalIgnoreCase)) continue;
                        }
                    }
                }
                
                if (parent.UnderordnetKoder == null)
                {
                    parent.UnderordnetKoder = new[] {element.Value.Kode};
                }
                else
                {
                    //var kode = parent.UnderordnetKoder.FirstOrDefault(x =>
                    //    x.Equals(element.Value.OverordnetKode, StringComparison.OrdinalIgnoreCase));
                    var kode = parent.UnderordnetKoder.FirstOrDefault(x =>
                        x.Equals(element.Value.Kode, StringComparison.OrdinalIgnoreCase));
                    if (kode != null) continue;

                    var array = new List<string>();
                    array.AddRange(parent.UnderordnetKoder);
                    array.Add(element.Value.Kode);
                    parent.UnderordnetKoder = array.ToArray();
                }

                bulk.Store(
                    RavenJObject.FromObject(parent),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Naturtypes'}"),
                    $"Naturtype/{parent.Kode.Replace(" ", "_")}"
                );
                Thread.Sleep(1);
            }
        }

        private static IEnumerable<string> GetIndexes(IDocumentStore store)
        {
            return store.DatabaseCommands
                .GetIndexNames(0, int.MaxValue)
                .Where(x => !x.Equals(PrimaryIndex, StringComparison.OrdinalIgnoreCase));
        }

        private void Log2Console(string arg, bool force = false)
        {
            _logCallback(arg, force);
        }
    }
}
