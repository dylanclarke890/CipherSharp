using CipherSharp.Ciphers.Transposition;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class RouteTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            int key = 4;

            // Act
            var result = Route.Encode(text, key);

            // Assert
            Assert.Equal("holdweloXXrl", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "holdweloXXrl";
            int key = 4;

            // Act
            var result = Route.Decode(text, key);

            // Assert
            Assert.Equal("helloworldXX", result);
        }
    }
}
