using CipherSharp.Ciphers.Polyalphabetic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class QuagmireFourTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[3] { "test", "key", "hello" };
            // Act
            var result = Quagmire.Four.Encode(text, keys);

            // Assert
            Assert.Equal("RYZZCCQCZU", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "RYZZCCQCZU";
            string[] keys = new string[3] { "test", "key", "hello" };
            // Act
            var result = Quagmire.Four.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
