namespace NinKode.Database.Services.v2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.v1;
    using NinKode.Database.Model.v2;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Linq;
    using System.IO;
    using System.Text.Json;

    public class CodeV2Service : ICodeV2Service
    {
        //private const string IndexName = "NaturTypes/ByKode";
        //private const string RavenDbKeyName = "RavenDbNameV2";
        //private const string RavenDbKeyUrl = "RavenDbUrl";

        private List<NaturTypeV2> allNaturetypes;
        private string _sosiv2jsonFileStr;

        //private readonly DocumentStore _store;

        public List<NaturTypeV2> AllNaturetypes
        {
            get
            {
                if (allNaturetypes == null)
                {

                    if (File.Exists(_sosiv2jsonFileStr))
                    {
                        var text = File.ReadAllText(_sosiv2jsonFileStr);
                        allNaturetypes = JsonSerializer.Deserialize<List<NaturTypeV2>>(text);
                        return allNaturetypes;
                    }
                    /*
                    allNaturetypes = new List<NaturTypeV2>();
                    using (var session = _store.OpenSession())
                    {
                        var query = session.Query<NaturTypeV2>(IndexName);
                        using (var enumerator = session.Advanced.Stream(query))
                        {
                            while (enumerator.MoveNext())
                            {
                                allNaturetypes.Add(enumerator.Current?.Document);
                            }
                        }
                    }
                    string jsonString = JsonSerializer.Serialize(allNaturetypes.ToArray());
                    System.IO.File.WriteAllText(_sosiv2jsonFileStr, jsonString);
                    */
                }
                return allNaturetypes;
            }
        }

        public CodeV2Service(IConfiguration configuration)
        {
            //var dbName = configuration.GetValue(RavenDbKeyName, "SOSINiNv2");
            //var dbUrl = configuration.GetValue("RavenDbUrl", "http://localhost:8080/");
            _sosiv2jsonFileStr = configuration.GetValue("SOSINiNv2Json", "");
            /*
            if (string.IsNullOrWhiteSpace(dbName)) throw new Exception($"Missing \"{RavenDbKeyName}\"");
            if (string.IsNullOrWhiteSpace(dbUrl)) throw new Exception($"Missing \"{RavenDbKeyUrl}\"");

            _store = new DocumentStore
            {
                DefaultDatabase = dbName,
                Url = dbUrl
            };
            _store.Initialize(true);

            var index = _store.DatabaseCommands.GetIndex(IndexName);

            if (index != null) return;

            _store.DatabaseCommands.PutIndex(IndexName,
                new IndexDefinition
                {
                    Map = "from doc in docs.NaturTypes\nselect new\n{\n\tKode = doc.Kode\n}"
                }
            );
            */
        }

        public IEnumerable<Codes> GetAll(NiNDbContext dbContext, string host, string version = "", bool tree = false)
        {
            /*
            var all = new List<Codes>();
            using (var session = _store.OpenSession())
            {
                
                var query = session.Query<NaturTypeV2>(IndexName);
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return CreateCodesByNaturtype(enumerator.Current?.Document, host);
                    }
                }
            }
            String jsonString = JsonSerializer.Serialize(all.ToArray());
            System.IO.File.WriteAllText(_sosiv2jsonFileStr, jsonString);
            */
            return null;
        }

        public Codes GetByKode(NiNDbContext dbContext, string id, string host, string version = "")
        {
            if (string.IsNullOrEmpty(id)) return null;
            /*
            id = id.Replace("_", " ");

            using (var session = _store.OpenSession())
            {
                var query = session.Query<NaturTypeV2>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        return CreateCodesByNaturtype(enumerator.Current?.Document, host);
                    }
                }
            }
            */
            return null;
        }

        public Codes GetCode(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            id = id.Replace("_", " ");
            var result = AllNaturetypes.Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (result == null) return null;
            return CreateCodeByNaturtype(result);
            //using (var session = _store.OpenSession())
            //{
            //    var query = session.Query<NaturTypeV2>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            return CreateCodeByNaturtype(enumerator.Current?.Document);
            //        }
            //    }
            //}

            //return null;
        }

        #region private methods

        private static Codes CreateCodeByNaturtype(NaturTypeV2 naturType)
        {
            if (naturType == null) return null;

            return new Codes
            {
                Navn = naturType.Navn,
                Kategori = naturType.Kategori,
                Kode = new AllCodesCode
                {
                    Id = naturType.Kode,
                    Definition = $"{RemoveNaFromKode(naturType.Kode)}"
                },
                ElementKode = naturType.ElementKode,
                UnderordnetKoder = naturType.UnderordnetKoder == null ? null : CreateCodesByNaturtype(naturType.UnderordnetKoder, "").ToArray()
            };
        }

        private static string RemoveNaFromKode(string kode)
        {
            if (!kode.StartsWith("NA ")) return kode;

            return kode.Substring("NA ".Length);
        }

        private static Codes CreateCodesByNaturtype(NaturTypeV2 naturType, string host)
        {
            if (naturType == null) return null;

            return new Codes
            {
                Navn = naturType.Navn,
                Kategori = naturType.Kategori,
                Kode = new AllCodesCode
                {
                    Id = naturType.Kode,
                    Definition = $"{host}{naturType.Kode.Replace(" ", "_")}"
                },
                ElementKode = naturType.ElementKode,
                OverordnetKode = new AllCodesCode
                {
                    Id = naturType.OverordnetKode,
                    Definition = !string.IsNullOrEmpty(naturType.OverordnetKode) ? $"{host}{naturType.OverordnetKode.Replace(" ", "_")}" : ""
                },
                UnderordnetKoder = naturType.UnderordnetKoder == null ? null : CreateCodesByNaturtype(naturType.UnderordnetKoder, host).ToArray()
            };
        }

        private static IEnumerable<AllCodesCode> CreateCodesByNaturtype(string[] koder, string host)
        {
            if (koder == null) yield break;

            var codeList = new List<string>();
            codeList.AddRange(koder);

            foreach (var code in codeList.OrderByList<string>())
            {
                yield return new AllCodesCode
                {
                    Id = code,
                    Definition = $"{host}{code.Replace(" ", "_")}"
                };
            }
        }

        #endregion
    }
}
