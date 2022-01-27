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
            int range = 1;
            int turn = 0;
            Alberti alberti = new(text, key, startingLetter, turn, range);

            // Act
            var result = alberti.Encode();

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

        [Fact]
        public void Encode_LargerParameters_ReturnsCipherText()
        {
            // Arrange
            string message = "Some random text to test that is lowercase and length so that I can properly measure the performance I think this should be enough";
            string textKey = "Alongtestkeythatcanbeusedtoforthecipher";
            char albertiStartingChar = 'M';
            int turn = 9;
            Alberti alberti = new(message, textKey, albertiStartingChar, turn);
            // Act
            var result = alberti.Encode();

            // Assert
            var expected = "OSDFL3FD8TQFSBQZN7M4NAO42CUZE7JUMCEC76UF79QPOSQPMBHUMEI28KE25IDFMCV2XBYF97JR8HDC75EJNAHX4BYJOCYZGGTBX7FZG9Y";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Decode_LargerParameters_ReturnsPlainText()
        {
            // Arrange
            string message = "OSDFL3FD8TQFSBQZN7M4NAO42CUZE7JUMCEC76UF79QPOSQPMBHUMEI28KE25IDFMCV2XBYF97JR8HDC75EJNAHX4BYJOCYZGGTBX7FZG9Y";
            string textKey = "Alongtestkeythatcanbeusedtoforthecipher";
            char albertiStartingChar = 'M';
            int turn = 9;
            Alberti alberti = new(message, textKey, albertiStartingChar, turn);
            // Act
            var result = alberti.Decode();
            // Assert
            var expected = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
            Assert.Equal(expected, result);
        }
    }
}
