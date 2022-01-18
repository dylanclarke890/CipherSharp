using CipherSharp.Ciphers.Polyalphabetic;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class EnigmaTests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            var result = Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);

            // Assert
            Assert.Equal("FFPMNIQOQC", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            var result = Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }
    }
}
