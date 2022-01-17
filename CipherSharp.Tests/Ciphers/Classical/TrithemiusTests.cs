using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class TrithemiusTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = Trithemius.Encode(text);

            // Assert
            Assert.Equal("HFNOSBUYTM", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HFNOSBUYTM";

            // Act
            var result = Trithemius.Decode(text);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
