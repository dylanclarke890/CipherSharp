using CipherSharp.Ciphers.Square;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Square
{
    public class FourSquareTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = { "abc", "abc" };
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Encode(text, keys, mode, displaySquare);

            // Assert
            Assert.Equal("KCLLMYMTOA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "KCLLMYMTOA";
            string[] keys = { "abc", "abc" };
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Decode(text, keys, mode, displaySquare);

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
            Assert.Throws<ArgumentException>(() => FourSquare.Encode(text, keys, mode, displaySquare));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2]{ "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => FourSquare.Decode(text, keys, mode, displaySquare));
        }
    }
}
