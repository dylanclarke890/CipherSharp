using CipherSharp.Ciphers.Polyalphabetic;
using System;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Ciphers.Polyalphabetic
{
    public class M209Tests
    {
        [Fact]
        public void Encode_BasicParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = new();
            M209 m209 = new(text, wheelKey, pins, lugs);

            // Act
            var result = m209.Encode();

            // Assert
            Assert.Equal("SVOOLDLIOW", result);
        }

        [Fact]
        public void Encode_FullParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "HELLOWORLD";
            string wheelKey = "ABCDEF";
            List<string> pins = new()
            {
                "++-+---++-+-++----++-++---",
                "+--++-+--+++--+--++-+-+--",
                "++----++-+-+++---++++-+",
                "--+-++-++---++-+--+++",
                "-+-+++-++---++-+--+",
                "++-+---+--+--++-+"
            };
            List<List<int>> lugs = new()
            {
                new() { 3, 6 },
                new() { 0, 6 },
                new() { 1, 6 },
                new() { 1, 5 },
                new() { 4, 5 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 5 },
                new() { 2, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 }
            };
            M209 m209 = new(text, wheelKey, pins, lugs);

            // Act
            var result = m209.Encode();

            // Assert
            Assert.Equal("GYDPTENEDJ", result);
        }

        [Fact]
        public void Decode_FullParameters_ReturnsCipherText()
        {
            // Arrange
            string text = "GYDPTENEDJ";
            string wheelKey = "ABCDEF";
            List<string> pins = new()
            {
                "++-+---++-+-++----++-++---",
                "+--++-+--+++--+--++-+-+--",
                "++----++-+-+++---++++-+",
                "--+-++-++---++-+--+++",
                "-+-+++-++---++-+--+",
                "++-+---+--+--++-+"
            };
            List<List<int>> lugs = new()
            {
                new() { 3, 6 },
                new() { 0, 6 },
                new() { 1, 6 },
                new() { 1, 5 },
                new() { 4, 5 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 0, 4 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 0 },
                new() { 2, 5 },
                new() { 2, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 },
                new() { 0, 5 }
            };
            M209 m209 = new(text, wheelKey, pins, lugs);

            // Act
            var result = m209.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Fact]
        public void Decode_BasicParameters_ReturnsPlainText()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = new();
            M209 m209 = new(text, wheelKey, pins, lugs);

            // Act
            var result = m209.Decode();

            // Assert
            Assert.Equal("HELLOWORLD", result);
        }

        [Theory]
        [InlineData("SVOOLDLIOW", null)]
        [InlineData(null, "ABCDEF")]
        public void NewInstance_NullStrings_ThrowsArgumentException(string text, string wheelKey)
        {
            // Arrange
            List<string> pins = new();
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new M209(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void NewInstance_NullPins_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = null;
            List<List<int>> lugs = new();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new M209(text, wheelKey, pins, lugs));
        }

        [Fact]
        public void NewInstance_NullLugs_ThrowsArgumentNullException()
        {
            // Arrange
            string text = "SVOOLDLIOW";
            string wheelKey = "ABCDEF";
            List<string> pins = new();
            List<List<int>> lugs = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => new M209(text, wheelKey, pins, lugs));
        }
    }
}
