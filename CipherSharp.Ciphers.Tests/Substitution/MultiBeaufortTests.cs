using CipherSharp.Ciphers.Substitution;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Substitution
{
    public class MultiBeaufortTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiBeaufort.Encode(text, keys);

            // Assert
            Assert.Equal("YERCZQGIWI", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "YERCZQGIWI";
            string[] keys = new string[2] { "test", "key" };

            // Act
            var result = MultiBeaufort.Decode(text, keys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
