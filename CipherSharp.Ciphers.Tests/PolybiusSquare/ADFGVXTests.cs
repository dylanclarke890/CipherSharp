using CipherSharp.Ciphers.PolybiusSquare;
using Xunit;

namespace CipherSharp.Tests.Ciphers.PolybiusSquare
{
    public class ADFGVXTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string matrixKey = "test";
            int[] columnarKeys = new int[2] { 0, 2 };
            bool printKey = false;

            // Act
            var result = ADFGVX.Encode(text, matrixKey, columnarKeys, printKey);

            // Assert
            Assert.Equal("DMNNLRF2ZD", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "DMNNLRF2ZD";
            string matrixKey = "test";
            int[] columnarKeys = new int[2] { 0, 2 };
            bool printKey = false;

            // Act
            var result = ADFGVX.Decode(text, matrixKey, columnarKeys, printKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
