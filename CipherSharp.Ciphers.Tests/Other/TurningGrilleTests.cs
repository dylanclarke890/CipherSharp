using CipherSharp.Ciphers.Other;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class TurningGrilleTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
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
            TurningGrille turningGrille = new(text, key, n);

            // Act
            var result = turningGrille.Encode();

            // Assert
            // Randomised letters each time so no way to verify the entire string, just length
            Assert.Equal(144, result.Length);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
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
            TurningGrille turningGrille = new(text, key, n);

            // Act
            var result = turningGrille.Encode();

            // Assert
            Assert.Equal(144, result.Length);
        }

        [Fact]
        public void NewInstance_NullKey_ThrowsArgumentNullException()
        {
            // Arrange
            string message = "helloworld";
            int[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new TurningGrille(message, keys));
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string message = null;
            int[] keys = new int[2] { 1, 2 };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new TurningGrille(message, keys));
        }
    }
}
