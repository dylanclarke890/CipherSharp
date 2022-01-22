using CipherSharp.Ciphers.PolybiusSquare;
using System;
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

        [Fact]
        public void Encode_NullParameters_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string matrixKey = null;
            int[] columnarKeys = null;
            bool printKey = false;

            // Act - done in assert

            // Assert
            Assert.Throws<ArgumentException>(() => ADFGVX.Encode(text, matrixKey, columnarKeys, printKey));
        }

        [Fact]
        public void Decode_NullParameters_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string matrixKey = null;
            int[] columnarKeys = null;
            bool printKey = false;

            // Act - done in assert

            // Assert
            Assert.Throws<ArgumentException>(() => ADFGVX.Decode(text, matrixKey, columnarKeys, printKey));
        }
    }
}
