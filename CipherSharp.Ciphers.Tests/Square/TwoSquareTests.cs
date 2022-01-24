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
            TwoSquare twoSquare = new(text, keys, mode);
            // Act
            var result = twoSquare.Encode();

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
            TwoSquare twoSquare = new(text, keys, mode);
            // Act
            var result = twoSquare.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string[] keys = { "test" };
            AlphabetMode mode = AlphabetMode.JI;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new TwoSquare(text, keys, mode));
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
            Assert.Throws<ArgumentNullException>(() => new TwoSquare(text, keys, mode));
        }
    }
}
