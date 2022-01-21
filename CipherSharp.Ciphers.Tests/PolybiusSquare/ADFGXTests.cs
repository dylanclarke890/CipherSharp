using CipherSharp.Ciphers.PolybiusSquare;
using Xunit;

namespace CipherSharp.Tests.Ciphers.PolybiusSquare
{
    public class ADFGXTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string polybiusKey = "test";
            int[] columnarKey = new int[2] { 1, 2 };
            bool displaySquare = false;

            // Act
            var result = ADFGX.Encode(text, polybiusKey, columnarKey, displaySquare);

            // Assert
            Assert.Equal("CLURKWLEAK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CLURKWLEAK";
            string polybiusKey = "test";
            int[] columnarKey = new int[2] { 1, 2 };
            bool displaySquare = false;

            // Act
            var result = ADFGX.Decode(text, polybiusKey, columnarKey, displaySquare);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
