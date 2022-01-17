using CipherSharp.Ciphers.Classical;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
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

            // Act
            var result = ProgressiveKey.Encode(text, numKey, textKey);

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

            // Act
            var result = ProgressiveKey.Decode(text, numKey, textKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
