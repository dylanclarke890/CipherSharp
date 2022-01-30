using CipherSharp.Attacks;
using CipherSharp.Utility.FrequencyAnalysis;
using System;
using Xunit;

namespace CipherSharp.Attacks.Tests
{
    public class CaesarCrackerTests
    {
        [Fact]
        public void BruteForce_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var caesarCracker = new CaesarCracker(new FrequencyCount(), "IFMMPXPSME");

            // Act
            var result = caesarCracker.BruteForce();

            // Assert
            Assert.Equal(new(), result);
        }

        [Fact]
        public void FrequencyAnalysisGuess_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var caesarCracker = new CaesarCracker(new FrequencyCount(), "IFMMPXPSME");

            // Act
            var result = caesarCracker.FrequencyAnalysisGuesses();

            // Assert
            Assert.Equal(new(), result);
        }
    }
}
