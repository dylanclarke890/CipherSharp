using CipherSharp.Ciphers.Transposition;
using CipherSharp.Enums;
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
    }
}
