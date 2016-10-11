namespace ProjectEuler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Sequences
    {
        private static List<int> cachedPrimes = new List<int>() { 2 };


        public static IEnumerable<long> Fibbonaci()
        {
            long firstValue = 1;
            long secondValue = 1;

            yield return firstValue;
            yield return secondValue;

            while (true)
            {
                long returnValue = firstValue + secondValue;

                // Now move the values around to allow us to calculate the next value.
                firstValue = secondValue;
                secondValue = returnValue;

                yield return returnValue;
            }
        }

        public static long SumOfSeries(long val, long max)
        {
            long numberOfTerms = (max - 1) / val;
            long lastTermValue = numberOfTerms * val;

            float averageTerm = (val + lastTermValue) / 2.0f;

            return (long)(averageTerm * numberOfTerms);
        }

        public static IEnumerable<BigNumber> BigFibbonaci()
        {
            BigNumber firstValue = 1;
            BigNumber secondValue = 1;

            yield return firstValue;
            yield return secondValue;

            while (true)
            {
                BigNumber returnValue = firstValue + secondValue;

                // Now move the values around to allow us to calculate the next value.
                firstValue = secondValue;
                secondValue = returnValue;

                yield return returnValue;
            }
        }

        public static IEnumerable<int> Primes(int startingValue = 1)
        {
            while (true)
            {
                if (startingValue.IsPrime())
                {
                    yield return startingValue;

                }

                startingValue++;
            }
        }

        public static IEnumerable<int> PrimesUpTo(int startingValue, int lastValue)
        {
            while (startingValue <= lastValue)
            {
                if (startingValue.IsPrime())
                {
                    yield return startingValue;

                }

                startingValue++;
            }
        }


        public static IEnumerable<int> CachedPrimes(bool continueGeneration = true)
        {
            // Dump our cached values first.
            foreach (var value in cachedPrimes)
            {
                yield return value;
            }

            if (continueGeneration)
            {
                foreach (var newValue in Primes(cachedPrimes.Last() + 1))
                {
                    cachedPrimes.Add(newValue);
                    yield return newValue;
                }
            }
        }

        public static IEnumerable<DateTime> MonthsUntil(this DateTime startDate, DateTime endDate)
        {
            DateTime returnDate = startDate;

            while (returnDate <= endDate)
            {
                yield return returnDate;
                returnDate = returnDate.AddMonths(1);
            }

            yield break;
        }

        /// <summary>
        /// Returns the series of triangle numbers...
        /// 1 = 1
        /// 1 + 2 = 3
        /// 1 + 2 + 3 = 6
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<long> TriangleNumbers()
        {
            long total = 0;
            long i = 1;

            do
            {
                total += i++;
                yield return total;
            } while (total > 0);

            yield break;
        }

        public static IEnumerable<long> PentagonalNumbers()
        {
            long total = 0;
            long i = 1;

            do
            {
                total = (i * ((3 * i) - 1)) / 2;
                i++;
                yield return total;
            }
            while (total > 0);

            yield break;
        }

        public static IEnumerable<long> HexagonalNumbers()
        {
            long total = 0;
            long i = 0;

            do
            {
                total += (4*i) + 1;
                i++;
                yield return total;
            } while (total > 0);

            yield break;
        }

        public static IEnumerable<int> Decrement(int start)
        {
            int returnValue = start;
            while (start >= 0)
            {
                yield return start--;
            }

            yield break;
        }

        public static IEnumerable<Tuple<int, int, int>> PythagoreanTriplets(int maxValue)
        {
            for (int i = 1; i < maxValue; i++)
            {
                for (int j = i + 1; j < maxValue; j++)
                {
                    for (int k = j + 1; k < maxValue; k++)
                    {
                        if (ValueExtensions.IsPythagoreanTriplet(i, j, k))
                        {
                            yield return Tuple.Create(i, j, k);
                            break;
                        }
                    }
                }
            }

            yield break;
        }

        public static IEnumerable<Tuple<int, int, int>> PythagoreanTripletsPerimeter(int perimeter)
        {
            for (int i = 1; i < perimeter; i++)
            {
                for (int j = i + 1; j < perimeter; j++)
                {
                    for (int k = j + 1; k < perimeter; k++)
                    {
                        if (i + j + k == perimeter && ValueExtensions.IsPythagoreanTriplet(i, j, k))
                        {
                            yield return Tuple.Create(i, j, k);
                        }
                    }
                }
            }

            yield break;
        }

        public static IEnumerable<System.Int64[]> GetPascalTriangle()
        {
            System.Int64[] row = new System.Int64[1] { 1 };

            for (int n = 2; ; n++)
            {
                yield return row;

                System.Int64[] nrow = new System.Int64[n];
                Array.Copy(row, nrow, n - 1);
                for (int i = 1; i < n; i++)
                    nrow[i] += row[i - 1];
                row = nrow;
            }
        }

        public static int[] DivisorSequence(int numerator, int divisor)
        {
            var previousSequences = new List<Tuple<int, int, int>>();

            while (numerator % divisor != 0)
            {
                numerator *= 10;

                var divisorState = Tuple.Create(numerator / divisor, numerator, divisor);

                if (!previousSequences.Contains(divisorState))
                {
                    previousSequences.Add(divisorState);
                }
                else
                {
                    return previousSequences.Select(t => t.Item1).ToArray();
                }

                numerator = numerator % divisor;
            }

            return previousSequences.Select(t => t.Item1).ToArray();
        }
    }
}
