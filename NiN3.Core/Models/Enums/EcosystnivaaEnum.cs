using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;

namespace NiN3.Core.Models.Enums
{
    /// <summary>
    /// Testing summary for ecosystnivaa
    /// </summary>
    public enum EcosystnivaaEnum
    {
        //[Description("")]
        //Default,
        [EnumMember(Value = "A")]
        [Description("Abiotisk")]
        A,
        [EnumMember(Value = "B")]
        [Description("Biotisk")]
        B,
        [EnumMember(Value = "C")]
        [Description("Økodiversitet")]
        C
    }
}
