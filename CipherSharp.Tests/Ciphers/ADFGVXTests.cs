using CipherSharp.Ciphers;
using System;
using Xunit;

namespace CipherSharp.Tests.Ciphers
{
    public class ADFGVXTests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "helloworld";
            int[] keys = new int[2] { 0, 1 };
            bool printKey = false;

            // Act
            var result = ADFGVX.Encode(text, keys, printKey);

            // Assert
            Assert.Equal("DFAXFAFAFGGXFGGAFAAV", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "DFAXFAFAFGGXFGGAFAAV";
            int[] keys = new int[2] { 0, 1 };
            bool printKey = false;

            // Act
            var result = ADFGVX.Decode(text, keys, printKey);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
