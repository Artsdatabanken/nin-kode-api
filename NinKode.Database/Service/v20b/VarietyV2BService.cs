namespace NinKode.Database.Service.v20b
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Model.v20b;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Linq;

    public class VarietyV2BService
    {
        private const string IndexName = "Variasjons/ByKode";
        private readonly DocumentStore _store;

        public VarietyV2BService(string url, string databaseName)
        {
            _store = new DocumentStore
            {
                Url = url,
                DefaultDatabase = databaseName
            };
            _store.Initialize(true);

            var index = _store.DatabaseCommands.GetIndex(IndexName);

            if (index != null) return;

            _store.DatabaseCommands.PutIndex(IndexName,
                new IndexDefinition
                {
                    Map = "from doc in docs.Variasjons\nselect new\n{\n\tKode = doc.Kode\n}"
                }
            );
        }

        public IEnumerable<VarietyAllCodes> GetAll(string host)
        {
            using (var session = _store.OpenSession())
            {
                var query = session.Query<VariasjonV2B>(IndexName);
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return CreateVarietyAllCodes(enumerator.Current.Document, host);
                    }
                }
            }
        }

        public VarietyCode GetByKode(string id, string host)
        {
            if (string.IsNullOrEmpty(id)) return null;

            using (var session = _store.OpenSession())
            {
                var query = session.Query<VariasjonV2B>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        return CreateVarietyCode(enumerator.Current.Document, host);
                    }
                }
            }

            return null;
        }

        #region private methods

        private static VarietyAllCodes CreateVarietyAllCodes(VariasjonV2B variasjon, string host)
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

        private static VarietyCode CreateVarietyCode(VariasjonV2B variasjon, string host)
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

            foreach (var kode in koder)
            {
                yield return new VarietyCodeCode
                {
                    Id = kode,
                    Definition = $"{host}{kode.Replace(" ", "_")}"
                };
            }
        }

        #endregion
    }
}
