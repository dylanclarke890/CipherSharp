using CipherSharp.Ciphers;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class DoubleColumnarTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;

            // Act
            var result = DoubleColumnar.Encode(text, initialKey, complete);

            // Assert
            Assert.Equal("hdrwleXloolX", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "hdrwleXloolX";
            string[] initialKey = new string[2] { "ABC", "ABC" };
            bool complete = true;

            // Act
            var result = DoubleColumnar.Decode(text, initialKey, complete);

            // Assert
            Assert.Equal("helloworldXX", result);
        }
    }
}
