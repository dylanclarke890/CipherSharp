using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class DoubleColumnarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;

            // Act
            var result = DoubleColumnar.Encode(text, initialKey, complete);

            // Assert
            Assert.Equal("hdrwleXloolX", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "hdrwleXloolX";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;

            // Act
            var result = DoubleColumnar.Decode(text, initialKey, complete);

            // Assert
            Assert.Equal("helloworldXX", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DoubleColumnar.Encode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DoubleColumnar.Decode(text, keys));
        }
    }
}
