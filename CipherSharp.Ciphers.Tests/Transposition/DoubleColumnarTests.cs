using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class DoubleColumnarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;
            DoubleColumnar columnar = new(text, initialKey);
            // Act
            var result = columnar.Encode(complete);

            // Assert
            Assert.Equal("HDRWLEXLOOLX", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HDRWLEXLOOLX";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;
            DoubleColumnar columnar = new(text, initialKey);
            // Act
            var result = columnar.Decode(complete);

            // Assert
            Assert.Equal("HELLOWORLDXX", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null;
            string[] keys = { "test" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new DoubleColumnar(message, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "test";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new DoubleColumnar(message, keys));
        }
    }
}
