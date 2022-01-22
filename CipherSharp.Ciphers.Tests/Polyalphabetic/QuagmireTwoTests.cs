using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class QuagmireTwoTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            // Act
            var result = Quagmire.Two.Encode(text, keys);

            // Assert
            Assert.Equal("RCHXNUTQHN", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "RCHXNUTQHN";
            string[] keys = new string[2] { "test", "key" };
            // Act
            var result = Quagmire.Two.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, new string[1] { "test" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Quagmire.Two.Decode(text, keys));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, new string[1] { "test" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Quagmire.Two.Decode(text, keys));
        }
    }
}
