using CipherSharp.Ciphers.PolybiusSquare;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.PolybiusSquare
{
    public class BifidTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = Bifid.Encode(text, key);

            // Assert
            Assert.Equal("CLURKWLEAK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CLURKWLEAK";
            string key = "test";

            // Act
            var result = Bifid.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Bifid.Encode(text, key));
        }

        [Theory]
        [InlineData("CLURKWLEAK", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Bifid.Decode(text, key));
        }
    }
}
