using System;
using AutoMapper;
using Food.Configuration;
using Food.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Food.UnitTests
{
    public class UserControllerTests
    {
        [Fact]
        public void GetUserById_WithUnexistingUser_ReturnsNotFound()
        {
            //Arrange
            var mapperStub = new Mock<IMapper>();
            ProjectConfiguration configuration = new ProjectConfiguration();
            var Controller = new UserController(mapperStub.Object, configuration);
            //Act

            var result = Controller.GetUserById(2);
            //Assert

            Assert.IsType<NotFoundResult>(result.Result);

        }
    }
}
