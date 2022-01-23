using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class MultiVigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string message = "HELLOWORLD";
            string[] keys = new string[2] { "test", "key" };
            MultiVigenere multiVigenere = new (message, keys);
            // Act
            var result = multiVigenere.Encode();

            // Assert
            Assert.Equal("GDKKNVNQKCGDKKNVNQKC", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string message = "GDKKNVNQKCGDKKNVNQKC";
            string[] keys = new string[2] { "test", "key" };
            MultiVigenere multiVigenere = new(message, keys);
            // Act
            var result = multiVigenere.Decode();

            // Assert
            Assert.Contains("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null;
            string[] keys = new string[2] { "test", "key" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new MultiVigenere(message, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "helloworld";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new MultiVigenere(message, keys));
        }
    }
}
