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
            Alberti alberti = new(text, key, startingLetter, turn);

            // Act
            var result = alberti.Encode(range);

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
            Alberti alberti = new(text, key, startingLetter, turn);
            // Act
            var result = alberti.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Encode_NullRange_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "OLUUX5X0UK";
            string key = "TEST";
            char startingLetter = 'H';
            int turn = 0;
            Alberti alberti = new(text, key, startingLetter, turn);
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => alberti.Encode(null));
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            char letter = 'A';
            int turn = 0;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Alberti(text, key, letter, turn));
        }

        [Theory]
        [InlineData('4')]
        [InlineData('!')]
        [InlineData('\0')]
        public void NewInstance_NonLetterStartingPos_ThrowsArgumentException(char letter)
        {
            // Arrange
            string text = "OLUUX5X0UK";
            string key = "TEST";
            int turn = 0;
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Alberti(text, key, letter, turn));
        }
    }
}
