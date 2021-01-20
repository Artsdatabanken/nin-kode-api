namespace NinKode.Database.Service.v1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using NinKode.Common.Models.Code;
    using NinKode.Database.Model.v1;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Linq;

    public class CodeV1Service : ICodeV1Service
    {
        private const string IndexName = "NaturTypes/ByKode";
        private const string RavenDbKeyName = "RavenDbNameV1";
        private const string RavenDbKeyUrl = "RavenDbUrl";

        private readonly DocumentStore _store;

        public CodeV1Service(IConfiguration configuration)
        {
            var dbName = configuration.GetValue(RavenDbKeyName, "");
            var dbUrl = configuration.GetValue("RavenDbUrl", "http://localhost:8080/");

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
        }

        public IEnumerable<Codes> GetAll(string host)
        {
            using (var session = _store.OpenSession())
            {
                var query = session.Query<NaturTypeV1>(IndexName);
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return CreateCodesByNaturtype(enumerator.Current?.Document, host);
                    }
                }
            }
        }

        public Codes GetByKode(string id, string host)
        {
            if (string.IsNullOrEmpty(id)) return null;

            id = id.Replace("_", " ");

            using (var session = _store.OpenSession())
            {
                var query = session.Query<NaturTypeV1>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        return CreateCodesByNaturtype(enumerator.Current?.Document, host);
                    }
                }
            }

            return null;
        }

        #region private methods

        private static Codes CreateCodesByNaturtype(NaturTypeV1 naturType, string host)
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

            foreach (var kode in koder)
            {
                yield return new AllCodesCode
                {
                    Id = kode,
                    Definition = $"{host}{kode.Replace(" ", "_")}"
                };
            }
        }

        #endregion
    }
}
