using CipherSharp.Enums;
using CipherSharp.Helpers;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Helpers
{
    public class UtilitiesTests
    {
        [Fact]
        public void AlphabetPermutation_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string key = null;
            string alphabet = null;

            // Act
            var result = Utilities.AlphabetPermutation(key, alphabet);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void CartesianProduct_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string firstString = null;
            string secondString = null;

            // Act
            var result = Utilities.CartesianProduct(firstString, secondString);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void CartesianProduct_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            string firstString = null;
            string secondString = null;
            string thirdString = null;

            // Act
            var result = Utilities.CartesianProduct(firstString, secondString, thirdString);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void SplitIntoChunks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = null;
            int chunkSize = 0;

            // Act
            var result = Utilities.SplitIntoChunks(text, chunkSize);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void CreateMatrix_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string initialKey = null;
            PolybiusMode mode = default;

            // Act
            var result = Utilities.CreateMatrix(initialKey, mode);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void UniqueOccurences_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            int[] numbers = null;

            // Act
            var result = Utilities.UniqueOccurences(numbers);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void DivMod_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            int dividend = 0;
            int divisor = 0;

            // Act
            var result = Utilities.DivMod(dividend, divisor);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void PadText_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = null;
            int totalLength = 0;
            string fromString = null;
            string alphabet = null;

            // Act
            var result = Utilities.PadText(text, totalLength, fromString, alphabet);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void IndirectSort_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            IEnumerable<string> array = new List<string>() { "B", "C", "A" };

            // Act
            var result = Utilities.IndirectSort(array);

            // Assert
            Assert.Equal(new List<int>() { 2, 0, 1 }, result);
        }
    }
}
