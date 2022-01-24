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
            ROT13 rOT13 = new(text);
            // Act
            var result = rOT13.Encode();

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "helloworld";
            ROT13 rOT13 = new(text);
            // Act
            var result = rOT13.Decode();

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }

        [Fact]
        public void NewInstance_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new ROT13(text));
        }
    }
}
