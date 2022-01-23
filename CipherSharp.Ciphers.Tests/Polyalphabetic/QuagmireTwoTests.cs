using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class QuagmireTwoTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[3] { "test", "key", "hello" };
            QuagmireTwo quagmire = new(text, keys);
            // Act
            var result = quagmire.Encode();

            // Assert
            Assert.Equal("RCHXNUTQHN", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "RCHXNUTQHN";
            string[] keys = new string[3] { "test", "key", "hello" };
            QuagmireTwo quagmire = new(text, keys);
            // Act
            var result = quagmire.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            string[] keys = new string[3] { "test", "key", "hello" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new QuagmireTwo(text, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "RYZZCCQCZU";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new QuagmireTwo(text, keys));
        }
    }
}
