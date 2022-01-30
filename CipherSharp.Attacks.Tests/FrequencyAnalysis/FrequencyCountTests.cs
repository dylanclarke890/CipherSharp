using CipherSharp.Attacks.FrequencyAnalysis;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Attacks.Tests.FrequencyAnalysis
{
    public class FrequencyCountTests
    {
        [Fact]
        public void Monogram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var frequencyCount = new FrequencyCount();
            string msg = "HELLOWORLD";

            // Act
            var result = frequencyCount.Monogram(msg);

            Dictionary<char, int> expected = new()
            {
                ['H'] = 1,
                ['E'] = 1,
                ['L'] = 3,
                ['O'] = 2,
                ['W'] = 1,
                ['R'] = 1,
                ['D'] = 1,
            };

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Bigram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var frequencyCount = new FrequencyCount();
            string msg = "HELLOWORLD";

            // Act
            var result = frequencyCount.Bigram(msg);

            Dictionary<string, int> expected = new()
            {
                ["HE"] = 1,
                ["EL"] = 1,
                ["LL"] = 1,
                ["LO"] = 1,
                ["OW"] = 1,
                ["WO"] = 1,
                ["OR"] = 1,
                ["RL"] = 1,
                ["LD"] = 1,
            };

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Trigram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var frequencyCount = new FrequencyCount();
            string msg = "HELLOWORLD";

            // Act
            var result = frequencyCount.Trigram(msg);

            Dictionary<string, int> expected = new()
            {
                ["HEL"] = 1,
                ["ELL"] = 1,
                ["LLO"] = 1,
                ["LOW"] = 1,
                ["OWO"] = 1,
                ["WOR"] = 1,
                ["ORL"] = 1,
                ["RLD"] = 1,
            };

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
