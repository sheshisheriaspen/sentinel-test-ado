using System;

namespace SentinelTestAdo
{
    /// <summary>
    /// FIXED VERSION - Corrected implementation with best practices
    /// </summary>
    public class FixedCalculator
    {
        /// <summary>
        /// Calculate sum of 1 to n - FIXED
        /// </summary>
        public static int sum_to_n(int n)
        {
            if (n < 0)
                throw new ArgumentException("n must be non-negative", nameof(n));

            int sum = 0;
            for (int i = 1; i <= n; i++)  // FIXED: now includes n
            {
                sum += i;
            }
            return sum;
        }

        /// <summary>
        /// Divide two numbers safely - FIXED
        /// Throws ArgumentException if denominator is zero
        /// </summary>
        public static double safe_divide(int a, int b)
        {
            if (b == 0)
                throw new ArgumentException("Denominator cannot be zero", nameof(b));

            return (double)a / b;
        }
    }
}
