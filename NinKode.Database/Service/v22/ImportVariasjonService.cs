namespace NinKode.Database.Service.v22
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
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
            ProcessLandform(ref variasjoner, session, indexes);
            ProcessMenneskeskapt(ref variasjoner, session, indexes);
            ProcessNaturgitt(ref variasjoner, session, indexes);
            ProcessRegional(ref variasjoner, session, indexes);
            ProcessRomlig(ref variasjoner, session, indexes);
            ProcessTerrengform(ref variasjoner, session, indexes);
            ProcessTilstand(ref variasjoner, session, indexes);

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

        private void ProcessTilstand(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Tilstandsvariasjon";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonTilstand>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonTilstand = enumerator.Current?.Document;
                if (variasjonTilstand == null) continue;

                if (variasjoner.ContainsKey(variasjonTilstand.SammensattKode)) continue;

                var parentKode = variasjonTilstand.Nivaa2KodeNy;
                if (string.IsNullOrEmpty(parentKode))
                {
                    variasjoner.Add(variasjonTilstand.SammensattKode, new VariasjonV22
                    {
                        Kode = variasjonTilstand.SammensattKode,
                        Navn = variasjonTilstand.Trinnbeskrivelse,
                        OverordnetKode = $"{variasjonTilstand.col_0}{variasjonTilstand.Kode}"
                    });
                    continue;
                }

                variasjoner.Add(variasjonTilstand.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonTilstand.SammensattKode,
                    Navn = variasjonTilstand.Trinnbeskrivelse,
                    OverordnetKode = parentKode
                });

                if (variasjoner.ContainsKey(parentKode)) continue;

                variasjoner.Add(parentKode, new VariasjonV22
                {
                    Kode = parentKode,
                    Navn = variasjonTilstand.Variabel,
                    OverordnetKode = $"{variasjonTilstand.col_0}{variasjonTilstand.Kode}"
                });
            }
        }

        private void ProcessTerrengform(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Terrengformvariasjon";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonTerrengform>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonTerrengform = enumerator.Current?.Document;
                if (variasjonTerrengform == null) continue;

                if (variasjoner.ContainsKey(variasjonTerrengform.SammensattKode)) continue;

                variasjoner.Add(variasjonTerrengform.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonTerrengform.SammensattKode,
                    Navn = variasjonTerrengform.Vaiabelnavn,
                    OverordnetKode = $"BeSys{variasjonTerrengform.col_0}"
                });
            }
        }

        private void ProcessRomlig(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Romlig_strukturvariasjon";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonRomlig>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonRomlig = enumerator.Current?.Document;
                if (variasjonRomlig == null) continue;

                if (variasjoner.ContainsKey(variasjonRomlig.SammensattKode)) continue;

                if (string.IsNullOrEmpty(variasjonRomlig.Trinn))
                {
                    variasjoner.Add(variasjonRomlig.SammensattKode, new VariasjonV22
                    {
                        Kode = variasjonRomlig.SammensattKode,
                        Navn = variasjonRomlig.Navn,
                        OverordnetKode = $"BeSys{variasjonRomlig.Kode}"
                    });
                    continue;
                }

                if (string.IsNullOrEmpty(variasjonRomlig.VariabelType) || variasjonRomlig.VariabelType.Equals("T", StringComparison.OrdinalIgnoreCase))
                {
                    variasjoner.Add(variasjonRomlig.SammensattKode, new VariasjonV22
                    {
                        Kode = variasjonRomlig.SammensattKode,
                        Navn = variasjonRomlig.Forklaring,
                        OverordnetKode = $"{variasjonRomlig.Kode}{variasjonRomlig.Kode2}"
                    });
                    continue;
                }

                var parentKode = $"BeSys{variasjonRomlig.Kode}";
                variasjoner.Add(variasjonRomlig.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonRomlig.SammensattKode,
                    Navn = variasjonRomlig.Navn,
                    OverordnetKode = parentKode
                });
            }
        }

        private void ProcessRegional(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Regional_naturvariasjon";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonRegional>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonRegional = enumerator.Current?.Document;
                if (variasjonRegional == null) continue;

                if (variasjoner.ContainsKey(variasjonRegional.SammensattKode)) continue;

                var parentKode = $"{variasjonRegional.col_0}{variasjonRegional.Kode}";
                variasjoner.Add(variasjonRegional.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonRegional.SammensattKode,
                    Navn = variasjonRegional.KlasseTrinnbetegnelse,
                    OverordnetKode = parentKode
                });

                if (variasjoner.ContainsKey(parentKode))
                {
                    var parent = variasjoner[parentKode];
                    if (!string.IsNullOrEmpty(parent.Navn)) continue;
                    variasjoner.Remove(parentKode);
                }

                variasjoner.Add(parentKode, new VariasjonV22
                {
                    Kode = parentKode,
                    Navn = variasjonRegional.col_1,
                    OverordnetKode = $"BeSys{variasjonRegional.col_0}"
                });
            }
        }

        private void ProcessNaturgitt(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Naturgitte_objekt";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonNaturgitt>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonNaturgitt = enumerator.Current?.Document;
                if (variasjonNaturgitt == null) continue;
                
                if (variasjoner.ContainsKey(variasjonNaturgitt.SammensattKode)) continue;

                var parentKode = variasjonNaturgitt.Nivaa2KodeNy;
                variasjoner.Add(variasjonNaturgitt.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonNaturgitt.SammensattKode,
                    Navn = variasjonNaturgitt.Verdi,
                    OverordnetKode = parentKode
                });

                if (variasjoner.ContainsKey(parentKode)) continue;

                var kode = $"{variasjonNaturgitt.Besys}{variasjonNaturgitt.Nivaa1kode}-{variasjonNaturgitt.col_3}";
                if (parentKode.Equals(kode))
                {
                    parentKode = $"{variasjonNaturgitt.Besys}{variasjonNaturgitt.Nivaa1kode}";
                }
                variasjoner.Add(kode, new VariasjonV22
                {
                    Kode = kode,
                    Navn = variasjonNaturgitt.Nivaa2Beskrivelse,
                    OverordnetKode = parentKode
                });

                if (variasjoner.ContainsKey(parentKode)) continue;

                parentKode = $"{variasjonNaturgitt.Besys}{variasjonNaturgitt.Nivaa1kode}";
                variasjoner.Add(parentKode, new VariasjonV22
                {
                    Kode = parentKode,
                    Navn = variasjonNaturgitt.Nivaa2Beskrivelse,
                    OverordnetKode = parentKode
                });

                if (variasjoner.ContainsKey(parentKode)) continue;

                variasjoner.Add(parentKode, new VariasjonV22
                {
                    Kode = parentKode,
                    Navn = variasjonNaturgitt.col_2,
                    OverordnetKode = parentKode
                });
            }
        }

        private void ProcessMenneskeskapt(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Menneskeskapte_objekt";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonMenneskeskapt>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonMenneskeskapt = enumerator.Current?.Document;
                if (variasjonMenneskeskapt == null) continue;

                if (string.IsNullOrEmpty(variasjonMenneskeskapt.Type))
                {
                    if (string.IsNullOrEmpty(variasjonMenneskeskapt.Navn))
                    {
                        variasjoner.Add(variasjonMenneskeskapt.SammensattKode, new VariasjonV22
                        {
                            Kode = variasjonMenneskeskapt.SammensattKode,
                            Navn = variasjonMenneskeskapt.Verdi,
                            OverordnetKode = $"{variasjonMenneskeskapt.col_0}{variasjonMenneskeskapt.Nivaa1kode}"
                        });
                    }
                    else
                    {
                        variasjoner.Add(variasjonMenneskeskapt.SammensattKode, new VariasjonV22
                        {
                            Kode = variasjonMenneskeskapt.SammensattKode,
                            Navn = variasjonMenneskeskapt.Navn,
                            OverordnetKode = variasjonMenneskeskapt.Nivaa2_Kode
                        });
                        if (variasjoner.ContainsKey(variasjonMenneskeskapt.Nivaa2_Kode)) continue;
                        variasjoner.Add(variasjonMenneskeskapt.Nivaa2_Kode, new VariasjonV22
                        {
                            Kode = variasjonMenneskeskapt.Nivaa2_Kode,
                            Navn = variasjonMenneskeskapt.col_5,
                            OverordnetKode = $"{variasjonMenneskeskapt.col_0}{variasjonMenneskeskapt.Nivaa1kode}"
                        });
                    }
                    continue;
                }

                if (variasjoner.ContainsKey(variasjonMenneskeskapt.SammensattKode)) continue;

                variasjoner.Add(variasjonMenneskeskapt.SammensattKode, new VariasjonV22
                {
                    Kode = variasjonMenneskeskapt.SammensattKode,
                    Navn = variasjonMenneskeskapt.Verdi,
                    OverordnetKode = variasjonMenneskeskapt.Nivaa2KodeNy
                });

                if (!variasjoner.ContainsKey(variasjonMenneskeskapt.Nivaa2KodeNy))
                {
                    variasjoner.Add(variasjonMenneskeskapt.Nivaa2KodeNy, new VariasjonV22
                    {
                        Kode = variasjonMenneskeskapt.Nivaa2KodeNy,
                        Navn = variasjonMenneskeskapt.Navn,
                        OverordnetKode = $"{variasjonMenneskeskapt.col_0}{variasjonMenneskeskapt.Nivaa1kode}-{variasjonMenneskeskapt.Nivaa2Kode}"
                    });
                }

                var kode = $"{variasjonMenneskeskapt.col_0}{variasjonMenneskeskapt.Nivaa1kode}-{variasjonMenneskeskapt.Nivaa2Kode}";
                if (variasjoner.ContainsKey(kode)) continue;
                variasjoner.Add(kode, new VariasjonV22
                {
                    Kode = kode,
                    Navn = variasjonMenneskeskapt.col_5,
                    OverordnetKode = $"{variasjonMenneskeskapt.col_0}{variasjonMenneskeskapt.Nivaa1kode}"
                });
            }
        }

        private void ProcessLandform(
            ref Dictionary<string, VariasjonV22> variasjoner,
            IDocumentSession session,
            IEnumerable<string> indexes)
        {
            var indexName = "_Variasjon_Landform";
            var index = indexes.FirstOrDefault(x => x.StartsWith(indexName, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(index)) return;

            Log2Console(index, true);

            //var options = new JsonSerializerOptions {WriteIndented = true};
            var query = session.Query<VariasjonLandform>(index);
            using var enumerator = session.Advanced.Stream(query);
            while (enumerator.MoveNext())
            {
                var variasjonGeo = enumerator.Current?.Document;
                if (variasjonGeo == null) continue;

                //Log2Console($"json: {JsonSerializer.Serialize(variasjonGeo, options)}", true);
                var parentKode = $"{variasjonGeo.col_0}{variasjonGeo.Nivaa1kode}";

                if (!variasjoner.ContainsKey(parentKode))
                {
                    var parent = new VariasjonV22
                    {
                        Kode = parentKode,
                        Navn = variasjonGeo.col_3,
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
