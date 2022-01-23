using CipherSharp.Ciphers.Polyalphabetic;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class TrithemiusTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            Trithemius trithemius = new(text);
            // Act
            var result = trithemius.Encode();

            // Assert
            Assert.Equal("HFNOSBUYTM", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HFNOSBUYTM";
            Trithemius trithemius = new(text);
            // Act
            var result = trithemius.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullText_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new Trithemius(text));
        }
    }
}
