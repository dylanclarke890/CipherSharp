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

            // Act
            var result = VIC.Encode(text, keys, phrase, transKey);

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

            // Act
            var result = VIC.Decode(text, keys, phrase, transKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData(null, new string[1] { "test" }, "ABCDEFGHIJKLMNOPQRST")]
        [InlineData("3873311043344384", null, "ABCDEFGHIJKLMNOPQRST")]
        [InlineData("3873311043344384", new string[1] { "test" }, null)]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string[] keys, string phrase)
        {
            // Arrange
            int transKey = 5;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => VIC.Encode(text, keys, phrase, transKey));
        }

        [Theory]
        [InlineData(null, new string[1] { "test" }, "ABCDEFGHIJKLMNOPQRST")]
        [InlineData("3873311043344384", null, "ABCDEFGHIJKLMNOPQRST")]
        [InlineData("3873311043344384", new string[1] { "test" }, null)]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string[] keys, string phrase)
        {
            // Arrange
            int transKey = 5;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => VIC.Decode(text, keys, phrase, transKey));
        }
    }
}
