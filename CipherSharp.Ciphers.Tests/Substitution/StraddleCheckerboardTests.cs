using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class StraddleCheckerboardTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string initialKey = "test";
            int[] keys = new int[2] { 0, 1};
            StraddleCheckerboard straddleCheckerboard = new(text, initialKey, keys);
            // Act
            var result = straddleCheckerboard.Encode();

            // Assert
            Assert.Equal("013050508140811058", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "013050508140811058";
            string initialKey = "test";
            int[] keys = new int[2] { 0, 1 };
            StraddleCheckerboard straddleCheckerboard = new(text, initialKey, keys);
            // Act
            var result = straddleCheckerboard.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, "test")]
        public void Encode_NullStrings_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            int[] keys = { 1 };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new StraddleCheckerboard(text, key, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "test";
            string key = "test";
            int[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new StraddleCheckerboard(text, key, keys));
        }
    }
}
