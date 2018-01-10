using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppscoreAncestry;
using AppscoreAncestry.Controllers;

namespace AppscoreAncestry.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Simple Search", result.ViewBag.Title);
        }

        [TestMethod]
        public void AdvancedSearch()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.advancedSearch() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual("Simple Search", result.ViewBag.Title);
        }
    }
}
