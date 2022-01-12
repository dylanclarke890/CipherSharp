using CipherSharp.Ciphers;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class PolybiusTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string initialKey = "test";
            string sep = "";
            string mode = "IJ";

            // Act
            var result = Polybius.Encode(text, initialKey, sep, mode);

            // Assert
            Assert.Equal("25123333415241443322", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "25123333415241443322";
            string initialKey = "test";
            string sep = "";
            string mode = "IJ";

            // Act
            var result = Polybius.Decode(text, initialKey, sep, mode);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
