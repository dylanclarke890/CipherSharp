using CipherSharp.Ciphers.Classical;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
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
    }
}
