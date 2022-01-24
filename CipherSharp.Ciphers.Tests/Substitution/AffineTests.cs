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
            Affine affine = new(text, key);
            // Act
            var result = affine.Encode();

            // Assert
            Assert.Equal("ARMMVTVEMO", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "ARMMVTVEMO";
            int[] key = new int[2] { 3, 5 };
            Affine affine = new(text, key);
            // Act
            var result = affine.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            int[] keys = { 1, 2 };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Affine(text, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "test";
            int[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new Affine(text, keys));
        }
    }
}
