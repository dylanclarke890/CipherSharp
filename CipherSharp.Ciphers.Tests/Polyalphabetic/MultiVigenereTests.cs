using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class MultiVigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiVigenere.Encode(text, keys);

            // Assert
            Assert.Equal("GDKKNVNQKCGDKKNVNQKC", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "GDKKNVNQKCGDKKNVNQKC";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiVigenere.Decode(text, keys);

            // Assert
            Assert.Contains("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, new string[2] { "test", "key" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => MultiVigenere.Decode(text, keys));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, new string[2] { "test", "key" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => MultiVigenere.Decode(text, keys));
        }
    }
}
