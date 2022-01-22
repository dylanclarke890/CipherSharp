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

        [Fact]
        public void Encode_NullText_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Encode_NullRotorKeys_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Encode_NullReflectorKey_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Encode_NullPositionsKey_ThrowsArgumentException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = null;
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Encode_NullPlugs_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Encode_NullRingKeys_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Encode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullText_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullRotorKeys_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullReflectorKey_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullPositionsKey_ThrowsArgumentException()
        {
            // Arrange
            string text = "FFPMNIQOQC";
            List<string> rotorKeys = new() { "I", "II", "III" };
            string reflectorKey = "A";
            List<string> positionsKey = null;
            List<string> plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
            List<string> ringKeys = new() { "A", "B", "C" };

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullPlugs_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }

        [Fact]
        public void Decode_NullRingKeys_ThrowsArgumentException()
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
            Assert.Throws<ArgumentException>(() => Enigma.Decode(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys));
        }
    }
}
