using CipherSharp.Ciphers;
using CipherSharp.Enums;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class FourSquareTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = { "abc", "abc" };
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Encode(text, keys, mode, displaySquare);

            // Assert
            Assert.Equal("KCLLMYMTOA", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "KCLLMYMTOA";
            string[] keys = { "abc", "abc"};
            AlphabetMode mode = AlphabetMode.JI;
            bool displaySquare = false;

            // Act
            var result = FourSquare.Decode(text, keys, mode, displaySquare);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
