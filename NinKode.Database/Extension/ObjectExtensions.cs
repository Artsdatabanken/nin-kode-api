namespace NinKode.Database.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NaturalSort.Extension;
    using NinKode.Common.Models.Code;

    public static class ObjectExtensions
    {
        public static IList<string> OrderByList<T>(this IEnumerable<string> list)
        {
            var orderedList = list.ToList();
            orderedList.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            return orderedList.ToList();
        }
    }

    public class EnvironmentComparer : Comparer<EnvironmentVariable>
    {
        public override int Compare(EnvironmentVariable x, EnvironmentVariable y)
        {
            var list = new List<string> { x.Kode, y.Kode };
            list.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            var element = list[0];
            return x.Kode.Equals(element) ? -1 : 1;
        }
    }

    public class StepComparer : Comparer<Step>
    {
        public override int Compare(Step x, Step y)
        {
            var list = new List<string> { x.Kode, y.Kode };
            list.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            var element = list[0];
            return x.Kode.Equals(element) ? -1 : 1;
        }
    }

    public class CodesComparer : Comparer<Codes>
    {
        public override int Compare(Codes x, Codes y)
        {
            var list = new List<string> { x.Kode.Id, y.Kode.Id };
            list.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            var element = list[0];
            return x.Kode.Id.Equals(element) ? -1 : 1;
        }
    }
}
