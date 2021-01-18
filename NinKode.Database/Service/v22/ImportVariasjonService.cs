namespace NinKode.Database.Service.v22
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.common;
    using NinKode.Database.Model.v22;
    using Raven.Abstractions.Data;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Json.Linq;

    public class ImportVariasjonService
    {
        private const string PrimaryIndex = "Raven/DocumentsByEntityName";
        public readonly string _dbUrl;
        public readonly Action<string, bool> _logCallback;
        private readonly VarietyV22Service _varietyV22Service;

        public ImportVariasjonService(string dbUrl, string dbName, Action<string, bool> logCallback)
        {
            _dbUrl = dbUrl;
            _logCallback = logCallback;
            _varietyV22Service = new VarietyV22Service(_dbUrl, dbName);
        }

        public void Import(IDocumentStore store)
        {
            var variasjoner = new Dictionary<string, VariasjonV22>();
            
            using var session = store.OpenSession();
            var indexes = GetIndexes(session.Advanced.DocumentStore).ToList();

            ProcessBeskrivelsessystem(ref variasjoner, session, indexes);
            ProcessArtssammensetning(ref variasjoner, session, indexes);
            ProcessGeologisksammensetning(ref variasjoner, session, indexes);

            FixChildrenAndParents(variasjoner);

            SaveVariasjoner(store, variasjoner);
        }

        #region private methods
        
        private void ProcessBeskrivelsessystem(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Beskrivelsessystem";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            var query = session.Query<BeskrivelsesSystem>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var beskrivelsesSystem = enumerator.Current?.Document;
                if (beskrivelsesSystem == null) continue;

                VariasjonV22 variasjon;
                if (variasjoner.ContainsKey(beskrivelsesSystem.Kode))
                {
                    if (variasjoner.ContainsKey(beskrivelsesSystem.Nivaa2Kode)) continue;
                    
                    variasjon = new VariasjonV22
                    {
                        Kode = beskrivelsesSystem.Nivaa2Kode,
                        Navn = beskrivelsesSystem.col3,
                        OverordnetKode = beskrivelsesSystem.Kode
                    };
                }
                else
                {
                    variasjon = new VariasjonV22
                    {
                        Kode = beskrivelsesSystem.Kode,
                        Navn = beskrivelsesSystem.Variabelgruppe,
                        OverordnetKode = beskrivelsesSystem.OverordnetKode,
                        UnderordnetKoder = beskrivelsesSystem.UnderordnetKoder
                    };

                    if (!beskrivelsesSystem.Nivaa2Kode.Equals("BeSys1,BeSys2,BeSys3,BeSys4,BeSys5,BeSys6,BeSys7,BeSys8,BeSys9")
                        && !variasjoner.ContainsKey(beskrivelsesSystem.Nivaa2Kode))
                    {
                        var child = new VariasjonV22
                        {
                            Kode = beskrivelsesSystem.Nivaa2Kode,
                            Navn = beskrivelsesSystem.col3,
                            OverordnetKode = beskrivelsesSystem.Kode
                        };
                        variasjoner.Add($"{child.Kode}", child);
                    }
                }

                variasjoner.Add($"{variasjon.Kode}", variasjon);
            }
        }

        private void ProcessGeologisksammensetning(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Geologisk_sammensetning";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonGeo>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonGeo = enumerator.Current?.Document;
                if (variasjonGeo == null) continue;

                if (variasjonGeo.Variabeltype.Equals("M", StringComparison.OrdinalIgnoreCase))
                {
                    variasjoner.Add(variasjonGeo.Nivaa2KodeNy, new VariasjonV22
                    {
                        Kode = variasjonGeo.Nivaa2KodeNy,
                        Navn = variasjonGeo.col_5,
                        OverordnetKode = $"{variasjonGeo.col_0}{variasjonGeo.Nivaa1kode}"
                    });
                    continue;
                }

                //Log2Console($"json: {JsonSerializer.Serialize(variasjonGeo, options)}", true);
                var parentKode = $"{variasjonGeo.Nivaa2KodeNy}";
                
                if (!variasjoner.ContainsKey(parentKode))
                {
                    var parent = new VariasjonV22
                    {
                        Kode = parentKode,
                        Navn = variasjonGeo.col_5,
                        OverordnetKode = $"{variasjonGeo.col_0}{variasjonGeo.Nivaa1kode}"
                    };
                    variasjoner.Add($"{parent.Kode}", parent);
                }

                if (variasjoner.ContainsKey(variasjonGeo.SammensattKode)) continue;

                var variasjon = new VariasjonV22
                {
                    Kode = $"{variasjonGeo.SammensattKode}",
                    Navn = $"{variasjonGeo.Navn}",
                    Type = $"{variasjonGeo.Variabeltype}",
                    OverordnetKode = parentKode
                };

                variasjoner.Add($"{variasjon.Kode}", variasjon);
            }
        }

        private void ProcessArtssammensetning(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            //variasjoner.Add(
            //    "1AR-A-0",
            //    new VariasjonV22 { Kode = "1AR-A-0", Navn = "Dominansutforming", OverordnetKode = "1AR" }
            //);
            variasjoner.Add(
                "1AR-A-0",
                new VariasjonV22 { Kode = "1AR-A-0", Navn = "Dominansutforming", OverordnetKode = "1AR" }
            );

            var indexName = "_Variasjon_Artssammensetning";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonArt>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonArt = enumerator.Current?.Document;
                if (variasjonArt == null) continue;

                //if (string.IsNullOrEmpty(variasjonArt.Trinn) || string.IsNullOrEmpty(variasjonArt.Type)) continue;
                if (string.IsNullOrEmpty(variasjonArt.Trinn)) continue;

                //Log2Console($"json: {JsonSerializer.Serialize(variasjonArt, options)}", true);
                var parentKode = $"{variasjonArt.Nivaa2KodeNy}";
                //if (parentKode.Length > variasjonArt.SammensattKode.Length || parentKode.Equals(variasjonArt.SammensattKode))
                //{
                //    parentKode = $"{variasjonArt.Besys1}{variasjonArt.Nivaa1kode}";
                //}
                //parentKode = $"{variasjonArt.Besys1}{variasjonArt.Nivaa2kode}";

                //parentKode = $"{variasjonArt.Besys1}{variasjonArt.Nivaa2KodeNy}";

                if (!variasjoner.ContainsKey(parentKode))
                {
                    var parent = new VariasjonV22
                    {
                        Kode = parentKode,
                        Navn = variasjonArt.Nivaa2beskrivelse,
                        OverordnetKode = $"{variasjonArt.Besys1}{variasjonArt.Nivaa1kode}"
                    };
                    variasjoner.Add($"{parent.Kode}", parent);
                }

                if (variasjoner.ContainsKey(variasjonArt.SammensattKode)) continue;

                var variasjon = new VariasjonV22
                {
                    Kode = $"{variasjonArt.SammensattKode}",
                    Navn = $"{variasjonArt.NavnForklaring}",
                    Type = $"{variasjonArt.Tags}",
                    OverordnetKode = parentKode
                };

                variasjoner.Add($"{variasjon.Kode}", variasjon);
            }
        }

        private static void FixChildrenAndParents(IDictionary<string, VariasjonV22> variasjoner)
        {
            foreach (var element in variasjoner)
            {
                if (string.IsNullOrEmpty(element.Value.OverordnetKode)) continue;

                var parent = variasjoner.ContainsKey(element.Value.OverordnetKode)
                    ? variasjoner[element.Value.OverordnetKode]
                    : null;
                
                if (parent == null) continue;

                if (parent.UnderordnetKoder == null)
                {
                    parent.UnderordnetKoder = new[] {element.Value.Kode};
                }
                else
                {
                    var kode = parent.UnderordnetKoder.FirstOrDefault(x =>
                        x.Equals(element.Value.Kode, StringComparison.OrdinalIgnoreCase));
                    if (kode != null) continue;
                    var array = new List<string>();
                    array.AddRange(parent.UnderordnetKoder);
                    array.Add(element.Value.Kode);
                    parent.UnderordnetKoder = array.ToArray();
                }
            }
        }

        private static void SaveVariasjoner(IDocumentStore store, IDictionary<string, VariasjonV22> variasjoner)
        {
            using var bulk = store.BulkInsert(null, new BulkInsertOptions { OverwriteExisting = true });

            foreach (var variasjon in variasjoner)
            {
                bulk.Store(
                    RavenJObject.FromObject(variasjon.Value),
                    RavenJObject.Parse("{'Raven-Entity-Name': 'Variasjons'}"),
                    $"Variasjon/{variasjon.Value.Kode.Replace(" ", "_")}"
                );
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

        #endregion
    }
}
