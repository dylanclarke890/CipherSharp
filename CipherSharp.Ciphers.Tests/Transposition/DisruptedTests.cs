﻿using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class DisruptedTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] key = new string[4] { "test", "key", "test", "key" };
            bool complete = false;

            // Act
            var result = Disrupted.Encode(text, key, complete);

            // Assert
            Assert.Equal("ellwhlorod", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "ellwhlorod";
            string[] key = new string[4] { "test", "key", "test", "key" };
            bool complete = false;

            // Act
            var result = Disrupted.Decode(text, key, complete);

            // Assert
            Assert.Equal("helloworld", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Disrupted.Encode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new string[2] { "abc", "abc" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Disrupted.Decode(text, keys));
        }
    }
}