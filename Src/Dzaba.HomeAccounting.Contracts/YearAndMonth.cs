using System;

namespace Dzaba.HomeAccounting.Contracts
{
    public struct YearAndMonth : IEquatable<YearAndMonth>, IComparable<YearAndMonth>
    {
        public ushort Year { get; }

        public byte Month { get; }

        public YearAndMonth(ushort year, byte month)
        {
            Year = year;
            Month = month;
        }

        public YearAndMonth(DateTime date)
        {
            Year = (ushort)date.Year;
            Month = (byte)date.Month;
        }

        public bool Equals(YearAndMonth other)
        {
            return Month == other.Month && Year == other.Year;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((YearAndMonth)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + Month.GetHashCode();
                hash = (hash * 7) + Year.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(YearAndMonth other)
        {
            if (Year == other.Year)
            {
                return Month.CompareTo(other.Month);
            }

            return Year.CompareTo(other.Year);
        }

        public static bool operator ==(YearAndMonth x, YearAndMonth y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(YearAndMonth x, YearAndMonth y)
        {
            return !Equals(x, y);
        }

        public static bool operator >(YearAndMonth x, YearAndMonth y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator >=(YearAndMonth x, YearAndMonth y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <(YearAndMonth x, YearAndMonth y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator <=(YearAndMonth x, YearAndMonth y)
        {
            return x.CompareTo(y) <= 0;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, 1);
        }

        public YearAndMonth AddMonths(int months)
        {
            var year = Year;
            var month = Month + months;

            while (month > 12)
            {
                year++;
                month -= 12;
            }

            while (month <= 0)
            {
                year--;
                month += 12;
            }

            return new YearAndMonth(year, (byte)month);
        }

        public override string ToString()
        {
            return $"{Year}/{Month}";
        }
    }
}
