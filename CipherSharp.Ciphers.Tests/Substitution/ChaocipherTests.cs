using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class ChaocipherTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "HXUCZVAMDSLKPEFJRIGTWOBNYQ", "PTLNBQDEOYSFAVZKGJRIHWXUMC" };

            // Act
            var result = Chaocipher.Encode(text, keys);

            // Assert
            Assert.Equal("WAHQZIUETS", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "WAHQZIUETS";
            string[] keys = new string[2] { "HXUCZVAMDSLKPEFJRIGTWOBNYQ", "PTLNBQDEOYSFAVZKGJRIHWXUMC" };

            // Act
            var result = Chaocipher.Decode(text, keys);

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
            Assert.Throws<ArgumentException>(() => Chaocipher.Encode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Chaocipher.Decode(text, keys));
        }
    }
}
