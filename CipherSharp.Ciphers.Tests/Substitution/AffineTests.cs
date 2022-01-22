using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class AffineTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            int[] key = new int[2] { 3, 5 };

            // Act
            var result = Affine.Encode(text, key);

            // Assert
            Assert.Equal("ARMMVTVEMO", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "ARMMVTVEMO";
            int[] key = new int[2] { 3, 5 };

            // Act
            var result = Affine.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new int[2] { 1, 2 })]
        public void Encode_NullParameters_ThrowsArgumentException(string text, int[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Affine.Decode(text, keys));
        }

        [Theory]
        [InlineData("KCLLMYMTOA", null)]
        [InlineData(null, new int[2] { 1, 2 })]
        public void Decode_NullParameters_ThrowsArgumentException(string text, int[] keys)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Affine.Decode(text, keys));
        }
    }
}
