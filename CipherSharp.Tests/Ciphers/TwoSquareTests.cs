using CipherSharp.Ciphers;
using CipherSharp.Enums;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class TwoSquareTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key"};
            AlphabetMode mode = AlphabetMode.IJ;
            bool printKey = false;

            // Act
            var result = TwoSquare.Encode(text, keys, mode, printKey);

            // Assert
            Assert.Equal("HEQQOWVWLD", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HEQQOWVWLD";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.IJ;
            bool printKey = false;

            // Act
            var result = TwoSquare.Decode(text, keys, mode, printKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
