namespace NinKode.Database.Extension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NaturalSort.Extension;
    using NinKode.Common.Models.Code;
    using NinKode.Common.Models.Variety;

    public static class ObjectExtensions
    {
        public static IList<string> OrderByList<T>(this IEnumerable<string> list)
        {
            var orderedList = list.ToList();
            orderedList.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            return orderedList.ToList();
        }

        internal class CustomComparer : IComparer<string>
        {
            private readonly StringComparison _comparison;
            private readonly Comparer _comparer;

            public CustomComparer(StringComparison comparison)
            {
                _comparison = comparison;
                _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);
            }

            public int Compare(string x, string y)
            {
                string numxs = string.Concat(x.TakeWhile(c => char.IsDigit(c)).ToArray());
                string numys = string.Concat(y.TakeWhile(c => char.IsDigit(c)).ToArray());

                int xnum;
                int ynum;
                if (!int.TryParse(numxs, out xnum) || !int.TryParse(numys, out ynum))
                {
                    return _comparer.Compare(x, y);
                }
                int compareNums = xnum.CompareTo(ynum);
                if (compareNums != 0)
                {
                    return compareNums;
                }
                return _comparer.Compare(x, y);
            }
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

    public class AllCodesCodeComparer : Comparer<AllCodesCode>
    {
        public override int Compare(AllCodesCode x, AllCodesCode y)
        {
            var list = new List<string> { x.Id, y.Id };
            list.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            var element = list[0];
            return x.Id.Equals(element) ? -1 : 1;
        }
    }

    public class VarietyCodeCodeComparer : Comparer<VarietyCodeCode>
    {
        public override int Compare(VarietyCodeCode x, VarietyCodeCode y)
        {
            var list = new List<string> { x.Id, y.Id };
            list.Sort(new NaturalSortComparer(StringComparison.Ordinal));
            var element = list[0];
            return x.Id.Equals(element) ? -1 : 1;
        }
    }
}
