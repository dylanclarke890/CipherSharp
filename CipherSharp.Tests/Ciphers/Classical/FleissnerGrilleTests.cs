using CipherSharp.Ciphers.Classical;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Classical
{
    public class FleissnerGrilleTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            int[] key = new int[36] 
            { 
                0, 1, 2, 3, 4, 5, 6, 7,
                8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29, 30, 31,
                32, 33, 34, 35
            };

            int n = 6;

            // Act
            var result = FleissnerGrille.Encode(text, key, n);

            // Assert
            Assert.Equal("hrlloworld", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            int[] key = new int[36]
            {
                0, 1, 2, 3, 4, 5, 6, 7,
                8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29, 30, 31,
                32, 33, 34, 35
            };
            int n = 6;

            // Act
            var result = FleissnerGrille.Decode(text, key, n);

            // Assert
            Assert.Equal("hrlloworld", result);
        }
    }
}
