using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class BeaufortTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = Beaufort.Encode(text, key);

            // Assert
            Assert.Equal("MAHIFIECIQ", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "MAHIFIECIQ";
            string key = "test";

            // Act
            var result = Beaufort.Decode(text, key);

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
            Assert.Throws<ArgumentException>(() => Beaufort.Encode(text, key));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Beaufort.Decode(text, key));
        }
    }
}
