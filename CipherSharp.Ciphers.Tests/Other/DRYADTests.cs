using CipherSharp.Ciphers.Other;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class DRYADTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "213165587194201";
            int key = 300633440;

            // Act
            var result = DRYAD.Encode(text, key);

            // Assert
            Assert.Equal(20, result.Length);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "213165587194201";
            int key = 300633440;

            // Act
            var result = DRYAD.Decode(DRYAD.Encode(text, key), key);

            // Assert
            Assert.Equal("213165587194201", result);
        }

        [Fact]
        public void Encode_NullParameters_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DRYAD.Encode(text, key));
        }

        [Fact]
        public void Decode_NullParameters_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DRYAD.Decode(text, key));
        }

        [Fact]
        public void Encode_TextContainsLetters_ThrowsArgumentException()
        {
            // Arrange
            string text = "213165587194201as";
            int key = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => DRYAD.Encode(text, key));
        }
    }
}
