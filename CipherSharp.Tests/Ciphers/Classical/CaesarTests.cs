using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class CaesarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            int key = 1;

            // Act
            var result = Caesar.Encode(text, key);

            // Assert
            Assert.Equal("IFMMPXPSME", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "IFMMPXPSME";
            int key = 1;

            // Act
            var result = Caesar.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
