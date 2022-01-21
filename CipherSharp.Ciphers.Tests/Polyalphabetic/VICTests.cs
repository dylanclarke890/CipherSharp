using CipherSharp.Ciphers.Polyalphabetic;
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
    }
}
