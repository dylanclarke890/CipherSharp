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
            DRYAD dryad = new(text, key);

            // Act
            var result = dryad.Encode();

            // Assert
            Assert.Equal(20, result.Length); // randomized each time
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CZVKVN MUNCKM IKYZAO";
            int key = 300633440;
            DRYAD dryad = new(text, key);

            // Act
            var result = dryad.Decode();
            
            // Assert
            Assert.Equal("213165587194201", result);
        }

        [Fact]
        public void Encode_NullParameters_ThrowsInvalidOperationException()
        {
            // Arrange
            string text = null;
            int key = 0;
            DRYAD dryad = new(text, key);
            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => dryad.Encode());
        }

        [Fact]
        public void Decode_NullParameters_ThrowsInvalidOperationException()
        {
            // Arrange
            string text = null;
            int key = 0;
            DRYAD dryad = new(text, key);

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => dryad.Decode());
        }

        [Fact]
        public void Encode_TextContainsLetters_ThrowsInvalidOperationException()
        {
            // Arrange
            string text = "213165587194201as";
            int key = 0;
            DRYAD dryad = new(text, key);
            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() => dryad.Encode());
        }
    }
}
