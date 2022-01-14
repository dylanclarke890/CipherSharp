using CipherSharp.Helpers;
using Xunit;

namespace CipherSharp.Tests.Helpers
{
    public class UtilitiesTests
    {
        [Fact]
        public void AlphabetPermutation_BasicParameters_ReturnsPermutatedAlphabet()
        {
            // Arrange
            string key = "test";
            string alphabet = AppConstants.Alphabet;

            // Act
            var result = Utilities.AlphabetPermutation(key, alphabet);

            // Assert
            Assert.Equal("TESABCDFGHIJKLMNOPQRUVWXYZ", result);
        }

        [Fact]
        public void UniqueRank_IntArray_ReturnsRankOfArray()
        {
            // Arrange
            int[] numbers = new int[8] { 1, 2, 2, 3, 2, 2, 2, 1 };

            // Act
            var result = numbers.UniqueRank();

            // Assert
            var expected = new int[8] { 0, 2, 3, 7, 4, 5, 6, 1 };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void DivMod_BasicParameters_ReturnsCorrectValues()
        {
            // Arrange
            int dividend = 6;
            int divisor = 2;

            // Act
            var result = Utilities.DivMod(dividend, divisor);

            // Assert
            Assert.Equal(3, result.Item1);
            Assert.Equal(0, result.Item2);
        }
    }
}
