namespace ProjectEuler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct BigNumber
    {
        private string bigNumberValue;

        public BigNumber(int startingValue)
        {
            if (startingValue < 0)
            {
                throw new ArgumentException("BigNumber values can only be positive.");
            }

            bigNumberValue = startingValue.ToString();
        }

        public BigNumber(long startingValue)
        {
            if (startingValue < 0)
            {
                throw new ArgumentException("BigNumber values can only be positive.");
            }

            bigNumberValue = startingValue.ToString();
        }

        public BigNumber(string startingValue)
        {
            // Performance improvement
            //if (!startingValue.All(c => char.IsDigit(c)))
            //{
            //    throw new InvalidOperationException("'value' is not a numeric string.");
            //}

            bigNumberValue = startingValue;
        }

        public BigNumber(IEnumerable<int> startingValue)
        {
            bigNumberValue = new string(startingValue.Select(d => d.ConvertToChar()).ToArray());
        }

        public IEnumerable<int> Digits()
        {
            for (int i = 0; i < this.bigNumberValue.Length; i++)
            {
                yield return int.Parse(this.bigNumberValue[i].ToString());
            }

            yield break;
        }

        public int NumberOfDigits()
        {
            return this.bigNumberValue.Length;
        }

        public BigNumber Factorial()
        {
            return Factorial(this);
        }

        private BigNumber Factorial(BigNumber value)
        {
            if (value == 0)
            {
                return 1;
            }
            else
            {
                return value * this.Factorial(value - 1);
            }
        }

        public BigNumber Append(BigNumber number)
        {
            this.bigNumberValue = this.bigNumberValue + number.bigNumberValue;
            return this;
        }

        public override string ToString()
        {
            return this.bigNumberValue;
        }

        public static BigNumber operator ^(BigNumber first, BigNumber second)
        {
            BigNumber returnValue = 1;
            Console.WriteLine("{0}^{1}", first, second);

            for (int i = 0; i < second; i++)
            {
                returnValue = returnValue * first;
            }
           
            return returnValue;
        }

        public static BigNumber operator ^(BigNumber first, long second)
        {
            BigNumber returnValue = 1;
            Console.WriteLine("{0}^{1}", first, second);

            while (second != 0)
            {
                if (second % 2 != 0)
                {
                    returnValue = returnValue * first;
                    second = second - 1;
                }

                first = first*first;
                second = second/2;
            }

            return returnValue;
        }

        public static BigNumber operator ++(BigNumber value)
        {
            return value += 1;
        }

        public static BigNumber operator *(BigNumber first, BigNumber second)
        {
            BigNumber returnValue = 0;

            BigNumber multiplacand;
            BigNumber multiplier;

            if (first < second)
            {
                multiplier = first;
                multiplacand = second;
            }
            else
            {
                multiplier = second;
                multiplacand = first;
            }

            for (int i = 0; i < multiplier.bigNumberValue.Length; i++)
            {
                int a = 0;
                int b = 0;

                if (multiplier.bigNumberValue.Length - i - 1 >= 0)
                {
                    a = multiplier.bigNumberValue[multiplier.bigNumberValue.Length - i - 1].ConvertToInt();
                }

                List<char> returnString = new List<char>();
                int carry = 0;

                // Add 0's for the correct number of places.
                for (int z = 0; z < i; z++)
                {
                    returnString.Add('0');
                }

                for (int j = 0; j < multiplacand.bigNumberValue.Length; j++)
                {
                    if (multiplacand.bigNumberValue.Length - j - 1 >= 0)
                    {
                        b = multiplacand.bigNumberValue[multiplacand.bigNumberValue.Length - j - 1].ConvertToInt();
                    }

                    int result = a * b + carry;

                    if (result > 9)
                    {
                        carry = result / 10;
                        result = result % 10;
                    }
                    else
                    {
                        carry = 0;
                    }

                    returnString.Add(result.ConvertToChar());
                }

                if (carry > 0)
                {
                    returnString.Add(carry.ConvertToChar());
                    carry = 0;
                }

                returnString.Reverse();
                returnValue += new BigNumber(new string(returnString.ToArray()));
            }

            return returnValue;
        }

        public static BigNumber operator +(BigNumber first, BigNumber second)
        {
            List<char> returnString = new List<char>();

            int carry = 0;
            int biggestNumber = Math.Max(first.bigNumberValue.Length, second.bigNumberValue.Length);

            for (int i = 0; i < biggestNumber; i++)
            {
                int a = 0;
                int b = 0;

                if (first.bigNumberValue.Length - i - 1 >= 0)
                {
                    a = first.bigNumberValue[first.bigNumberValue.Length - i - 1].ConvertToInt();
                }

                if (second.bigNumberValue.Length - i - 1 >= 0)
                {
                    b = second.bigNumberValue[second.bigNumberValue.Length - i - 1].ConvertToInt();
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

                returnString.Add(result.ConvertToChar());
            }

            if (carry > 0)
            {
                returnString.Add(carry.ConvertToChar());
            }

            returnString.Reverse();
            return new BigNumber(new string(returnString.ToArray()));
        }

        public static bool operator <(BigNumber first, BigNumber second)
        {
            if (first.bigNumberValue.Length != second.bigNumberValue.Length)
            {
                return first.bigNumberValue.Length < second.bigNumberValue.Length;
            }

            for (int i = 0; i < first.bigNumberValue.Length; i++)
            {
                int a = first.bigNumberValue[i].ConvertToInt();
                int b = second.bigNumberValue[i].ConvertToInt();

                if (a == b)
                {
                    continue;
                }
                
                return a < b;
            }

            return false;
        }

        public static bool operator >(BigNumber first, BigNumber second)
        {
            if (first.bigNumberValue.Length != second.bigNumberValue.Length)
            {
                return first.bigNumberValue.Length > second.bigNumberValue.Length;
            }

            for (int i = 0; i < first.bigNumberValue.Length; i++)
            {
                int a = first.bigNumberValue[i].ConvertToInt();
                int b = second.bigNumberValue[i].ConvertToInt();

                if (a == b)
                {
                    continue;
                }

                return a > b;
            }

            return false;
        }

        public static bool operator == (BigNumber first, BigNumber second)
        {
            return first.bigNumberValue == second.bigNumberValue;
        }

        public static bool operator !=(BigNumber first, BigNumber second)
        {
            return !(first == second);
        }

        public static implicit operator BigNumber(string value)
        {
            return new BigNumber(value);
        }

        public static implicit operator BigNumber(int value)
        {
            return new BigNumber(value);
        }
        
        public static implicit operator BigNumber(long value)
        {
            return new BigNumber(value);
        }

        public static implicit operator int(BigNumber value)
        {
            int returnValue;
            if (int.TryParse(value.bigNumberValue, out returnValue))
            {
                return returnValue;
            }
                
            throw new InvalidCastException("Number to big to cast to int.");
        }

        public static implicit operator System.Int64(BigNumber value)
        {
            System.Int64 returnValue;
            if (System.Int64.TryParse(value.bigNumberValue, out returnValue))
            {
                return returnValue;
            }

            throw new InvalidCastException("Number to big to cast to int.");
        }

    }
}