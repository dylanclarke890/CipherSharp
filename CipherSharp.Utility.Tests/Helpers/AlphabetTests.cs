using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CipherSharp.Tests.Helpers
{
    public class AlphabetTests
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [Fact]
        public void AlphabetPermutation_BasicParameters_ReturnsPermutatedAlphabet()
        {
            // Arrange
            string key = "test";

            // Act
            var result = Alphabet.AlphabetPermutation(key, Alpha);

            // Assert
            Assert.Equal("TESABCDFGHIJKLMNOPQRUVWXYZ", result);
        }

        [Fact]
        public void ToLetter_FullRangeOfNumbers_ReturnsAlphabet()
        {
            int[] numbers = new int[52] {-26, -25, -24, -23, -22, -21, -20, -19, -18, -17, -16, -15, -14, -13, -12, -11, -10, -9, -8,
            -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25 };

            var result = numbers.ToLetter(Alpha).ToList();

            List<char> expected = Alpha.ToList();
            expected.AddRange(Alpha.ToList());

            Assert.Equal(expected, result);
        }
    }
}
