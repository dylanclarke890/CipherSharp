using CipherSharp.Ciphers.Transposition;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class ColumnarTests
    {
        [Fact]
        public void Encode_IntArray_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            int[] initialKey = new int[2] { 1, 2 };
            bool complete = true;

            // Act
            var result = Columnar.Encode(text, initialKey, complete);

            // Assert
            Assert.Equal("hloolelwrd", result);
        }

        [Fact]
        public void Decode_IntArray_ReturnsPlainText()
        {
            // Arrange
            string text = "hloolelwrd";
            int[] initialKey = new int[2] { 1, 2 };
            bool complete = true;

            // Act
            var result = Columnar.Decode(text, initialKey, complete);

            // Assert
            Assert.Equal("helloworld", result);
        }

        [Fact]
        public void Encode_CharArray_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] initialKey = new string[2] { "ABC", "DEF" };
            bool complete = true;

            // Act
            var result = Columnar.Encode(text, initialKey, complete);

            // Assert
            Assert.Equal("hloolelwrd", result);
        }

        [Fact]
        public void Decode_CharArray_ReturnsPlainText()
        {
            // Arrange
            string text = "hloolelwrd";
            string[] initialKey = new string[2] { "ABC", "DEF" };
            bool complete = true;

            // Act
            var result = Columnar.Decode(text, initialKey, complete);

            // Assert
            Assert.Equal("helloworld", result);
        }
    }
}
