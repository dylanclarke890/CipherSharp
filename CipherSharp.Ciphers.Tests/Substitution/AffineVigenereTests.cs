using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class AffineVigenereTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };
            AffineVigenere affine = new(text, keys);
            // Act
            var result = affine.Encode();

            // Assert
            Assert.Equal("CYLIZXRRCK", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "CYLIZXRRCK";
            string[] keys = new string[2] { "test", "key" };
            AffineVigenere affine = new(text, keys);
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
            string[] keys = { "test" };
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new AffineVigenere(text, keys));
        }

        [Fact]
        public void NewInstance_NullKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "test";
            string[] keys = null;
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new AffineVigenere(text, keys));
        }
    }
}
