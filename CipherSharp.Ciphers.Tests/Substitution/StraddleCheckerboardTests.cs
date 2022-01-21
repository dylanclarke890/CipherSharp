using CipherSharp.Ciphers.Substitution;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class StraddleCheckerboardTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string initialKey = "test";
            int[] keys = new int[2] { 0, 1};

            // Act
            var result = StraddleCheckerboard.Encode(text, initialKey, keys);

            // Assert
            Assert.Equal("013050508140811058", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "013050508140811058";
            string initialKey = "test";
            int[] keys = new int[2] { 0, 1 };

            // Act
            var result = StraddleCheckerboard.Decode(text, initialKey, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
