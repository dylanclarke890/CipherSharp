using CipherSharp.Ciphers.Square;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Square
{
    public class PlayfairTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "abc";
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = Playfair.Encode(text, key, mode, displaySquare);

            // Assert
            Assert.Equal("KCNVMPYMQMCY", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "KCNVMPYMQMCY";
            string key = "abc";
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = Playfair.Decode(text, key, mode, displaySquare);

            // Assert
            Assert.Equal("HELXLOWORLDX", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Playfair.Encode(text, key, mode, displaySquare));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Playfair.Decode(text, key, mode, displaySquare));
        }
    }
}
