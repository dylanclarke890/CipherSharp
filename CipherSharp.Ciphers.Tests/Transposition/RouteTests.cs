using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class RouteTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            int key = 4;
            Route route = new(text, key);
            // Act
            var result = route.Encode();

            // Assert
            Assert.Equal("HOLDWELOXXRL", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HOLDWELOXXRL";
            int key = 4;
            Route route = new(text, key);
            // Act
            var result = route.Decode();

            // Assert
            Assert.Equal("HELLOWORLDXX", result);
        }

        [Fact]
        public void NewInstance_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int key = 4;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Route(text, key));
        }
    }
}
