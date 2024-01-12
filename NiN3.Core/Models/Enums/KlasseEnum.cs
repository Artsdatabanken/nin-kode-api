using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace NiN3.Core.Models.Enums
{
    public enum KlasseEnum
    {
        //[Description("")]
        //Default,
        [Description("Type")]
        T,
        [Description("Hovedtypegruppe")]
        HTG,
        [Description("Hovedtype")]
        HT,
        [Description("Grunntype")]
        GT,
        [Description("Kartleggingsenhet")]
        KE,
        [Description("Variabel")]
        V,
        [Description("Variabelnavn")]
        VN
    }
}
