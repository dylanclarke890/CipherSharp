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
            Bifid bifid = new(text, key);
            // Act
            var result = bifid.Encode();

            // Assert
            Assert.Equal("CLURKWLEAK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CLURKWLEAK";
            string key = "test";
            Bifid bifid = new(text, key);
            // Act
            var result = bifid.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Bifid(text, key));
        }
    }
}
