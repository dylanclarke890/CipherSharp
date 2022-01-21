using CipherSharp.Utility.Extensions;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void IndirectSort_BasicParameters_ReturnsListOfIndicesToSortInput()
        {
            // Arrange
            IEnumerable<string> array = new List<string>() { "B", "C", "A" };

            // Act
            var result = array.IndirectSort();

            // Assert
            Assert.Equal(new List<int>() { 2, 0, 1 }, result);
        }

        [Fact]
        public void IndexWhere_BasicParameters_ReturnsListOfIndicesThatFitCriteria()
        {
            // Arrange
            IEnumerable<string> array = new List<string>() { "B", "C", "A" };

            // Act
            var result = array.IndexWhere(x => x == "B" || x == "C");

            // Assert
            Assert.Equal(new List<int>() { 0, 1 }, result);
        }
    }
}
