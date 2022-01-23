using CipherSharp.Ciphers.Polyalphabetic;
using System;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class SIGABATests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";
            SIGABA sigaba = new(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos);
            // Act
            var result = sigaba.Encode();

            // Assert
            Assert.Equal("PDVJEZXFCE", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "PDVJEZXFCE";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";
            SIGABA sigaba = new(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos);
            // Act
            var result = sigaba.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData(null, "TABLE", "GRAPH", "02367")]
        [InlineData("HELLOWORLD", null, "GRAPH", "02367")]
        [InlineData("HELLOWORLD", "TABLE", null, "02367")]
        [InlineData("HELLOWORLD", "TABLE", "GRAPH", null)]
        public void NewInstance_NullStrings_ThrowsArgumentException(string text, string indicatorKey,
            string controlPos, string indexPos)
        {
            // Arrange
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new SIGABA(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos));
        }

        [Fact]
        public void NewInstance_NullCipherKey_ArgumentNullException()
        {
            // Arrange
            string text = "PDVJEZXFCE";
            List<string> cipherKey = null;
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SIGABA(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos));
        }

        [Fact]
        public void NewInstance_NullControlKey_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "PDVJEZXFCE";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = null;
            List<string> indexKey = new() { "II", "I", "V", "IV", "III" };
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SIGABA(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos));
        }

        [Fact]
        public void NewInstance_NullIndexKey_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "PDVJEZXFCE";
            List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };
            List<string> controlKey = new() { "IX", "VI", "I", "VII", "VIII" };
            List<string> indexKey = null;
            string indicatorKey = "TABLE";
            string controlPos = "GRAPH";
            string indexPos = "02367";

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new SIGABA(text, cipherKey, controlKey, indexKey,
                indicatorKey, controlPos, indexPos));
        }
    }
}
