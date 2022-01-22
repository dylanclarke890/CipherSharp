﻿using CipherSharp.Ciphers.Substitution;
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

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, "test")]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string textKey)
        {
            // Arrange
            int numKey = 2;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => ProgressiveKey.Encode(text, numKey, textKey));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string textKey)
        {
            // Arrange
            int numKey = 2;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => ProgressiveKey.Decode(text, numKey, textKey));
        }
    }
}
