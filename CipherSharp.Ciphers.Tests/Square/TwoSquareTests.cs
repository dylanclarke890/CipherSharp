using CipherSharp.Ciphers.Square;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Square
{
    public class TwoSquareTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.JI;
            bool printKey = false;

            // Act
            var result = TwoSquare.Encode(text, keys, mode, printKey);

            // Assert
            Assert.Equal("HEQQOWVWLD", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HEQQOWVWLD";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.JI;
            bool printKey = false;

            // Act
            var result = TwoSquare.Decode(text, keys, mode, printKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => TwoSquare.Encode(text, keys, mode, displaySquare));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => TwoSquare.Decode(text, keys, mode, displaySquare));
        }
    }
}
