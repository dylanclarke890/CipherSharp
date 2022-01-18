using CipherSharp.Ciphers.Polyalphabetic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class VigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = Vigenere.Encode(text, key);

            // Assert
            Assert.Equal("GDKKNVNQKC", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "GDKKNVNQKC";
            string key = "test";

            // Act
            var result = Vigenere.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
