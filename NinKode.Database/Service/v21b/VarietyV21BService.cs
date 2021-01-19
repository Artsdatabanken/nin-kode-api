﻿namespace NinKode.Database.Service.v21b
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using NinKode.Common.Models.Variety;
    using NinKode.Database.Model.v21b;
    using Raven.Abstractions.Indexing;
    using Raven.Client.Document;
    using Raven.Client.Linq;

    public class VarietyV21BService : IVarietyV21BService
    {
        private const string IndexName = "Variasjons/ByKode";
        private readonly DocumentStore _store;

        public VarietyV21BService(IConfiguration configuration)
        {
            _store = new DocumentStore
            {
                Url = configuration.GetValue("RavenDbUrl", "http://it-webadb01.it.ntnu.no:8180/"),
                DefaultDatabase = configuration.GetValue("RavenDbNameV21B", "SOSINiNv2.0b")
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
                var query = session.Query<VariasjonV21B>(IndexName);
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        yield return CreateVarietyAllCodes(enumerator.Current?.Document, host);
                    }
                }
            }
        }

        public VarietyCode GetByKode(string id, string host)
        {
            if (string.IsNullOrEmpty(id)) return null;

            using (var session = _store.OpenSession())
            {
                var query = session.Query<VariasjonV21B>(IndexName).Where(x => x.Kode.Equals(id, StringComparison.OrdinalIgnoreCase));
                using (var enumerator = session.Advanced.Stream(query))
                {
                    while (enumerator.MoveNext())
                    {
                        return CreateVarietyCode(enumerator.Current?.Document, host);
                    }
                }
            }

            return null;
        }

        #region private methods

        private static VarietyAllCodes CreateVarietyAllCodes(VariasjonV21B variasjon, string host)
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

        private static VarietyCode CreateVarietyCode(VariasjonV21B variasjon, string host)
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