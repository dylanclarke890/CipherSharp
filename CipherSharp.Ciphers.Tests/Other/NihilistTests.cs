using CipherSharp.Ciphers.Other;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class NihilistTests
    {
        [Fact]
        public void NewInstance_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "helloworld";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Nihilist(message, keys));
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null;
            string[] keys = new string[2] { "test", "key"};
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Nihilist(message, keys));
        }

        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.EX;
            Nihilist nihilist = new(text, keys, mode);

            // Act
            var result = nihilist.Encode();

            // Assert
            Assert.Equal("55 24 83 63 47 96 66 54 83 52", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "55 24 83 63 47 96 66 54 83 52";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.EX;
            Nihilist nihilist = new(text, keys, mode);

            // Act
            var result = nihilist.Decode();

            // Assert
            Assert.Equal("H E L L O W O R L D", result);
        }
    }
}
