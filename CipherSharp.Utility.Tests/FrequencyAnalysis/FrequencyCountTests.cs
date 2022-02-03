using CipherSharp.Utility.FrequencyAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CipherSharp.Utility.Tests.FrequencyAnalysis
{
    public class FrequencyCountTests
    {
        private readonly string _textString700;

        public FrequencyCountTests()
        {
            _textString700 = File.ReadAllText("../../../data/caesar.txt");
        }

        [Fact]
        public void Monogram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string msg = "HELLOWORLD";
            var frequencyCount = new FrequencyCount(msg);

            // Act
            var result = frequencyCount.Monogram();

            Dictionary<char, decimal> expected = new()
            {
                ['H'] = 10.0M,
                ['E'] = 10.0M,
                ['L'] = 30.0M,
                ['O'] = 20.0M,
                ['W'] = 10.0M,
                ['R'] = 10.0M,
                ['D'] = 10.0M,
            };

            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(100, Math.Floor(result.Sum(kv => kv.Value)));
        }

        [Fact]
        public void Monogram_LongerText_ExpectedBehavior()
        {
            // Arrange
            string msg = _textString700;
            var frequencyCount = new FrequencyCount(msg);

            // Act
            var result = frequencyCount.Monogram();

            // Assert
            Assert.Equal(100, Math.Round(result.Sum(kv => kv.Value)));
        }

        [Fact]
        public void Bigram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string msg = "HELLOWORLD";
            var frequencyCount = new FrequencyCount(msg);

            // Act
            var result = frequencyCount.Bigram();

            Dictionary<string, decimal> expected = new()
            {
                ["HE"] = 10.0M,
                ["EL"] = 10.0M,
                ["LL"] = 10.0M,
                ["LO"] = 10.0M,
                ["OW"] = 10.0M,
                ["WO"] = 10.0M,
                ["OR"] = 10.0M,
                ["RL"] = 10.0M,
                ["LD"] = 10.0M,
            };

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Trigram_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string msg = "HELLOWORLD";
            var frequencyCount = new FrequencyCount(msg);

            // Act
            var result = frequencyCount.Trigram();

            Dictionary<string, decimal> expected = new()
            {
                ["HEL"] = 10.0M,
                ["ELL"] = 10.0M,
                ["LLO"] = 10.0M,
                ["LOW"] = 10.0M,
                ["OWO"] = 10.0M,
                ["WOR"] = 10.0M,
                ["ORL"] = 10.0M,
                ["RLD"] = 10.0M,
            };

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
