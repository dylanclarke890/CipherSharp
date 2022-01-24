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
            Polybius polybius = new(text, initialKey, sep, mode);
            // Act
            var result = polybius.Encode();

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
            Polybius polybius = new(text, initialKey, sep, mode);
            // Act
            var result = polybius.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData(null, "test", "")]
        [InlineData("helloworld", null, "")]
        [InlineData("helloworld", "test", null)]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key, string sep)
        {
            // Arrange
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<ArgumentException>(() => new Polybius(text, key, sep, mode));
        }

        [Fact]
        public void Encode_PlainTextContainsDigits_ThrowsInvalidOperationException()
        {
            // Arrange
            string text = "helloworld1";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<InvalidOperationException>(() => new Polybius (text, initialKey, sep, mode).Encode());
        }

        [Fact]
        public void Decode_CipherTextContainsLetters_ThrowsInvalidOperationException()
        {
            // Arrange
            string text = "25123333415241443322h";
            string initialKey = "test";
            string sep = "";
            AlphabetMode mode = AlphabetMode.JI;

            // Act - done as part of assert

            // Assert
            Assert.Throws<InvalidOperationException>(() => new Polybius(text, initialKey, sep, mode).Decode());
        }
    }
}
