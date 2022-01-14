using CipherSharp.Extensions;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void CartesianProduct_TwoStrings_ReturnsCartesianProductOfStrings()
        {
            // Arrange
            string firstString = "123";
            string secondString = "567";

            // Act
            var result = firstString.CartesianProduct(secondString);

            // Assert
            List<List<string>> expected = new()
            {
                new() { "1", "5" },
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
            var result = firstString.CartesianProduct(secondString, thirdString);

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
            var result = text.SplitIntoChunks(chunkSize);

            // Assert
            List<string> expected = new() { "1", "2", "3" };
            Assert.Equal(expected, result);
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
            var result = text.PadText(totalLength, fromString, alphabet);

            // Assert
            Assert.Equal("testxx", result);
        }
    }
}
