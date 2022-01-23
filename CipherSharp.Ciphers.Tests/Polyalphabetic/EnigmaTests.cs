using CipherSharp.Ciphers.Polyalphabetic;
using System;
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
            Enigma enigma = new(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);

            // Act
            var result = enigma.Encode();

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
            Enigma enigma = new(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);
            
            // Act
            var result = enigma.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void NewInstance_NullMessage_ThrowsArgumentException()
        {
            // Arrange
            string text = null;
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void NewInstance_NullRotorKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = null;
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void NewInstance_NullReflectorKey_ThrowsArgumentException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = null;
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void NewInstance_NullPositionKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionKeys = null;
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionKeys, plugs, ringKeys));
        }

        [Fact]
        public void NewInstance_NullPlugs_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = null;
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void NewInstance_NullRingKeys_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = new() { "A", "B", "C" };
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new Enigma(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }
    }
}
