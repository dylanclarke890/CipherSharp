using CipherSharp.Ciphers.Other;
using CipherSharp.Utility.Enums;
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
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
