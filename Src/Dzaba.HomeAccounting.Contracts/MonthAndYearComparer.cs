using System;
using System.Collections.Generic;

namespace Dzaba.HomeAccounting.Contracts
{
    public class MonthAndYearComparer : IEqualityComparer<DateTime>, IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            if (x.Year == y.Year)
            {
                return x.Month.CompareTo(y.Month);
            }

            return x.Year.CompareTo(y.Year);
        }

        public bool Equals(DateTime x, DateTime y)
        {
            return x.Month == y.Month && x.Year == y.Year;
        }

        public int GetHashCode(DateTime obj)
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + obj.Month.GetHashCode();
                hash = (hash * 7) + obj.Year.GetHashCode();
                return hash;
            }
        }
    }
}
