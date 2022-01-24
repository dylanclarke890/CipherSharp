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
            Caesar caesar = new(text, key);
            // Act
            var result = caesar.Encode();

            // Assert
            Assert.Equal("IFMMPXPSME", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "IFMMPXPSME";
            int key = 1;
            Caesar caesar = new(text, key);

            // Act
            var result = caesar.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Caesar(text, key));
        }
    }
}
