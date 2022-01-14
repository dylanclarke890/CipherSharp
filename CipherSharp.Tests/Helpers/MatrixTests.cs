using CipherSharp.Enums;
using CipherSharp.Helpers;
using System.Collections.Generic;
using Xunit;

namespace CipherSharp.Tests.Helpers
{
    public class MatrixTests
    {
        [Fact]
        public void CreateMatrix_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string initialKey = "test";
            AlphabetMode mode = AlphabetMode.EX;

            // Act
            var result = Matrix.Create(initialKey, mode);

            // Assert
            List<List<string>> expected = new()
            {
                new() { "TESABC" },
                new() { "DFGHIJ" },
                new() { "KLMNOP" },
                new() { "QRUVWX" },
                new() { "YZ0123" },
                new() { "456789" },
            };
            Assert.Equal(expected, result);
        }
    }
}
