using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class RailFenceTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            int key = 3;
            RailFence railFence = new(text, key);
            // Act
            var result = railFence.Encode();

            // Assert
            Assert.Equal("HOLELWRDLO", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HOLELWRDLO";
            int key = 3;
            RailFence railFence = new(text, key);
            // Act
            var result = railFence.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RailFence(text, key));
        }
    }
}
