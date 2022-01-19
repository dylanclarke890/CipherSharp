using CipherSharp.Ciphers.Polyalphabetic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class QuagmireTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            // Act
            var result = Quagmire.One.Encode(text, keys);

            // Assert
            Assert.Equal("QCIURRXUIN", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "QCIURRXUIN";
            string[] keys = new string[2] { "test", "key" };
            // Act
            var result = Quagmire.One.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
