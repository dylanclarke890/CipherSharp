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
            FourSquare fourSquare = new(text, keys, mode);
            // Act
            var result = fourSquare.Encode();

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
            FourSquare fourSquare = new(text, keys, mode);
            // Act
            var result = fourSquare.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string[] keys = { "test", "key" };
            AlphabetMode mode = AlphabetMode.JI;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new FourSquare(text, keys, mode));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = null;
            AlphabetMode mode = AlphabetMode.JI;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new FourSquare(text, keys, mode));
        }
    }
}
