using CipherSharp.Ciphers;
using CipherSharp.Enums;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class FourSquareTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = { "abc", "abc" };
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Encode(text, keys, mode, displaySquare);

            // Assert
            Assert.Equal("KCLLMYMTOA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "KCLLMYMTOA";
            string[] keys = { "abc", "abc"};
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Decode(text, keys, mode, displaySquare);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
