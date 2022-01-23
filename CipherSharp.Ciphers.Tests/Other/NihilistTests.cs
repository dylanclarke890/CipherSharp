using CipherSharp.Ciphers.Other;
using CipherSharp.Utility.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class NihilistTests
    {
        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, new string[2] { "test", "key" })]
        public void CreatingInstanceWithNullParameters_ThrowsArgumentNullException(string text, string[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Nihilist(text, keys));
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
