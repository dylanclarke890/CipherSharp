using CipherSharp.Ciphers.Classical;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class RecursiveKeyTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string key = "test";

            // Act
            var result = RecursiveKey.Encode(text, key);

            // Assert
            Assert.Equal("AXEEJRJMIA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "AXEEJRJMIA";
            string key = "test";

            // Act
            var result = RecursiveKey.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
