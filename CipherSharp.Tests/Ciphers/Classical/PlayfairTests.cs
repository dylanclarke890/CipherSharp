using CipherSharp.Ciphers.Classical;
using CipherSharp.Enums;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class PlayfairTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "abc";
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = Playfair.Encode(text, key, mode, displaySquare);

            // Assert
            Assert.Equal("KCNVMPYMQMCY", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "KCNVMPYMQMCY";
            string key = "abc";
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = Playfair.Decode(text, key, mode, displaySquare);

            // Assert
            Assert.Equal("HELXLOWORLDX", result);
        }
    }
}
