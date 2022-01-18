using CipherSharp.Ciphers.Mechanical;
using System;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Mechanical
{
    public class M209Tests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = new();

            // Act
            var result = M209.Encode(text, wheelKey, pins, lugs);

            // Assert
            Assert.Equal("SVOOLDLIOW", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = new();

            // Act
            var result = M209.Decode(text, wheelKey, pins, lugs);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
