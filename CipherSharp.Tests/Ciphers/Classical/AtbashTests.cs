using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class AtbashTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = Atbash.Encode(text);

            // Assert
            Assert.Equal("SVOOLDLIOW", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "SVOOLDLIOW";

            // Act
            var result = Atbash.Decode(text);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
