﻿namespace NinKode.Database.Services.v22
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using NiN.Database;
    using NinKode.Common.Interfaces;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Extension;
    using NinKode.Database.Model.v21b;
    using NinKode.Database.Model.v22;
    using Raven.Abstractions.Indexing;

    public class VarietyV22Service : IVarietyV22Service
    {
        //private const string IndexName = "Variasjons/ByKode";
        //private const string RavenDbKeyName = "RavenDbNameV22";
        //private const string RavenDbKeyUrl = "RavenDbUrl";

        private List<VariasjonV22> allVariations;
        private string _variations_v22jsonFileStr;

        public List<VariasjonV22> AllVariations
        {
            get
            {
                if (allVariations == null)
                {

                    if (File.Exists(_variations_v22jsonFileStr))
                    {
                        var text = File.ReadAllText(_variations_v22jsonFileStr);
                        allVariations = JsonSerializer.Deserialize<List<VariasjonV22>>(text);
                        return allVariations;
                    }

                    //allVariations = new List<VariasjonV22>();
                    //using (var session = _store.OpenSession())
                    //{
                    //    var query = session.Query<VariasjonV22>(IndexName);
                    //    using (var enumerator = session.Advanced.Stream(query))
                    //    {
                    //        while (enumerator.MoveNext())
                    //        {
                    //            allVariations.Add(enumerator.Current?.Document);
                    //        }
                    //    }
                    //}
                    //string jsonString = JsonSerializer.Serialize(allVariations.ToArray());
                    //System.IO.File.WriteAllText(_variations_v22jsonFileStr, jsonString);
                }

                return allVariations;
            }
        }

        //private readonly DocumentStore _store;

        public VarietyV22Service(IConfiguration configuration)
        {
            //var dbName = configuration.GetValue(RavenDbKeyName, "SOSINiNv2.2");
            //var dbUrl = configuration.GetValue("RavenDbUrl", "http://localhost:8080/");

            //if (string.IsNullOrWhiteSpace(dbName)) throw new Exception($"Missing \"{RavenDbKeyName}\"");
            //if (string.IsNullOrWhiteSpace(dbUrl)) throw new Exception($"Missing \"{RavenDbKeyUrl}\"");

            _variations_v22jsonFileStr = configuration.GetValue("variations22Json", "CsvFiles\\SOSINiNVariations_v22.json");

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
            //        Map = "from doc in docs.Variasjons\nselect new\n{\n\tKode = doc.Kode\n}"
            //    }
            //);
        }

        public IEnumerable<VarietyAllCodes> GetAll(NiNDbContext dbContext, string host, string version = "")
        {
            //using (var session = _store.OpenSession())
            //{
            //    var query = session.Query<VariasjonV22>(IndexName);
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            yield return CreateVarietyAllCodes(enumerator.Current?.Document, $"{host}hentkode/");
            //        }
            //    }
            //}
            return null;
        }

        public VarietyCode GetByKode(NiNDbContext dbContext, string id, string host, string version = "")
        {
            //if (string.IsNullOrEmpty(id)) return null;

            //using (var session = _store.OpenSession())
            //{
            //    var query = session.Query<VariasjonV22>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //    using (var enumerator = session.Advanced.Stream(query))
            //    {
            //        while (enumerator.MoveNext())
            //        {
            //            return CreateVarietyCode(enumerator.Current?.Document, host);
            //        }
            //    }
            //}

            return null;
        }

        public VarietyCode GetVariety(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var result = AllVariations.Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (result == null) return null;
            return CreateVarietyByCode(result);
            /*
            using (var session = _store.OpenSession())
            {
                var query = session.Query<VariasjonV22>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        return CreateVarietyByCode(enumerator.Current?.Document);
                    }
                }
            }
            
            return null;
            */
        }

        #region private methods

        private static VarietyCode CreateVarietyByCode(VariasjonV22 variasjon)
        {
            if (variasjon == null) return null;

            return new VarietyCode
            {
                Code = new VarietyCodeCode
                {
                    Id = variasjon.Kode
                },
                Name = variasjon.Navn,
                OverordnetKode = new VarietyCodeCode
                {
                    Id = variasjon.OverordnetKode
                },
                UnderordnetKoder = variasjon.UnderordnetKoder == null ? null : CreateVarietyCode(variasjon.UnderordnetKoder, "").ToArray()

            };
        }

        private static VarietyAllCodes CreateVarietyAllCodes(VariasjonV22 variasjon, string host)
        {
            if (variasjon == null) return null;

            return new VarietyAllCodes
            {
                Code = CreateVarietyAllCodesCode(variasjon.Kode, host),
                Name = variasjon.Navn,
                OverordnetKode = CreateVarietyAllCodesCode(variasjon.OverordnetKode, host),
                UnderordnetKoder = CreateVarietyAllCodesCode(variasjon.UnderordnetKoder, host).ToArray()
            };
        }

        private static VarietyAllCodesCode CreateVarietyAllCodesCode(string kode, string host)
        {
            if (string.IsNullOrEmpty(kode)) return null;

            return new VarietyAllCodesCode
            {
                Id = kode,
                Definition = $"{host}{kode.Replace(" ", "_")}"
            };
        }

        private static IEnumerable<VarietyAllCodesCode> CreateVarietyAllCodesCode(string[] koder, string host)
        {
            if (koder == null) yield break;

            foreach (var kode in koder)
            {
                yield return new VarietyAllCodesCode
                {
                    Id = kode,
                    Definition = $"{host}{kode.Replace(" ", "_")}"
                };
            }
        }

        private static VarietyCode CreateVarietyCode(VariasjonV22 variasjon, string host)
        {
            if (variasjon == null) return null;

            var varietyCode = new VarietyCode
            {
                Code = new VarietyCodeCode
                {
                    Id = variasjon.Kode,
                    Definition = $"{host}{variasjon.Kode.Replace(" ", "_")}"
                },
                Name = variasjon.Navn,
                OverordnetKode = new VarietyCodeCode
                {
                    Id = variasjon.OverordnetKode,
                    Definition = !string.IsNullOrEmpty(variasjon.OverordnetKode) ? $"{host}{variasjon.OverordnetKode.Replace(" ", "_")}" : ""
                },
                UnderordnetKoder = variasjon.UnderordnetKoder == null ? null : CreateVarietyCode(variasjon.UnderordnetKoder, host).ToArray()
            };

            return varietyCode;
        }

        private static IEnumerable<VarietyCodeCode> CreateVarietyCode(string[] koder, string host)
        {
            if (koder == null) yield break;

            var codeList = new List<string>();
            codeList.AddRange(koder);

            foreach (var code in codeList.OrderByList<string>())
            {
                yield return new VarietyCodeCode
                {
                    Id = code,
                    Definition = $"{host}{code.Replace(" ", "_")}"
                };
            }
        }
        #endregion
    }
}
