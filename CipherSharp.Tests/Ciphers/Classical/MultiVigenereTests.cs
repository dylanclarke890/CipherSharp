using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class MultiVigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiVigenere.Encode(text, keys);

            // Assert
            Assert.Equal("GDKKNVNQKCGDKKNVNQKC", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "GDKKNVNQKCGDKKNVNQKC";
            string[] keys = new string[2] { "test", "key"};

            // Act
            var result = MultiVigenere.Decode(text, keys);

            // Assert
            Assert.Contains("HELLOWORLD", result);
        }
    }
}
