using CipherSharp.Ciphers.PolybiusSquare;
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
    }
}
