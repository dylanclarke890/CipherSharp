using CipherSharp.Ciphers;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class ColumnarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
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
        public void Decode_BasicParameters_ReturnsPlainText()
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
    }
}
