using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class HuttonTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            Hutton hutton = new(text, keys);
            // Act
            var result = hutton.Encode();

            // Assert
            Assert.Equal("NQPUTMXMKR", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "NQPUTMXMKR";
            string[] keys = new string[2] { "test", "key" };
            Hutton hutton = new(text, keys);
            // Act
            var result = hutton.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null;
            string[] keys = { "test" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Hutton(message, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "test";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Hutton(message, keys));
        }
    }
}
