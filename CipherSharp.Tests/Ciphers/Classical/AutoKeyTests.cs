using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class AutoKeyTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = AutoKey.Encode(text, key);

            // Assert
            Assert.Equal("AIDEVAZCZZ", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "AIDEVAZCZZ";
            string key = "test";

            // Act
            var result = AutoKey.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
