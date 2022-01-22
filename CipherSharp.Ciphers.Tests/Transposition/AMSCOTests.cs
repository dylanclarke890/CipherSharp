using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class AMSCOTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string key = "TEST";
            ParityMode mode = ParityMode.Odd;

            // Act
            var result = AMSCO.Encode(text, key, mode);

            // Assert
            Assert.Equal("ELRLLDHOOW", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "ELRLLDHOOW";
            string key = "TEST";
            ParityMode mode = ParityMode.Odd;

            // Act
            var result = AMSCO.Decode(text, key, mode);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            ParityMode mode = ParityMode.Odd;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => AMSCO.Decode(text, key, mode));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            ParityMode mode = ParityMode.Odd;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => AMSCO.Decode(text, key, mode));
        }
    }
}
