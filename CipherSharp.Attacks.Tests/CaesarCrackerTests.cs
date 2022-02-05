using System.IO;
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
            var caesarCracker = new CaesarCracker(EncodedText);

            // Act
            var result = caesarCracker.BruteForce();

            // Assert
            Assert.Equal(25, result.Count);
        }

        [Fact]
        public void FrequencyAnalysisAlternate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var caesarCracker = new CaesarCracker(EncodedText);

            // Act
            var result = caesarCracker.FrequencyAnalysis();

            // Assert
            Assert.Equal(5, result.Count);
        }
    }
}
