using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SuperMath
{
    /// <summary>
    /// A decimal with base 3
    /// </summary>
    public class Decimal3 : IEquatable<Decimal3>
    {
        /// <summary>
        /// Determine if this item equals another item
        /// </summary>
        /// <param name="other">The other decimal to compare</param>
        /// <returns>Whether or not they are the same</returns>
        public bool Equals(Decimal3 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_value, other._value) && Equals(_parts, other._parts) && _isNegative == other._isNegative;
        }

        /// <summary>
        /// Compares to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Decimal3;
            if (other != null && Equals(other)) return true;
            var number = obj as long?;
            return number.HasValue && Equals(Parse(number.Value));
        }

        /// <summary>
        /// Get's a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _value?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (_parts?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ _isNegative.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Simple way to get -1
        /// </summary>
        public static Decimal3 Neg1 = Parse(-1);

        /// <summary>
        /// Simple way to get the identity (1)
        /// </summary>
        public static Decimal3 Identity = Parse(1);

        /// <summary>
        /// Simple way to get the smallest number
        /// </summary>
        public static Decimal3 MinValue = Parse(long.MinValue + 1);

        /// <summary>
        /// Simple way to get the largest number
        /// </summary>
        public static Decimal3 MaxValue = Parse(long.MaxValue - 1);

        /// <summary>
        /// The internal value
        /// </summary>
        private string _value;

        /// <summary>
        /// The internal remainders
        /// </summary>
        private Decimal3 _parts;

        /// <summary>
        /// Is this number negative?
        /// </summary>
        private bool _isNegative;

        /// <summary>
        /// The current value as a base10 number
        /// </summary>
        public long Value
        {
            get { return Utility.ArbitraryToDecimalSystem(_value, 3) * (_isNegative ? -1 : 1); }
            set { _value = Utility.DecimalToArbitrarySystem(value, 3); }
        }

        /// <summary>
        /// Create a new base3 number
        /// </summary>
        public Decimal3()
        {
            Value = 0;
            _isNegative = false;
        }

        /// <summary>
        /// Copy a base3 number
        /// </summary>
        /// <param name="value"></param>
        public Decimal3(Decimal3 value)
        {
            _value = value._value;
            _parts = value._parts;
            _isNegative = value._isNegative;
        }

        /// <summary>
        /// Parse a long to a base3 number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal3 Parse(long value)
        {
            return value < 0 ? new Decimal3() { _isNegative = true, Value = value * -1 } : new Decimal3() { Value = value };
        }

        /// <summary>
        /// Parse a char to a number3
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal3 Parse(char value)
        {
            return Parse(value.ToString());
        }

        /// <summary>
        /// Parse a string to a base3 number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal3 Parse(string value)
        {
            return new Decimal3() { _value = value };
        }

        /// <summary>
        /// Parse a char (plus sign) to a base3
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isNegative"></param>
        /// <returns></returns>
        public static Decimal3 Parse(char value, bool isNegative)
        {
            return new Decimal3() { _value = value.ToString(), _isNegative = isNegative };
        }

        /// <summary>
        /// Adds two numbers together using a table
        /// </summary>
        /// <param name="lh">The lefthand side</param>
        /// <param name="rh">The righthand side</param>
        /// <param name="result">Output the result</param>
        /// <param name="remainder">A stack to append any remainders to</param>
        private static void Add(char lh, char rh, out char result, Stack<char> remainder)
        {
            result = '0';
            switch (lh)
            {
                case '0':
                    result = rh;
                    break;
                case '1':
                    switch (rh)
                    {
                        case '0':
                            result = '1';
                            break;
                        case '1':
                            result = '2';
                            break;
                        case '2':
                            result = '0';
                            remainder.Push('1');
                            break;
                    }
                    break;
                case '2':
                    switch (rh)
                    {
                        case '0':
                            result = '2';
                            break;
                        case '1':
                            result = '0';
                            remainder.Push('1');
                            break;
                        case '2':
                            result = '1';
                            remainder.Push('1');
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Subtracks two numbers together using tables
        /// </summary>
        /// <param name="lh"></param>
        /// <param name="rh"></param>
        /// <param name="result"></param>
        /// <param name="needs"></param>
        private static void Subtract(char lh, char rh, out char result, Stack<char> needs)
        {
            result = '0';
            switch (lh)
            {
                case '0':
                    switch (rh)
                    {
                        case '0':
                            result = '0';
                            break;
                        case '1':
                            result = '2';
                            needs.Push('1');
                            break;
                        case '2':
                            result = '1';
                            needs.Push('1');
                            break;
                    }
                    break;
                case '1':
                    switch (rh)
                    {
                        case '0':
                            result = '1';
                            break;
                        case '1':
                            result = '0';
                            break;
                        case '2':
                            result = '2';
                            needs.Push('1');
                            break;
                    }
                    break;
                case '2':
                    switch (rh)
                    {
                        case '0':
                            result = '2';
                            break;
                        case '1':
                            result = '1';
                            break;
                        case '2':
                            result = '0';
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Multiplies two numbers together
        /// </summary>
        /// <param name="lh"></param>
        /// <param name="rh"></param>
        /// <param name="result"></param>
        /// <param name="carry"></param>
        private static void Multiply(Decimal3 lh, Decimal3 rh, out char result, Stack<char> carry)
        {
            var inc = new Decimal3();

            for (var i = 0; i < short.Parse(rh._value); i++)
            {
                inc += lh;
            }

            result = inc._value.Last();

            for (var i = 0; i < inc._value.Length - 1; i++)
            {
                carry.Push(inc._value[i]);
            }
        }

        /// <summary>
        /// Pads a number on the left side with 0's up to a certain point
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pad"></param>
        /// <returns></returns>
        private static string PadToMax(string input, int pad)
        {
            return input.Length < pad ? input.PadLeft(pad, '0') : input;
        }

        /// <summary>
        /// Splits a decimal3 into many decimal3s and reverses them
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static IEnumerable<Decimal3> SplitReverse(Decimal3 number)
        {
            return number._value.Reverse().Select(num => Parse(num, number._isNegative));
        }

        /// <summary>
        /// Splits a decimal3 into many decimal3s
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static IEnumerable<Decimal3> Split(Decimal3 number)
        {
            return number._value.Select(num => Parse(num, number._isNegative));
        }

        /// <summary>
        /// Divides a decimal3 by a long
        /// </summary>
        /// <param name="lh"></param>
        /// <param name="rh"></param>
        /// <returns></returns>
        public static Decimal3 operator /(Decimal3 lh, long rh)
        {
            return lh / Parse(rh);
        }

        /// <summary>
        /// Performs division on a decimal3
        /// </summary>
        /// <param name="lh"></param>
        /// <param name="rh"></param>
        /// <returns></returns>
        public static Decimal3 operator /(Decimal3 lh, Decimal3 rh)
        {
            var outer = rh;
            var partition = new Decimal3() { _value = "" };
            var answer = new Decimal3() { _value = "" };
            foreach (var inner in Split(lh))
            {
                partition._value += inner._value;

                var inc = new Decimal3();
                var count = 0;

                while (partition >= inc)
                {
                    count++;
                    inc += outer;
                }

                if (inc != partition)
                {
                    inc -= outer;
                    count--;
                }

                answer._value += Parse(count)._value;
                partition = partition - inc;
            }

            answer._parts = partition;

            return answer;
        }

        /// <summary>
        /// Multiply by a scalar
        /// </summary>
        /// <param name="lh"></param>
        /// <param name="rh"></param>
        /// <returns></returns>
        public static Decimal3 operator *(Decimal3 lh, long rh)
        {
            return lh * Parse(rh);
        }

        /// <summary>
        /// Multiplies a decimal3
        /// </summary>
        /// <param name="lefthand"></param>
        /// <param name="righthand"></param>
        /// <returns></returns>
        public static Decimal3 operator *(Decimal3 lefthand, Decimal3 righthand)
        {
            if (righthand._isNegative && righthand._value == "1")
            {
                return new Decimal3(lefthand) { _isNegative = !lefthand._isNegative };
            }

            if (lefthand._isNegative && lefthand._value == "1")
            {
                return righthand * lefthand;
            }

            var toAdd = new Stack<Decimal3>();
            var digit = -1;

            foreach (var bottom in SplitReverse(lefthand))
            {
                var carry = new Stack<char>();
                var result = new StringBuilder();
                digit++;

                for (var digitizer = 0; digitizer < digit; digitizer++) result.Append('0');

                foreach (var top in SplitReverse(righthand))
                {
                    char partialResult;
                    var temp = carry;
                    carry = new Stack<char>();

                    var l = bottom;
                    while (temp.Count > 0)
                    {
                        l += Parse(temp.Pop());
                    }

                    Multiply(l, top, out partialResult, carry);

                    result.Insert(0, partialResult);
                }

                while (carry.Count > 0)
                {
                    //Multiply(new Decimal3(carry.Pop()), bottom, out r, carry);
                    result.Insert(0, carry.Pop());
                }

                toAdd.Push(new Decimal3() { _value = result.ToString() });
            }

            var ret = new Decimal3();

            return toAdd.Aggregate(ret, (current, summer) => current + summer);

            /*
            var lh = lefthand._value;
            var rh = righthand._value;
            var max = Math.Max(lh.Length, rh.Length);

            lh = PadToMax(lh, max);
            rh = PadToMax(rh, max);

            var pl = lh.Reverse().ToList();
            var pr = rh.Reverse().ToList();

            var result = new StringBuilder();
            var carry = new Stack<char>();
            var hold = new Stack<Decimal3>();
            var threes = 0;

            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    var rha = pl[i];
                    var temp = carry;
                    carry = new Stack<char>();

                    while (temp.Count > 0)
                    {
                        Add(rha);
                    }
                }
                var rha = pl[i];
                var temp = carry;
                carry = new Stack<char>();

                while (temp.Count > 0)
                {
                    Add(rha, temp.Pop(), out rha, carry);
                }

                Multiply(rha, pr[i], out rha, carry);

                result.Insert(0, rha);
            }

            return new Decimal3(lefthand);*/
        }

        public static Decimal3 operator +(Decimal3 lh, long rh)
        {
            var r = Parse(rh);
            return lh + r;
        }

        public static Decimal3 operator +(Decimal3 lefthand, Decimal3 righthand)
        {
            var isPositive = !(lefthand._isNegative && righthand._isNegative);

            if (!lefthand._isNegative && righthand._isNegative)
            {
                return (lefthand - righthand * Neg1);
            }
            else if (lefthand._isNegative && !righthand._isNegative)
            {
                return (righthand - lefthand * Neg1);
            }

            var lh = lefthand._value;
            var rh = righthand._value;
            var max = Math.Max(lh.Length, rh.Length);

            lh = PadToMax(lh, max);
            rh = PadToMax(rh, max);

            var processlh = lh.Reverse().ToList();
            var processrh = rh.Reverse().ToList();

            var result = new StringBuilder();
            var carry = new Stack<char>();
            var threes = 0;

            for (var l = 0; l < processlh.Count; l++)
            {
                var rha = '0';

                var temp = carry;
                carry = new Stack<char>();

                // add carries from previous step
                while (temp.Count > 0)
                {
                    Add(rha, temp.Pop(), out rha, carry);
                }

                Add(rha, processlh[l], out rha, carry);
                Add(rha, processrh[l], out rha, carry);

                result.Insert(0, rha);
                threes += 1;
            }

            var hasCarry = (from v in carry
                            where v != '0'
                            select v).Count() != 0;

            if (!hasCarry) return new Decimal3() { _value = result.ToString(), _isNegative = !isPositive };

            var ret = new Decimal3() { _value = result.ToString() };

            while (carry.Count > 0)
            {
                ret = ret + new Decimal3() { _value = carry.Pop().ToString().PadRight(threes + 1, '0') };
            }

            return new Decimal3(ret) { _isNegative = !isPositive };
        }

        public static bool operator >=(Decimal3 lh, Decimal3 rh)
        {
            return lh > rh || lh == rh;
        }

        public static bool operator >=(Decimal3 lh, long rh)
        {
            return lh >= Parse(rh);
        }

        public static bool operator <=(Decimal3 lh, long rh)
        {
            return lh <= Parse(rh);
        }

        public static bool operator <=(Decimal3 lh, Decimal3 rh)
        {
            return lh < rh || lh == rh;
        }

        public static bool operator >(Decimal3 lh, long rh)
        {
            return lh > Parse(rh);
        }

        public static bool operator >(Decimal3 lefthand, Decimal3 righthand)
        {
            if (lefthand == righthand) return false;

            if (!lefthand._isNegative && righthand._isNegative)
            {
                return true;
            }

            /*if (lefthand._value.Length > righthand._value.Length)
            {
                return true;
            }*/

            var max = Math.Max(lefthand._value.Length, righthand._value.Length);

            var lh = lefthand._value.PadLeft(max, '0');
            var rh = righthand._value.PadLeft(max, '0');

            for (var i = 0; i < max; i++)
            {
                if (lh[i] > rh[i])
                {
                    return true;
                }
                else if (lh[i] < rh[i])
                {
                    break;
                }
            }

            return false;
        }

        public static bool operator <(Decimal3 lh, long rh)
        {
            return lh < Parse(rh);
        }

        public static bool operator <(Decimal3 lh, Decimal3 rh)
        {
            return rh > lh;
        }

        private Decimal3 Clean()
        {
            return new Decimal3() {_value = _value.Length > 0 ? _value.TrimStart('0') : _value};
        }

        public static bool operator ==(Decimal3 lh, long rh)
        {
            return lh == Parse(rh);
        }

        public static bool operator ==(Decimal3 lh, Decimal3 rh)
        {
            lh = lh.Clean();
            rh = rh.Clean();
            return lh.Equals(rh);
        }

        public static bool operator !=(Decimal3 lh, long rh)
        {
            return lh != Parse(rh);
        }

        public static bool operator !=(Decimal3 lh, Decimal3 rh)
        {
            return !(lh == rh);
        }

        public static Decimal3 operator -(Decimal3 lh, long rh)
        {
            return lh - Parse(rh);
        }

        public static Decimal3 operator -(Decimal3 lefthand, Decimal3 righthand)
        {
            if (lefthand < righthand)
            {
                return (righthand - lefthand) * Neg1;
            }

            var lh = lefthand._value;
            var rh = righthand._value;
            var digits = Math.Max(lh.Length, rh.Length);

            lh = PadToMax(lh, digits);
            rh = PadToMax(rh, digits);

            var plh = lh.Reverse().ToList();
            var prh = rh.Reverse().ToList();

            var result = new StringBuilder();
            var needs = new Stack<char>();

            for (var i = 0; i < digits; i++)
            {
                var rha = plh[i];
                var temp = needs;
                needs = new Stack<char>();

                while (temp.Count > 0)
                {
                    Subtract(plh[i], temp.Pop(), out rha, needs);
                }

                Subtract(rha, prh[i], out rha, needs);
                result.Insert(0, rha);
            }

            return new Decimal3() { _value = result.ToString() };
        }

        public override string ToString()
        {
            return $"{Value} :: {_value}";
        }
    }
}