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
            Playfair playfair = new(text, key, mode);
            // Act
            var result = playfair.Encode();

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
            Playfair playfair = new(text, key, mode);
            // Act
            var result = playfair.Decode();

            // Assert
            Assert.Equal("HELXLOWORLDX", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Playfair(text, key, mode));
        }
    }
}
