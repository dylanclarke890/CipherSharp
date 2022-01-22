using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class MultiBeaufortTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiBeaufort.Encode(text, keys);

            // Assert
            Assert.Equal("YERCZQGIWI", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "YERCZQGIWI";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiBeaufort.Decode(text, keys);

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
            Assert.Throws<ArgumentException>(() => MultiBeaufort.Encode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => MultiBeaufort.Decode(text, keys));
        }
    }
}
