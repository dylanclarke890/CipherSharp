using CipherSharp.Ciphers.Mechanical;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Mechanical
{
    public class SIGABATests
    {
        [Fact]
        public void Encode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "HELLOWORLD";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";

            // Act
            var result = SIGABA.Encode(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos);

            // Assert
            Assert.Equal("PDVJEZXFCE", result);
        }

        [Fact]
        public void Decode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string text = "PDVJEZXFCE";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";

            // Act
            var result = SIGABA.Decode(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
