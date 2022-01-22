using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class HuttonTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = Hutton.Encode(text, keys);

            // Assert
            Assert.Equal("NQPUTMXMKR", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "NQPUTMXMKR";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = Hutton.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Hutton.Encode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Hutton.Decode(text, keys));
        }
    }
}
