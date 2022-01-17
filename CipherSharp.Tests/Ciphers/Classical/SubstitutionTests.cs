using CipherSharp.Ciphers.Classical;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class SubstitutionTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = Substitution.Encode(text, key);

            // Assert
            Assert.Equal("FBJJMWMPJA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "FBJJMWMPJA";
            string key = "test";
            
            // Act
            var result = Substitution.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
