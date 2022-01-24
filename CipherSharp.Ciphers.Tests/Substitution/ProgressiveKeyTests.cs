using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class ProgressiveKeyTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            int numKey = 2;
            string textKey = "test";
            ProgressiveKey progressiveKey = new(text, numKey, textKey);
            // Act
            var result = progressiveKey.Encode();

            // Assert
            Assert.Equal("AIDEJCIMIA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "AIDEJCIMIA";
            int numKey = 2;
            string textKey = "test";
            ProgressiveKey progressiveKey = new(text, numKey, textKey);
            // Act
            var result = progressiveKey.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string textKey)
        {
            // Arrange
            int numKey = 2;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new ProgressiveKey(text, numKey, textKey));
        }
    }
}
