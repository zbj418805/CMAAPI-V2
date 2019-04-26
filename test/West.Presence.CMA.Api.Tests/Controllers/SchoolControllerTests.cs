using Microsoft.AspNetCore.Mvc;
using Moq;
using West.Presence.CMA.Api.Controllers;
using West.Presence.CMA.Api.Model;
using Xunit;

namespace West.Presence.CMA.Api.Tests.Controllers
{
    public class SchoolControllerTests
    {
        private SchoolsController _sut;

        [Fact]
        public void Test_SchoolController_Returns_OK()
        {
            // Arrange
            _sut = new SchoolsController(null);
            // Act
            var result = _sut.GetSchools(null, new QueryFilter(), null,"");

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, _sut.ModelState.ErrorCount);
        }
    }
}
