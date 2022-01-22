using CipherSharp.Ciphers.PolybiusSquare;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.PolybiusSquare
{
    public class PolybiusTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act
            var result = Polybius.Encode(text, initialKey, sep, mode);

            // Assert
            Assert.Equal("25123333415241443322", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "25123333415241443322";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act
            var result = Polybius.Decode(text, initialKey, sep, mode);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData(null, "test", "")]
        [InlineData("helloworld", null, "")]
        [InlineData("helloworld", "test", null)]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key, string sep)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<ArgumentException>(() => Polybius.Encode(text, key, sep, mode));
        }

        [Theory]
        [InlineData(null, "test", "")]
        [InlineData("25123333415241443322", null, "")]
        [InlineData("25123333415241443322", "test", null)]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key, string sep)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<ArgumentException>(() => Polybius.Decode(text, key, sep, mode));
        }

        [Fact]
        public void Encode_PlainTextContainsDigits_ThrowsArgumentException()
        {
            // Arrange
            string text = "helloworld1";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<ArgumentException>(() => Polybius.Encode(text, initialKey, sep, mode));
        }

        [Fact]
        public void Decode_CipherTextContainsLetters_ThrowsArgumentException()
        {
            // Arrange
            string text = "25123333415241443322h";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<ArgumentException>(() => Polybius.Decode(text, initialKey, sep, mode));
        }
    }
}
