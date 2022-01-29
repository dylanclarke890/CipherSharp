using CipherSharp.Ciphers.Transposition;
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
            string text = "HELLOWORLD";
            string[] key = new string[4] { "test", "key", "test", "key" };
            Disrupted<string> disrupted = new(text, key);
            // Act
            var result = disrupted.Encode();

            // Assert
            Assert.Equal("ELLWHLOROD", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "ELLWHLOROD";
            string[] key = new string[4] { "test", "key", "test", "key" };
            Disrupted<string> disrupted = new(text, key);
            // Act
            var result = disrupted.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string[] keys = { "test"};
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Disrupted<string>(text, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "test";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Disrupted<string>(text, keys));
        }
    }
}
