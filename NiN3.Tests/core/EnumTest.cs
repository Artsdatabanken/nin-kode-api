
using NiN3.Core.Models.Enums;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NiN3.Tests.Test.core
{

    //<summary>
    // Tests for various Enums using the EnumUtil Class.
    //</summary>
    public class EnumTest
    {

        [Fact]
        public void TestingEcosystnivaaEnum()
        {
            var stringToParse = "B";
            EcosystnivaaEnum? e = EnumUtil.ParseEnum<EcosystnivaaEnum>(stringToParse);
            Assert.Equal(stringToParse, e.ToString());
            Assert.Equal("Biotisk", EnumUtil.ToDescription(e));
        }

        [Fact]
        public void TestingTypeKategoriEnum()
        {
            var stringToParse = "PE";
            TypekategoriEnum? e = EnumUtil.ParseEnum<TypekategoriEnum>(stringToParse);
            Assert.Equal(stringToParse, e.ToString());
            Assert.Equal("Primært økodiversitetsnivå", EnumUtil.ToDescription(e));
        }

        [Fact]
        public void TestingTypeKategori2Enum()
        {
            var stringToParse = "NK";
            Typekategori2Enum? e = EnumUtil.ParseEnum<Typekategori2Enum>(stringToParse);
            Assert.Equal(stringToParse, e.ToString());
            Assert.Equal("Naturkompleks", EnumUtil.ToDescription(e));
        }

        [Fact]
        public void TestingProsedyrekategoriEnum()
        {
            var stringToParse = "O";
            ProsedyrekategoriEnum? e = EnumUtil.ParseEnum<ProsedyrekategoriEnum>(stringToParse);
            Assert.Equal(stringToParse, e.ToString());
            Assert.Equal("Spesiell variasjonsbredde. Sterkt endret system. Hevdpreget. Jordbruksmark.", EnumUtil.ToDescription(e));
        }

    
        [Fact]
        public void TestFetchEnumDescription()
        {
            Typekategori3Enum? result = EnumUtil.ParseEnum<Typekategori3Enum>("VM");
            Assert.Equal("Vannmassesystemer", EnumUtil.ToDescription(result));
        }

        [Fact]
        public void TestMaalestokkEnumDescription()
        {
            MaalestokkEnum? result = EnumUtil.ParseEnum<MaalestokkEnum>("M005");
            Assert.Equal("Kartleggingsenhet tilpasset 1:5000", EnumUtil.ToDescription(result));
        }

        [Fact]
        public void TestDescriptions()
        {
            Assert.Equal("Artssammensetningsdynamikk", GetEnumDescription(Variabelkategori2Enum.AD));
            Assert.Equal("Bergarter", GetEnumDescription(Variabelkategori2Enum.BE));
            Assert.Equal("Korttidsmiljøvariabel", GetEnumDescription(Variabelkategori2Enum.KM));
            Assert.Equal("Lokal miljøvariabel", GetEnumDescription(Variabelkategori2Enum.LM));
            Assert.Equal("Landform-objekter", GetEnumDescription(Variabelkategori2Enum.LO));
            Assert.Equal("Miljødynamikk", GetEnumDescription(Variabelkategori2Enum.MD));
            Assert.Equal("Menneskeskapt objekt", GetEnumDescription(Variabelkategori2Enum.MO));
            Assert.Equal("Naturgitt objekt", GetEnumDescription(Variabelkategori2Enum.NO));
            Assert.Equal("Romlig artsfordelingsmønster", GetEnumDescription(Variabelkategori2Enum.RA));
            Assert.Equal("Regional miljøvariabel", GetEnumDescription(Variabelkategori2Enum.RM));
            Assert.Equal("Romlig strukturvariasjon", GetEnumDescription(Variabelkategori2Enum.RS));
            Assert.Equal("Strukturerende og funksjonelle artsgrupper", GetEnumDescription(Variabelkategori2Enum.SA));
            Assert.Equal("Terrengformvariasjon", GetEnumDescription(Variabelkategori2Enum.TF));
            Assert.Equal("Vertikal struktur", GetEnumDescription(Variabelkategori2Enum.VS));
        }

        [Fact]
        public void TestParseEnumTypekategori2_WhenValueIs0()
        {
            var stringToParse_0 = "0";
            Typekategori2Enum? e = EnumUtil.ParseEnum<Typekategori2Enum>(stringToParse_0);
            Assert.Equal(null, e);
            Assert.Equal(null, EnumUtil.ToDescription(e));
            var stringToParse_dash = "-";
            Typekategori2Enum? e_dash = EnumUtil.ParseEnum<Typekategori2Enum>(stringToParse_0);
            Assert.Equal(null, e_dash);
            Assert.Equal(null, EnumUtil.ToDescription(e_dash));
            var stringToParse_blank = "";
            Typekategori2Enum? e_blank = EnumUtil.ParseEnum<Typekategori2Enum>(stringToParse_0);
            Assert.Equal(null, e_blank);
            Assert.Equal(null, EnumUtil.ToDescription(e_blank));
            Typekategori2Enum? eNull = EnumUtil.ParseEnum<Typekategori2Enum>(null);
            Assert.Equal(null, eNull);
            Assert.Equal(null, EnumUtil.ToDescription(eNull));
        }

        [Fact]
        public void TestParseEnumTypekategori2_WhenValueIsValid()
        {
            var stringToParse = "LA";
            Typekategori2Enum? e = EnumUtil.ParseEnum<Typekategori2Enum>(stringToParse);
            Assert.Equal("LA", e.ToString());
        }

        private static string GetEnumDescription(Enum enumerationValue)
        {
            var type = enumerationValue.GetType();
            var memInfo = type.GetMember(enumerationValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
