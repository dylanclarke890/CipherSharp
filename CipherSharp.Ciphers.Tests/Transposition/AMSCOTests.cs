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
            AMSCO aMSCO = new(text, key, mode);
            // Act
            var result = aMSCO.Encode();

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
            AMSCO aMSCO = new(text, key, mode);
            // Act
            var result = aMSCO.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            ParityMode mode = ParityMode.Odd;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new AMSCO(text, key, mode));
        }
    }
}
