using CipherSharp.Ciphers.Substitution;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class RunningKeyTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string key = "test";

            // Act
            var result = RunningKey.Encode(text, key);

            // Assert
            Assert.Equal("AXEEJRJMIA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "AXEEJRJMIA";
            string key = "test";

            // Act
            var result = RunningKey.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
