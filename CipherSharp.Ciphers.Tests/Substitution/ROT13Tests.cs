using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class ROT13Tests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = ROT13.Encode(text);

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = ROT13.Decode(text);

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }

        [Fact]
        public void Encode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => ROT13.Encode(text));
        }

        [Fact]
        public void Decode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => ROT13.Decode(text));
        }
    }
}
