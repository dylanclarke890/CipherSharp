using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Ciphers.Tests.Transposition
{
    public class RouteTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            int key = 4;

            // Act
            var result = Route.Encode(text, key);

            // Assert
            Assert.Equal("holdweloXXrl", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "holdweloXXrl";
            int key = 4;

            // Act
            var result = Route.Decode(text, key);

            // Assert
            Assert.Equal("helloworldXX", result);
        }

        [Fact]
        public void Encode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 4;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Route.Encode(text, key));
        }

        [Fact]
        public void Decode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 4;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Route.Decode(text, key));
        }
    }
}
