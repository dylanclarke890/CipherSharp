using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class AffineVigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key"};

            // Act
            var result = AffineVigenere.Encode(text, keys);

            // Assert
            Assert.Equal("CYLIZXRRCK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CYLIZXRRCK";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = AffineVigenere.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
