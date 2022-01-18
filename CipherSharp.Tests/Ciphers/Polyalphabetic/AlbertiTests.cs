using CipherSharp.Ciphers.Polyalphabetic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class AlbertiTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string key = "TEST";
            char startingLetter = 'H';
            int[] range = new int[2] { 1, 2 };
            int turn = 0;

            // Act
            var result = Alberti.Encode(text, key, startingLetter, range, turn);

            // Assert
            Assert.Equal("OLUUX5X0UK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "OLUUX5X0UK";
            string key = "TEST";
            char startingLetter = 'H';
            int turn = 0;

            // Act
            var result = Alberti.Decode(text, key, startingLetter, turn);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
