using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class RailFenceTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            int key = 3;

            // Act
            var result = RailFence.Encode(text, key);

            // Assert
            Assert.Equal("holelwrdlo", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "holelwrdlo";
            int key = 3;

            // Act
            var result = RailFence.Decode(text, key);

            // Assert
            Assert.Equal("helloworld", result);
        }
    }
}
