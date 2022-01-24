using CipherSharp.Ciphers.Transposition;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class ColumnarTests
    {
        [Fact]
        public void Encode_IntArray_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            int[] key = new int[2] { 1, 2 };
            bool complete = true;
            Columnar<int> columnar = new(text, key);
            // Act
            var result = columnar.Encode(complete);

            // Assert
            Assert.Equal("HLOOLELWRD", result);
        }

        [Fact]
        public void Decode_IntArray_ReturnsPlainText()
        {
            // Arrange
            string text = "HLOOLELWRD";
            int[] key = new int[2] { 1, 2 };
            bool complete = true;
            Columnar<int> columnar = new(text, key);
            // Act
            var result = columnar.Decode(complete);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Encode_StringArray_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string[] key = new string[2] { "ABC", "DEF" };
            bool complete = true;
            Columnar<string> columnar = new(text, key);
            // Act
            var result = columnar.Encode(complete);

            // Assert
            Assert.Equal("HLOOLELWRD", result);
        }

        [Fact]
        public void Decode_StringArray_ReturnsPlainText()
        {
            // Arrange
            string text = "HLOOLELWRD";
            string[] key = new string[2] { "ABC", "DEF" };
            bool complete = true;
            Columnar<string> columnar = new(text, key);
            // Act
            var result = columnar.Decode(complete);

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
            Assert.Throws<ArgumentException>(() => new Columnar<string>(message, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "test";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Columnar<string>(message, keys));
        }
    }
}
