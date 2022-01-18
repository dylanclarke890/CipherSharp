using CipherSharp.Ciphers.Transposition;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class DisruptedTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";
            bool complete = false;

            // Act
            var result = Disrupted.Encode(text, key, complete);

            // Assert
            Assert.Equal("elrolhlwod", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "elrolhlwod";
            string key = "test";
            bool complete = false;

            // Act
            var result = Disrupted.Decode(text, key, complete);

            // Assert
            Assert.Equal("helloworld", result);
        }
    }
}
