using System;
using System.Collections.Generic;
using System.Linq;

namespace Dzaba.HomeAccounting.Engine.Utils
{
    public static class Require
    {
        public static void NotNull(object obj, string argumentName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotEmpty(string str, string argumentName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotEmpty<T>(IEnumerable<T> collection, string argumentName)
        {
            NotNull(collection, argumentName);

            if (!collection.Any())
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotWhiteSpace(string str, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void IndexInRange(int current, int length, string argumentName)
        {
            if (current < 0 || current >= length)
            {
                throw new ArgumentOutOfRangeException(argumentName, $"Variable '{argumentName}' is out of range of length {length}.");
            }
        }

        public static void InRange(double current, double min, double max, string argumentName)
        {
            if (current < min || current > max)
            {
                throw new ArgumentOutOfRangeException(argumentName, $"Variable '{argumentName}' is out of range (${min}, ${max}).");
            }
        }
    }
}
