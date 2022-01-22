using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class CaesarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            int key = 1;

            // Act
            var result = Caesar.Encode(text, key);

            // Assert
            Assert.Equal("IFMMPXPSME", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "IFMMPXPSME";
            int key = 1;

            // Act
            var result = Caesar.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Encode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Caesar.Encode(text, key));
        }

        [Fact]
        public void Decode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Caesar.Decode(text, key));
        }
    }
}
