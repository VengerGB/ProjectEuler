
using System.Collections.Generic;

namespace ProjectEuler
{
    using System;
    using System.Linq;

    public struct RomanNumeral
    {
        private int value;

        public RomanNumeral(int value)
        {
            this.value = value;
        }

        public static RomanNumeral Parse(string numerals)
        {
            int total = 0;
            int previousValue = 0;

            foreach (var numeral in numerals)
            {
                int currentValue = ParseChar(numeral);

                if (previousValue > 0 && currentValue > previousValue)
                {
                    total = currentValue - total;
                }
                else
                {
                    total += currentValue;
                }

                previousValue = currentValue;
            }

            return new RomanNumeral(total);
        }

        public static int ParseChar(char numeral)
        {
            switch (numeral)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    throw new InvalidOperationException("Unknown numeral " + numeral);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var d = DateTime.MaxValue;
            var d2 = DateTime.MaxValue.ToUniversalTime();

            var d3 = DateTime.MaxValue - DateTime.UtcNow;

            var d4 = DateTime.MaxValue - DateTime.Now;
        }
    }
}