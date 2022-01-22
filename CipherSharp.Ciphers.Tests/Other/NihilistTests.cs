using CipherSharp.Ciphers.Other;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class NihilistTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            AlphabetMode mode = AlphabetMode.EX;

            // Act
            var result = Nihilist.Encode(text, keys, mode);

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

            // Act
            var result = Nihilist.Decode(text, keys, mode);

            // Assert
            Assert.Equal("H E L L O W O R L D", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, new string[2] { "test", "key" })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Nihilist.Encode(text, keys));
        }

        [Theory]
        [InlineData("55 24 83 63 47 96 66 54 83 52", null)]
        [InlineData(null, new string[2] { "test", "key" })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Nihilist.Decode(text, keys));
        }
    }
}
