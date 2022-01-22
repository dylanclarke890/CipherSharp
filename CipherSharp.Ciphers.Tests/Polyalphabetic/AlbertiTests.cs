using CipherSharp.Ciphers.Polyalphabetic;
using System;
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

        [Theory]
        [InlineData("helloworld", null, 'A', new int[1] { 1 })]
        [InlineData(null, "test", 'A', new int[1] { 1 })]
        [InlineData("helloworld", "test", '\0', new int[1] { 1 })]
        [InlineData("helloworld", "test", 'A', null)]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key, char letter, int[] range)
        {
            // Arrange
            int turn = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Alberti.Encode(text, key, letter, range, turn));
        }

        [Theory]
        [InlineData("helloworld", null, 'A')]
        [InlineData(null, "test", 'A')]
        [InlineData("helloworld", "test", '\0')]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key, char letter)
        {
            // Arrange
            int turn = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Alberti.Decode(text, key, letter, turn));
        }
    }
}
