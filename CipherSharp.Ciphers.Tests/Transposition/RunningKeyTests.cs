using CipherSharp.Ciphers.Substitution;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Transposition
{
    public class RunningKeyTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string key = "test";
            RunningKey running = new(text, key);
            // Act
            var result = running.Encode();

            // Assert
            Assert.Equal("AXEEJRJMIA", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "AXEEJRJMIA";
            string key = "test";
            RunningKey running = new(text, key);
            // Act
            var result = running.Decode();

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
            Assert.Throws<ArgumentException>(() => new RunningKey(text, key));
        }
    }
}
