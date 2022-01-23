using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class VICTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "123456", "123456" };
            string phrase = "ABCDEFGHIJKLMNOPQRST";
            int transKey = 5;
            VIC vic = new(text, keys, phrase, transKey);
            // Act
            var result = vic.Encode();

            // Assert
            Assert.Equal("3873311043344384", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "3873311043344384";
            string[] keys = new string[2] { "123456", "123456" };
            string phrase = "ABCDEFGHIJKLMNOPQRST";
            int transKey = 5;
            VIC vic = new(text, keys, phrase, transKey);
            // Act
            var result = vic.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData(null, "ABCDEFGHIJKLMNOPQRST")]
        [InlineData("3873311043344384", null)]
        public void NewInstance_NullStrings_ThrowsArgumentException(string text, string phrase)
        {
            // Arrange
            int transKey = 5;
            string[] keys = { "test", "key" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new VIC(text, keys, phrase, transKey));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "3873311043344384";
            string[] keys = null;
            string phrase = "ABCDEFGHIJKLMNOPQRST";
            int transKey = 5;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new VIC(text, keys, phrase, transKey));
        }
    }
}
