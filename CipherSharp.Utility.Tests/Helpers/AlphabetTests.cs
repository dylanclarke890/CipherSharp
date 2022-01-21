using CipherSharp.Utility.Helpers;
using Xunit;

namespace CipherSharp.Tests.Helpers
{
    public class AlphabetTests
    {
        [Fact]
        public void AlphabetPermutation_BasicParameters_ReturnsPermutatedAlphabet()
        {
            // Arrange
            string key = "test";
            string alphabet = AppConstants.Alphabet;

            // Act
            var result = Alphabet.AlphabetPermutation(key, alphabet);

            // Assert
            Assert.Equal("TESABCDFGHIJKLMNOPQRUVWXYZ", result);
        }
    }
}
