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
            ADFGX aDFGX = new(text, polybiusKey, columnarKey);
            // Act
            var result = aDFGX.Encode();

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
            ADFGX aDFGX = new(text, polybiusKey, columnarKey);
            // Act
            var result = aDFGX.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullStrings_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            int[] keys = { 1, 2 };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new ADFGX(text, key, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "CLURKWLEAK";
            string polybiusKey = "test";
            int[] columnarKey = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new ADFGX(text, polybiusKey, columnarKey));
        }
    }
}
