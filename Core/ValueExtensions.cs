using System.Text;

namespace ProjectEuler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using System.Security.Policy;

    public static class PrimeGenerator
    {
        private static HashSet<int> CalculatedPrimes = new HashSet<int>() { 2 };
        
        public static IEnumerable<int> Primes(int startingValue = 1)
        {
            while (true)
            {
                if (PrimeGenerator.IsPrime(startingValue))
                {
                    yield return startingValue;

                }

                startingValue++;
            }
        }

        public static bool IsPrime(int value)
        {
            if (PrimeGenerator.CalculatedPrimes.Contains(value))
            {
                return true;
            }
             
            
            var isPrime = QuickDeterminePrime(value);
            if (isPrime)
            {
                PrimeGenerator.CalculatedPrimes.Add(value);
            }

            return isPrime;
        }

        private static bool QuickDeterminePrime(int value)
        {
            if (value <= 1)
            {
                return false;
            }

            // first check if we already found this value
            var foundPrime = CalculatedPrimes.Contains(value);
            if (foundPrime)
            {
                return true;
            }

            // next check by dividing it by all known primes
            var isPrime = CalculatedPrimes.All(p => value % p != 0);
            if (isPrime)
            {
                CalculatedPrimes.Add(value);
                return true;
            }
            //for (int i = CalculatedPrimes.First(); i * i <= value; ++i)
            //{
            //    if (value % i == 0)
            //    {
            //        return false;
            //    }
            //}

            // This is a prime, lets not calculate it again.
            
            return false;
        }

        public static List<KeyValuePair<int, int>> PrimeFactors(int value)
        {
            var primeFactors = new List<KeyValuePair<int, int>>();

            foreach(var p in Sequences.PrimesUpTo(1, value))
            {
                var pair = new KeyValuePair<int, int>(p, 0);

                while (value % p == 0)
                {
                    pair = new KeyValuePair<int, int>(p, pair.Value + 1);
                    value = value / p;
                }

                if (pair.Value > 0)
                {
                    primeFactors.Add(pair);
                }
            }

            return primeFactors;
        }

        private static bool BruteForceDeterminePrime(int value)
        {
            if (value <= 1)
            {
                return false;
            }

            for (int i = 2; i * i <= value; ++i)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }


    public static class ValueExtensions
    {
        public static int[] GridAjacentNumbers(this int[,] grid, int runLength, int xStart, int yStart, int xTransform, int yTransform)
        {
            int xMax = grid.GetUpperBound(0);
            int yMax = grid.GetUpperBound(1);
            int currentXTransform = 0;
            int currentYTransform = 0;
            List<int> returnValues = new List<int>();
            int calculated = 0;

            while (calculated < runLength)
            {
                if ((xStart + currentXTransform <= xMax) && (xStart + currentXTransform >= 0) && (yStart + currentYTransform <= yMax) && (yStart + currentYTransform >= 0))
                {
                    returnValues.Add(grid[xStart + currentXTransform, yStart + currentYTransform]);
                    currentXTransform += xTransform;
                    currentYTransform += yTransform;
                }
                calculated++;
            }
        
            return returnValues.ToArray();
        }

        public static IEnumerable<int[]> AllAdjacentNumbers(this int[,] grid, int x, int y, int run)
        {
            for(int xTransform = -1; xTransform <= 1; xTransform++)
            {
                for (int yTransform = -1; yTransform <= 1; yTransform++)
                {
                    if (xTransform == 0 && yTransform == 0)
                    {
                        continue;
                    }

                    var value = grid.GridAjacentNumbers(run, x, y, xTransform, yTransform);
                    yield return value;
                }
            }
        }

        public static IEnumerable<int[]> AllAdjacentNumbers(this int[,] grid, int run = 4)
        {
            int xMax = grid.GetUpperBound(0);
            int yMax = grid.GetUpperBound(1);

            for (int x = 0; x <= xMax; x++)
            {
                for (int y = 0; y <= yMax; y++)
                {
                    foreach (var value in AllAdjacentNumbers(grid, x, y, run))
                    {
                        yield return value;
                    }
                }
            }
        }

        public static int[] NorthValues(this int[,] grid, int x, int y, int run)
        {
            return grid.GridAjacentNumbers(run, x, y, 0, -1);
        }

        public static int[] SouthValues(this int[,] grid, int x, int y, int run)
        {
            return grid.GridAjacentNumbers(run, x, y, 0, 1);
        }

        public static int[] SouthEastValues(this int[,] grid, int x, int y, int run)
        {
            return grid.GridAjacentNumbers(run, x, y, 1, 1);
        }

        public static bool IsPythagoreanTriplet(int a, int b, int c)
        {
            if (((a * a) + (b * b)) == (c*c))
            {
                return true;
            }

            return false;
        }

        public static bool IsPrime(this int value)
        {
            if (value <= 1)
            {
                return false;
            }

            for (int i = 2; i * i <= value; ++i )
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static int ConvertToInt(this char value)
        {
            return value - 48;
        }

        public static char ConvertToChar(this int value)
        {
            return (char)(value + 48);
        }

        public static bool IsPandigital(this int value)
        {
            var digits = value.GetDigitsYield();
            return Enumerable.Range(1, digits.Count()).All(v => digits.Contains(v));
        }

        public static bool IsPalendrome(this int value)
        {
            var digits = value.ToString();

            return digits.IsPalendrome();
        }

        public static bool IsPalendrome(this string value)
        {
            var digits = value.ToCharArray();

            for (int i = 0; i < digits.Length; i++)
            {
                // This is clumsy...  But should cause us to bail if we're comparing the array more than we need to.
                if (i == digits.Length - 1 - i)
                {
                    break;
                }

                if (digits[i] != digits[digits.Length - 1 - i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsTriangle(this long value)
        {
            bool triangle = false;

            if (value == 1)
            {
                return true;
            }

            if (value > 1)
            {
                //  n = (sqrt(8T + 1) - 1) / 2.

                double val = Math.Sqrt(8 * value + 1);
                long whole = (long)val;

                if (val == whole)
                {
                    if ((val - 1) % 2 == 0)
                    {
                        triangle = true;
                    }
                }
            }

            return triangle;
        }

        public static int FromDigits(this IEnumerable<int> value)
        {
            int returnvalue = 0;

            for(int i = 0 ; i < value.Count(); i++)
            {
                returnvalue *= 10;
                returnvalue += value.ElementAt(i);
            }

            return returnvalue;
        }

        public static int Factorial(this int value)
        {
            if (value == 0)
            {
                return 1;
            }
            else
            {
                return value * (value - 1).Factorial();
            }
        }

        public static System.Int64 Factorial(this System.Int64 value)
        {
            if (value == 0)
            {
                return 1;
            }
            else
            {
                return value * (value - 1).Factorial();
            }
        }

        public static int[] GetDigits(this long value)
        {
            // Manky - I should be able to work out how many digits there are in a number...
            var digits = new List<int>();

            while (value > 0)
            {
                digits.Add((int) value % 10);
                value = value / 10;
            }

            digits.Reverse();

            return digits.ToArray();
        }

        public static IEnumerable<int> GetDigitsYield(this int value)
        {
            while (value > 0)
            {
                yield return (int)value % 10;
                value = value / 10;
            }
        }

        public static int[] GetDigits(this int value)
        {
            return ((long) value).GetDigits();
        }

        public static long GetNumber(this int[] value)
        {
            long returnValue = 0;

            for(int i = 0; i < value.Length; i++)
            {
                returnValue *= 10;
                returnValue += value[i];
            }

            return returnValue;
        }

        private static int [] permutationValue;

        /// <summary>
        /// Returns sets of permuted numbers in Lexographic order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> list)
        {
            if (list.Count() == 1)
                return new List<IEnumerable<T>> { list };

            return list.Select((a, i1) => Permute(list.Where((b, i2) => i2 != i1))
                                            .Select(b => (new List<T> { a })
                                            .Union(b)))
                                            .SelectMany(c => c);
        }


        public static IEnumerable<int []> CalcPermutations(this int[] data, int elementLevel = -1, int k = 0)
        {
            if (elementLevel == -1)
            {
                permutationValue = new int[data.Length];
            }

            elementLevel++;
            permutationValue.SetValue(elementLevel, k);

            if (elementLevel == data.Length)
            {
                yield return permutationValue;
            }
            else
            {
                for (int i = 0; i < data.Length ; i++)
                {
                    if (permutationValue[i] == 0)
                    {
                        yield return CalcPermutations(data, elementLevel, i).First();
                    }
                }
            }
            elementLevel--;
            permutationValue.SetValue(0, k);
        }

        public static IEnumerable<int> Truncations(this int value)
        {
            var digits = value.GetDigitsYield().Reverse().ToList();

            // truncate from the left.
            while (digits.Count() > 1)
            {
                digits.RemoveAt(0);
                yield return digits.ToArray().FromDigits();
            }
            
            // truncate from the right.
            digits = value.GetDigitsYield().Reverse().ToList();
            while (digits.Count() > 1)
            {
                digits.RemoveAt(digits.Count() - 1);
                yield return digits.ToArray().FromDigits();
            }
        }

        public static bool IsPrime(this long value)
        {
            for (long i = 2; i * i <= value; ++i)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static Tuple<int, int, int> PrimeFactors(this long value)
        {
            long modifiedValue = value;
            int numberofTwos = 0;
            int numberofThrees = 0;
            int numberoffives = 0;


            //2's
            while (modifiedValue % 2 == 0 && modifiedValue > 0)
            {
                modifiedValue /= 2;
                numberofTwos++;
            }

            while (modifiedValue % 3 == 0 && modifiedValue > 0)
            {
                modifiedValue /= 3;
                numberofThrees++;
            }

            while (modifiedValue % 5 == 0 && modifiedValue > 0)
            {
                modifiedValue /= 5;
                numberoffives++;
            }

            return new Tuple<int, int, int>(numberofTwos, numberofThrees, numberoffives);
        }

        public static int NumberOfDivisors(this long value)
        {
            var primefactors = value.GetPrimeFactors();

            if (!primefactors.Any())
            {
                return 0;
            }

            int divisors = primefactors.Select(t => t.Item2 + 1).Aggregate((h, t) => h * t);

            return divisors;
        }

        public static IEnumerable<int> Divisors(this int value)
        {
            if (value > 0)
            {

                yield return 1;

                // Is this number odd?
                int startingValue = (value%2 != 0 ? 3 : 2);

                // We only need to go halfway...
                for (int i = startingValue; i <= value/2; ++i)
                {
                    if (value%i == 0)
                    {
                        yield return i;
                    }
                }

                // The value is always a divisor of its self...
                yield return value;
            }
        }

        public static BigNumber BigSum(this IEnumerable<BigNumber> values)
        {
            BigNumber l = values.Aggregate((h, t) => h + t);

            return l;
        }

        public static IEnumerable<int> RotateDigits(this int value)
        {
            var digits = value.GetDigitsYield().Reverse().ToArray();
            
            for (int startingPoint = 0; startingPoint < digits.Length; startingPoint++)
            {
                int[] reordered = new int[digits.Length];

                // Just like a circular buffer.
                Array.Copy(digits, startingPoint, reordered, 0, digits.Length - startingPoint);

                if (digits.Length - startingPoint < digits.Length)
                {
                    Array.Copy(digits, 0, reordered, digits.Length - startingPoint, startingPoint);
                }

                yield return reordered.FromDigits();
            }
        }

        public static bool IsPerfect(this int value)
        {
            return value.ProperDivisors().Sum() == value;
        }

        public static bool IsAbundant(this int value)
        {
            return value.ProperDivisors().Sum() > value;
        }

        public static string AddNumericString(this string value, string othervalue)
        {
            List<char> returnString = new List<char>();

            if (!value.All(c => char.IsDigit(c)))
            {
                throw new InvalidOperationException("'value' is not a numeric string.");
            }

            if (!othervalue.All(c => char.IsDigit(c)))
            {
                throw new InvalidOperationException("'othervalue' is not a numeric string.");
            }

            // Add digits individually right to left.

            int carry = 0;
            int biggestNumber = Math.Max(value.Length, othervalue.Length);

            for (int i = 0; i < biggestNumber; i++)
            {
                short a = 0;
                short b = 0;

                if (value.Length - i - 1 >= 0)
                {
                    a = short.Parse(value.ElementAt(value.Length - i - 1).ToString());
                }

                if (othervalue.Length - i - 1 >= 0)
                {
                    b = short.Parse(othervalue.ElementAt(othervalue.Length - i - 1).ToString());
                }

                int result = a + b + carry;

                if (result > 9)
                {
                    carry = result / 10;
                    result = result % 10;
                }
                else
                {
                    carry = 0;
                }

                returnString.Add(char.Parse(result.ToString()));
            }

            if (carry > 0)
            {
                returnString.Add(char.Parse(carry.ToString()));
            }

            returnString.Reverse();
            return new string(returnString.ToArray());
        }

        /// <summary>
        /// 'Proper' divisors don't include the number itself...
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<int> ProperDivisors(this int value)
        {
            return value.Divisors().TakeWhile(i => i != value);
        }

        public static long CollatzChainLength(this int value)
        {
            long length = 0;
            long currentValue = (long) value;

            while (currentValue != 1)
            {
                if (currentValue % 2 == 0)
                {
                    currentValue = currentValue / 2;
                }
                else
                {
                    currentValue = (3 * currentValue) + 1;
                }

                length++;
            }

            return length;
        }

        public static IEnumerable<int> CollatzChain(this int value)
        {
            int currentValue = value;

            while (currentValue != 1)
            {
                if (currentValue % 2 == 0)
                {
                    currentValue = currentValue / 2;
                }
                else
                {
                    currentValue = (3 * currentValue) + 1;
                }

                yield return currentValue;
            }

            yield break;
        }


        public static int SumOfCombinations(this List<int> denominations, int desiredValue, int currentTotal = 0, List<int> numberOfCoins = null)
        {
            if (currentTotal > desiredValue)
            {
                // This isn't a solution we can use - we went too far.
                return 0;
            }

            if (currentTotal == desiredValue)
            {
                // Bam - we found a solution.
                return 1;
            }

            if (numberOfCoins == null)
            {
                numberOfCoins = new List<int>();
            }

            return denominations.Sum(c =>
            {
                int successfulCombinations = 0;

                if (c >= numberOfCoins.LastOrDefault())
                {
                    List<int> currentSolution = new List<int>(numberOfCoins);
                    currentSolution.Add(c);
                    successfulCombinations += SumOfCombinations(denominations, desiredValue, currentTotal + c, currentSolution);
                }

                return successfulCombinations;
            });
        }

        public static string ToWords(this int value)
        {
            var digits = value.GetDigitsYield().Reverse().ToArray();

            if (digits.Length > 4)
            {
                throw new InvalidOperationException("I can't work out the words for a number this big.");
            }

            var stringBuilder = new StringBuilder();
            int counter = 0;

            if (digits.Length > 3)
            {
                stringBuilder.Append(digits[counter].SingleToWord());
                stringBuilder.Append(4.PlacesToWord());
                counter++;
            }

            if (digits.Length > 2 && digits[counter] != 0)
            {
                stringBuilder.Append(digits[counter].SingleToWord());
                stringBuilder.Append(3.PlacesToWord());
                
                if (digits[counter+1] != 0 || digits[counter+2] != 0)
                {
                    stringBuilder.Append(" and ");
                }

                counter++;
            }

            if (digits.Length > 1)
            {
                if (digits[counter] == 0)
                {
                }
                else if (digits[counter] == 1)
                {
                    stringBuilder.Append(digits[counter+1].TeensToWord());
                    return stringBuilder.ToString();
                }
                else
                {
                    stringBuilder.Append(digits[counter].TensToWord());

                    if (digits[counter+1] != 0)
                    {
                        stringBuilder.Append("-");
                    }
                }

                counter++;
            }

            if (digits.Length > 0)
            {
                stringBuilder.Append(digits[counter].SingleToWord());
            }

            return stringBuilder.ToString().Trim();
        }

        public static string SingleToWord(this int value)
        {
            switch (value)
            {
                case 1:
                    return "one ";
                case 2:
                    return "two ";
                case 3:
                    return "three ";
                case 4:
                    return "four ";
                case 5:
                    return "five ";
                case 6:
                    return "six ";
                case 7:
                    return "seven ";
                case 8:
                    return "eight ";
                case 9:
                    return "nine ";
                default:
                    return string.Empty;
            }
        }

        public static string TeensToWord(this int value)
        {
            switch (value)
            {
                case 0:
                    return "ten";
                case 1:
                    return "eleven";
                case 2:
                    return "twelve";
                case 3:
                    return "thirteen";
                case 4:
                    return "fourteen";
                case 5:
                    return "fifteen";
                case 6:
                    return "sixteen";
                case 7:
                    return "seventeen";
                case 8:
                    return "eighteen";
                case 9:
                    return "nineteen";
                default:
                    return string.Empty;
            }
        }

        public static string TensToWord(this int value)
        {
            switch (value)
            {
                case 2:
                    return "twenty";
                case 3:
                    return "thirty";
                case 4:
                    return "forty";
                case 5:
                    return "fifty";
                case 6:
                    return "sixty";
                case 7:
                    return "seventy";
                case 8:
                    return "eighty";
                case 9:
                    return "ninety";
                default:
                    return string.Empty;
            }
        }

        public static string PlacesToWord(this int value)
        {
            switch (value)
            {
                case 3:
                    return "hundred";
                case 4:
                    return "thousand";
                default:
                    return string.Empty;
            }
        }
        
        public static List<Tuple<int, int>> GetPrimeFactors(this long number)
        {
            List<Tuple<int, int>> returnFactorisation = new List<Tuple<int, int>>(); 

            foreach (int currentPrime in Sequences.CachedPrimes().TakeWhile(p => p <= number))
            {
                int timesDivisible = 0;
                while (number%currentPrime == 0)
                {
                    timesDivisible++;
                    number = number/currentPrime;
                }

                if (timesDivisible > 0)
                {
                    returnFactorisation.Add(Tuple.Create(currentPrime, timesDivisible));
                }
            }
            
            return returnFactorisation;
        }
        
    }

    public static class DebugExtensions
    {
        public static IEnumerable<T> Debug<T>(this IEnumerable<T> valueEnumeration)
        {
            IEnumerator<T> enumerate = valueEnumeration.GetEnumerator();

            while (enumerate.MoveNext())
            {
                Trace.WriteLine(enumerate.Current);
                yield return enumerate.Current;
            }

            yield break;
        }
    }
}