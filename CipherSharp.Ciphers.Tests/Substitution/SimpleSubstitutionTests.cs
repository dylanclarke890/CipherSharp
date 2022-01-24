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
            SimpleSubstitution simpleSubstitution = new(text, key);
            // Act
            var result = simpleSubstitution.Encode();

            // Assert
            Assert.Equal("FBJJMWMPJA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "FBJJMWMPJA";
            string key = "test";
            SimpleSubstitution simpleSubstitution = new(text, key);
            // Act
            var result = simpleSubstitution.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("helloworld", null)]
        [InlineData(null, "test")]
        public void NewInstance_NullParameters_ThrowsArgumentException(string text, string key)
        {
            // Arrange
            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new SimpleSubstitution(text, key));
        }
    }
}
