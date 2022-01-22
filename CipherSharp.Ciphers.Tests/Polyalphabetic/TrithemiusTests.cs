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

            // Act
            var result = Trithemius.Encode(text);

            // Assert
            Assert.Equal("HFNOSBUYTM", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "HFNOSBUYTM";

            // Act
            var result = Trithemius.Decode(text);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Encode_NullText_Throws_ArgumentException()
        {
            // Arrange
            string text = null;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Trithemius.Encode(text));
        }

        [Fact]
        public void Decode_NullText_Throws_ArgumentException()
        {
            // Arrange
            string text = null;
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => Trithemius.Decode(text));
        }
    }
}
