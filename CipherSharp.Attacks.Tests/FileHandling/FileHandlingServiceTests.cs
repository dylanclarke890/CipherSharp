using CipherSharp.Utility.FileHandling;
using System;
using Xunit;

namespace CipherSharp.Attacks.Tests.FileHandling
{
    public class FileHandlingServiceTests
    {
        [Fact]
        public void GetFile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = new FileHandlingService();
            string path = "caesar.txt";

            // Act
            var result = service.GetFile(path);

            // Assert
            Assert.NotEmpty(result);
        }
    }
}
