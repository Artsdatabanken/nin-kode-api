﻿namespace NinKode.Database.Services.v21b
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.v2;
    using NinKode.Database.Model.v21b;
    using Raven.Abstractions.Indexing;

    public class CodeV21BService : ICodeV21BService
    {
        //private const string IndexName = "NaturTypes/ByKode";
        //private const string RavenDbKeyName = "RavenDbNameV21B";
        //private const string RavenDbKeyUrl = "RavenDbUrl";

        private List<NaturTypeV21B> allNaturetypes;
        private string _sosiv21BjsonFileStr;

        //private readonly DocumentStore _store;

        public List<NaturTypeV21B> AllNaturetypes
        {
            get
            {
                if (allNaturetypes == null)
                {

                    if (File.Exists(_sosiv21BjsonFileStr))
                    {
                        var text = File.ReadAllText(_sosiv21BjsonFileStr);
                        allNaturetypes = JsonSerializer.Deserialize<List<NaturTypeV21B>>(text);
                        return allNaturetypes;
                    }

                    //allNaturetypes = new List<NaturTypeV21B>();
                    //using (var session = _store.OpenSession())
                    //{
                    //    var query = session.Query<NaturTypeV21B>(IndexName);
                    //    using (var enumerator = session.Advanced.Stream(query))
                    //    {
                    //        while (enumerator.MoveNext())
                    //        {
                    //            allNaturetypes.Add(enumerator.Current?.Document);
                    //        }
                    //    }
                    //}
                    //string jsonString = JsonSerializer.Serialize(allNaturetypes.ToArray());
                    //System.IO.File.WriteAllText(_sosiv21BjsonFileStr, jsonString);
                }

                return allNaturetypes;
            }
        }

        public CodeV21BService(IConfiguration configuration)
        {
            //var dbName = configuration.GetValue(RavenDbKeyName, "SOSINiNv2.0b");
            //var dbUrl = configuration.GetValue("RavenDbUrl", "http://localhost:8080/");
            _sosiv21BjsonFileStr = configuration.GetValue("SOSINiNv21BJson", "CsvFiles\\SOSINiNv21B.json");

            //if (string.IsNullOrWhiteSpace(dbName)) throw new Exception($"Missing \"{RavenDbKeyName}\"");
            //if (string.IsNullOrWhiteSpace(dbUrl)) throw new Exception($"Missing \"{RavenDbKeyUrl}\"");

            //_store = new DocumentStore
            //{
            //    DefaultDatabase = dbName,
            //    Url = dbUrl
            //};
            //_store.Initialize(true);

            //var index = _store.DatabaseCommands.GetIndex(IndexName);

            //if (index != null) return;

            //_store.DatabaseCommands.PutIndex(IndexName,
            //    new IndexDefinition
            //    {
            //        Map = "from doc in docs.NaturTypes\nselect new\n{\n\tKode = doc.Kode\n}"
            //    }
            //);
        }

        public IEnumerable<Codes> GetAll(NiNDbContext dbContext, string host, string version = "", bool tree = false)
        {
            //using (var session = _store.OpenSession())
            //{
            //    var query = session.Query<NaturTypeV21B>(IndexName);
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            yield return CreateCodesByNaturtype(enumerator.Current?.Document, host);
            //        }
            //    }
            //}
            return null;
        }

        public Codes GetByKode(NiNDbContext dbContext, string id, string host, string version = "")
        {
            if (string.IsNullOrEmpty(id)) return null;

            id = id.Replace("_", " ");

            //using (var session = _store.OpenSession())
            //{
            //    var query = session.Query<NaturTypeV21B>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            return CreateCodesByNaturtype(enumerator.Current?.Document, host);
            //        }
            //    }
            //}

            return null;
        }

        public Codes GetCode(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            id = id.Replace("_", " ");
            var result = AllNaturetypes.Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (result == null) return null;
            return CreateCodeByNaturtype(result);
        }

        #region private methods

        private static Codes CreateCodeByNaturtype(NaturTypeV21B naturType)
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
                UnderordnetKoder = naturType.UnderordnetKoder == null ? null : CreateCodesByNaturtype(naturType.UnderordnetKoder, "").ToArray(),
                Kartleggingsenheter = naturType.Kartleggingsenheter == null ? null : CreateKartleggingsenheter(naturType.Kartleggingsenheter, ""),
                Miljovariabler = naturType.Trinn == null ? null : CreateTrinn(naturType.Trinn).ToArray()
            };
        }

        private static string RemoveNaFromKode(string kode)
        {
            if (!kode.StartsWith("NA ")) return kode;

            return kode.Substring("NA ".Length);
        }

        private static Codes CreateCodesByNaturtype(NaturTypeV21B naturType, string host)
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
                UnderordnetKoder = naturType.UnderordnetKoder == null ? null : CreateCodesByNaturtype(naturType.UnderordnetKoder, host).ToArray(),
                Kartleggingsenheter = naturType.Kartleggingsenheter == null ? null : CreateKartleggingsenheter(naturType.Kartleggingsenheter, host),
                Miljovariabler = naturType.Trinn == null ? null : CreateTrinn(naturType.Trinn).ToArray()
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

        private static Dictionary<string, AllCodesCode[]> CreateKartleggingsenheter(IDictionary<string, string[]> kartleggingsenheter, string host)
        {
            if (kartleggingsenheter == null) return null;

            var result = new Dictionary<string, AllCodesCode[]>();

            foreach (var kartlegging in kartleggingsenheter)
            {
                result.Add(kartlegging.Key, CreateCodesByNaturtype(kartlegging.Value, host).ToArray());
            }

            return result;
        }

        private static IEnumerable<EnvironmentVariable> CreateTrinn(IEnumerable<TrinnV21B> naturTypeTrinn)
        {
            return naturTypeTrinn.Select(x => new EnvironmentVariable
            {
                Kode = x.Kode,
                Navn = x.Navn,
                Type = x.Type,
                Trinn = x.Trinn == null ? null : CreateTrinn(x.Trinn).ToArray()
            });
        }

        private static IEnumerable<Step> CreateTrinn(IEnumerable<SubTrinnV21B> naturTypeTrinn)
        {
            return naturTypeTrinn.Select(x => new Step
            {
                Kode = x.Kode,
                Navn = x.Navn
            });
        }

        #endregion
    }
}
