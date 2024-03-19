using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiN3.Core.Models.Enums
{
    public enum SearchMethodEnum
    {
        [Description("StartsWith")]
        SW,
        [Description("Contains")]
        C,
        Default
    }
}
