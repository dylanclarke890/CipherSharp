using CipherSharp.Enums;
using CipherSharp.Helpers;
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
            string[][] expected = new string[][]
            {
                new string[]{ "TESABC" },
                new string[]{ "DFGHIJ" },
                new string[]{ "KLMNOP" },
                new string[]{ "QRUVWX" },
                new string[]{ "YZ0123" },
                new string[]{ "456789" },
            };
            Assert.Equal(expected, result);
        }
    }
}
