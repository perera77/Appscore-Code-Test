using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppscoreAncestry;
using AppscoreAncestry.Controllers;

namespace AppscoreAncestry.Tests.Controllers
{
    [TestClass]
    public class AncestryControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            AncestryController controller = new AncestryController();

            // Act
            IHttpActionResult result = controller.searchPeople("Mill");

            // Assert
            Assert.IsNotNull(result);

            // Act
            result = controller.advancedSearch("Caitlin Millisent", female: true, directionAncestors: true );

            // Assert
            Assert.IsNotNull(result);

        }

       
    }
}
