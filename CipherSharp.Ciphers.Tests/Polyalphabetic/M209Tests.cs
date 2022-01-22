using CipherSharp.Ciphers.Polyalphabetic;
using System;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
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

        [Theory]
        [InlineData("SVOOLDLIOW", null)]
        [InlineData(null, "ABCDEF")]
        public void Encode_NullStrings_ThrowsArgumentException(string text, string wheelKey)
        {
            // Arrange
            List<string> pins = new();
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Encode(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void Encode_NullPins_ThrowsArgumentException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = null;
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Encode(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void Encode_NullLugs_ThrowsArgumentException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = null;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Encode(text, wheelKey, pins, lugs));
        }

        [Theory]
        [InlineData("SVOOLDLIOW", null)]
        [InlineData(null, "ABCDEF")]
        public void Decode_NullStrings_ThrowsArgumentException(string text, string wheelKey)
        {
            // Arrange
            List<string> pins = new();
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Decode(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void Decode_NullPins_ThrowsArgumentException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = null;
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Decode(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void Decode_NullLugs_ThrowsArgumentException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = null;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => M209.Decode(text, wheelKey, pins, lugs));
        }
    }
}
