using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class SimpleSubstitutionTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "helloworld";
            string key = "test";

            // Act
            var result = SimpleSubstitution.Encode(text, key);

            // Assert
            Assert.Equal("FBJJMWMPJA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "FBJJMWMPJA";
            string key = "test";

            // Act
            var result = SimpleSubstitution.Decode(text, key);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void Encode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => SimpleSubstitution.Encode(text, key));
        }

        [Theory]
        [InlineData("IMOTMNNLLK", null)]
        [InlineData(null, "test")]
        public void Decode_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => SimpleSubstitution.Decode(text, key));
        }
    }
}
