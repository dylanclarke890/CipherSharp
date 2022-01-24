using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class AtbashTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            Atbash atbash = new(text);
            // Act
            var result = atbash.Encode();

            // Assert
            Assert.Equal("SVOOLDLIOW", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            Atbash atbash = new(text);
            // Act
            var result = atbash.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Encode_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Atbash(text));
        }
    }
}
