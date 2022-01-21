using CipherSharp.Ciphers.Substitution;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class ROT13Tests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = ROT13.Encode(text);

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "helloworld";

            // Act
            var result = ROT13.Decode(text);

            // Assert
            Assert.Equal("URYYBJBEYQ", result);
        }
    }
}
