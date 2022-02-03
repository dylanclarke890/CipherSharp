using CipherSharp.Attacks;
using CipherSharp.Utility.FrequencyAnalysis;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace CipherSharp.Attacks.Tests
{
    public class CaesarCrackerTests
    {
        private readonly string EncodedText;

        public CaesarCrackerTests()
        {
            EncodedText = File.ReadAllText("../../../data/caesar.txt");
        }

        [Fact]
        public void BruteForce_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var caesarCracker = new CaesarCracker(new FrequencyCount(EncodedText), EncodedText);

            // Act
            var result = caesarCracker.BruteForce();

            // Assert
            Assert.Equal(25, result.Count);
        }

        [Fact]
        public void FrequencyAnalysisGuess_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var caesarCracker = new CaesarCracker(new FrequencyCount(EncodedText), EncodedText);

            // Act
            var result = caesarCracker.FrequencyAnalysisGuesses();

            // Assert
            Assert.Equal(new(), result);
        }
    }
}
