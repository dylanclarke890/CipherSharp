using CipherSharp.Enums;
using CipherSharp.Helpers;
using System.Collections.Generic;
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
        public void CartesianProduct_TwoStrings_ReturnsCartesianProductOfStrings()
        {
            // Arrange
            string firstString = "123";
            string secondString = "567";

            // Act
            var result = Utilities.CartesianProduct(firstString, secondString);

            // Assert
            List<List<string>> expected = new() 
            { 
                new() { "1", "5"},
                new() { "1", "6" },
                new() { "1", "7" },
                new() { "2", "5" },
                new() { "2", "6" },
                new() { "2", "7" },
                new() { "3", "5" },
                new() { "3", "6" },
                new() { "3", "7" },
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CartesianProduct_ThreeStrings_ReturnsCartesianProductOfStrings()
        {
            // Arrange
            string firstString = "012";
            string secondString = "345";
            string thirdString = "678";

            // Act
            var result = Utilities.CartesianProduct(firstString, secondString, thirdString);

            // Assert
            List<List<string>> expected = new()
            {
                new() { "0", "3", "6" },
                new() { "0", "3", "7" },
                new() { "0", "3", "8" },
                new() { "0", "4", "6" },
                new() { "0", "4", "7" },
                new() { "0", "4", "8" },
                new() { "0", "5", "6" },
                new() { "0", "5", "7" },
                new() { "0", "5", "8" },
                new() { "1", "3", "6" },
                new() { "1", "3", "7" },
                new() { "1", "3", "8" },
                new() { "1", "4", "6" },
                new() { "1", "4", "7" },
                new() { "1", "4", "8" },
                new() { "1", "5", "6" },
                new() { "1", "5", "7" },
                new() { "1", "5", "8" },
                new() { "2", "3", "6" },
                new() { "2", "3", "7" },
                new() { "2", "3", "8" },
                new() { "2", "4", "6" },
                new() { "2", "4", "7" },
                new() { "2", "4", "8" },
                new() { "2", "5", "6" },
                new() { "2", "5", "7" },
                new() { "2", "5", "8" },
            };

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SplitIntoChunks_BasicParameters_ReturnsExpectedListOfStrings()
        {
            // Arrange
            string text = "123";
            int chunkSize = 1;

            // Act
            var result = Utilities.SplitIntoChunks(text, chunkSize);

            // Assert
            List<string> expected = new() { "1", "2", "3" };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateMatrix_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string initialKey = "test";
            AlphabetMode mode = AlphabetMode.EX;

            // Act
            var result = Utilities.CreateMatrix(initialKey, mode);

            // Assert
            List<List<string>> expected = new()
            { 
                new() { "TESABC" },
                new() { "DFGHIJ" },
                new() { "KLMNOP" },
                new() { "QRUVWX" },
                new() { "YZ0123" },
                new() { "456789" },
            };
            Assert.Equal(expected, result);
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

        [Fact]
        public void PadText_BasicParameters_ReturnsPaddedString()
        {
            // Arrange
            string text = "test";
            int totalLength = 6;
            string fromString = "xx";
            string alphabet = "";

            // Act
            var result = Utilities.PadText(text, totalLength, fromString, alphabet);

            // Assert
            Assert.Equal("testxx", result);
        }

        [Fact]
        public void IndirectSort_BasicParameters_ReturnsListOfIndicesToSortInput()
        {
            // Arrange
            IEnumerable<string> array = new List<string>() { "B", "C", "A" };

            // Act
            var result = array.IndirectSort();

            // Assert
            Assert.Equal(new List<int>() { 2, 0, 1 }, result);
        }

        [Fact]
        public void IndexWhere_BasicParameters_ReturnsListOfIndicesThatFitCriteria()
        {
            // Arrange
            IEnumerable<string> array = new List<string>() { "B", "C", "A" };

            // Act
            var result = array.IndexWhere(x => x == "B" || x == "C" );

            // Assert
            Assert.Equal(new List<int>() { 0, 1 }, result);
        }
    }
}
