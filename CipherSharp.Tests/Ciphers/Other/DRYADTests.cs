using CipherSharp.Ciphers.Other;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Other
{
    public class DRYADTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "213165587194201";
            int key = 300633440;

            // Act
            var result = DRYAD.Encode(text, key);

            // Assert
            Assert.Equal(20, result.Length);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "213165587194201";
            int key = 300633440;

            // Act
            var result = DRYAD.Decode(DRYAD.Encode(text, key), key);

            // Assert
            Assert.Equal("213165587194201", result);
        }
    }
}
