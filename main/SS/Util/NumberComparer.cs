﻿using System;
using NPOI.Util;

namespace NPOI.SS.Util
{
    public class NumberComparer
    {
        /// <summary>
        /// <para>
        /// This class attempts to reproduce Excel's behaviour for comparing numbers.  Results are
        /// mostly the same as those from <see cref="Double.compare(double, double)" /> but with some
        /// rounding.  For numbers that are very close, this code converts to a format having 15
        /// decimal digits of precision and a decimal exponent, before completing the comparison.
        /// </para>
        /// <para>
        /// In Excel formula evaluation, expressions like "(0.06-0.01)=0.05" evaluate to "TRUE" even
        /// though the equivalent java expression is <c>false</c>.  In examples like this,
        /// Excel achieves the effect by having additional logic for comparison operations.
        /// </para>
        /// <para>
        /// 
        /// </para>
        /// <para>
        /// Note - Excel also gives special treatment to expressions like "0.06-0.01-0.05" which
        /// evaluates to "0" (in java, rounding anomalies give a result of 6.9E-18).  The special
        /// behaviour here is for different reasons to the example above:  If the last operator in a
        /// cell formula is '+' or '-' and the result is less than 2<sup>50</sup> times smaller than
        /// first operand, the result is rounded to zero.
        /// Needless to say, the two rules are not consistent and it is relatively easy to find
        /// examples that satisfy<br/>
        /// "A=B" is "TRUE" but "A-B" is not "0"<br/>
        /// and<br/>
        /// "A=B" is "FALSE" but "A-B" is "0"<br/>
        /// <br/>
        /// This rule (for rounding the result of a final addition or subtraction), has not been
        /// implemented in POI (as of Jul-2009).
        /// </para>
        /// </summary>
        /// <return><c>negative, 0, or positive</c> according to the standard Excel comparison
        /// of values <c>a</c> and <c>b</c>.
        /// </return>
        public static int Compare(double a, double b)
        {
            long rawBitsA = BitConverter.DoubleToInt64Bits(a);
            long rawBitsB = BitConverter.DoubleToInt64Bits(b);

            int biasedExponentA = IEEEDouble.GetBiasedExponent(rawBitsA);
            int biasedExponentB = IEEEDouble.GetBiasedExponent(rawBitsB);

            if(biasedExponentA == IEEEDouble.BIASED_EXPONENT_SPECIAL_VALUE)
            {
                throw new ArgumentException("Special double values are not allowed: " + ToHex(a));
            }
            if(biasedExponentB == IEEEDouble.BIASED_EXPONENT_SPECIAL_VALUE)
            {
                throw new ArgumentException("Special double values are not allowed: " + ToHex(a));
            }

            int cmp;

            // sign bit is in the same place for long and double:
            bool aIsNegative = rawBitsA < 0;
            bool bIsNegative = rawBitsB < 0;

            // compare signs
            if(aIsNegative != bIsNegative)
            {
                // Excel seems to have 'normal' comparison behaviour around zero (no rounding)
                // even -0.0 < +0.0 (which is not quite the initial conclusion of bug 47198)
                return aIsNegative ? -1 : +1;
            }

            // then compare magnitudes (IEEE 754 has exponent bias specifically to allow this)
            cmp = biasedExponentA - biasedExponentB;
            int absExpDiff = Math.Abs(cmp);
            if(absExpDiff > 1)
            {
                return aIsNegative ? -cmp : cmp;
            }

            if(absExpDiff == 1)
            {
                // special case exponent differs by 1.  There is still a chance that with rounding the two quantities could end up the same

            }
            else
            {
                // else - sign and exponents equal
                if(rawBitsA == rawBitsB)
                {
                    // fully equal - exit here
                    return 0;
                }
            }
            if(biasedExponentA == 0)
            {
                if(biasedExponentB == 0)
                {
                    return CompareSubnormalNumbers(rawBitsA & IEEEDouble.FRAC_MASK, rawBitsB & IEEEDouble.FRAC_MASK, aIsNegative);
                }
                // else biasedExponentB is 1
                return -CompareAcrossSubnormalThreshold(rawBitsB, rawBitsA, aIsNegative);
            }
            if(biasedExponentB == 0)
            {
                // else biasedExponentA is 1
                return +CompareAcrossSubnormalThreshold(rawBitsA, rawBitsB, aIsNegative);
            }

            // sign and exponents same, but fractional bits are different

            ExpandedDouble edA = ExpandedDouble.FromRawBitsAndExponent(rawBitsA, biasedExponentA - IEEEDouble.EXPONENT_BIAS);
            ExpandedDouble edB = ExpandedDouble.FromRawBitsAndExponent(rawBitsB, biasedExponentB - IEEEDouble.EXPONENT_BIAS);
            NormalisedDecimal ndA = edA.NormaliseBaseTen().RoundUnits();
            NormalisedDecimal ndB = edB.NormaliseBaseTen().RoundUnits();
            cmp = ndA.CompareNormalised(ndB);
            if(aIsNegative)
            {
                return -cmp;
            }
            return cmp;
        }

        /// <summary>
        /// If both numbers are subnormal, Excel seems to use standard comparison rules
        /// </summary>
        private static int CompareSubnormalNumbers(long fracA, long fracB, bool isNegative)
        {
            int cmp = fracA > fracB ? +1 : fracA < fracB ? -1 : 0;

            return isNegative ? -cmp : cmp;
        }



        /// <summary>
        /// Usually any normal number is greater (in magnitude) than any subnormal number.
        /// However there are some anomalous cases around the threshold where Excel produces screwy results
        /// </summary>
        /// <param name="isNegative">both values are either negative or positive. This parameter affects the sign of the comparison result</param>
        /// <return>usually <c>isNegative ? -1 : +1</c></return>
        private static int CompareAcrossSubnormalThreshold(long normalRawBitsA, long subnormalRawBitsB, bool isNegative)
        {
            long fracB = subnormalRawBitsB & IEEEDouble.FRAC_MASK;
            if(fracB == 0)
            {
                // B is zero, so A is definitely greater than B
                return isNegative ? -1 : +1;
            }
            long fracA = normalRawBitsA & IEEEDouble.FRAC_MASK;
            if(fracA <= 0x0000000000000007L && fracB >= 0x000FFFFFFFFFFFFAL)
            {
                // Both A and B close to threshold - weird results
                if(fracA == 0x0000000000000007L && fracB == 0x000FFFFFFFFFFFFAL)
                {
                    // special case
                    return 0;
                }
                // exactly the opposite
                return isNegative ? +1 : -1;
            }
            // else - typical case A and B is not close to threshold
            return isNegative ? -1 : +1;
        }



        /// <summary>
        /// for formatting double values in error messages
        /// </summary>
        private static String ToHex(double a)
        {
            return "0x" + StringUtil.ToHexString(BitConverter.DoubleToInt64Bits(a)).ToUpper();
        }
    }
}
