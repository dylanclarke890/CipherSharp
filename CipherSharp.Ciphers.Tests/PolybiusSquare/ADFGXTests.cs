using CipherSharp.Ciphers.PolybiusSquare;
using System;
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

        [Theory]
        [InlineData("helloworld", "test", null)]
        [InlineData(null, "test", new int[2] { 1, 2 })]
        [InlineData("helloworld", null, new int[2] { 1, 2 })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key, int[] keys)
        {
            // Arrange
            bool displaySquare = false;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => ADFGX.Encode(text, key, keys, displaySquare));
        }

        [Theory]
        [InlineData("helloworld", "test", null)]
        [InlineData(null, "test", new int[2] { 1, 2 })]
        [InlineData("helloworld", null, new int[2] { 1, 2 })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key, int[] keys)
        {
            // Arrange
            bool displaySquare = false;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => ADFGX.Decode(text, key, keys, displaySquare));
        }
    }
}
