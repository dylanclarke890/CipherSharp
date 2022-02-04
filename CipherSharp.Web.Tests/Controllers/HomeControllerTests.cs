using CipherSharp.Web.Controllers;
using Xunit;

namespace CipherSharp.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        public HomeControllerTests()
        {
        }

        private HomeController CreateHomeController()
        {
            return new HomeController();
        }

        [Fact]
        public void Index_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = CreateHomeController();

            // Act
            var result = homeController.Index();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void About_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = CreateHomeController();

            // Act
            var result = homeController.About();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Contact_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var homeController = CreateHomeController();

            // Act
            var result = homeController.Contact();

            // Assert
            Assert.True(false);
        }
    }
}
