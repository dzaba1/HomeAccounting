using System;

namespace Dzaba.HomeAccounting.Windows.Model
{
    public struct MemberNamePair : IEquatable<MemberNamePair>
    {
        public MemberNamePair(int? id, string name)
        {
            Id = id;
            Name = name;
        }

        public int? Id { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(MemberNamePair other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MemberNamePair other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(MemberNamePair left, MemberNamePair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MemberNamePair left, MemberNamePair right)
        {
            return !left.Equals(right);
        }
    }
}
