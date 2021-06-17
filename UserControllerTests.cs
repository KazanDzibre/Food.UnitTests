using System;
using AutoMapper;
using FluentAssertions;
using Food.Configuration;
using Food.Controllers;
using Food.Core;
using Food.Model;
using Food.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Food.UnitTests
{
    public class UserControllerTests
    {

        private readonly Mock<IMapper> mapperStub = new(); 
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Random rand = new();

        [Fact]
        public void GetUserById_WithUnexistingUser_ReturnsNotFound()
        {
            //Arrange
            var Controller = new UserController(mapperStub.Object, _userService.Object);
            //Act

            var result = Controller.GetUserById(2);
            //Assert

            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public void GetUserById_WithExistingUser_ReturnsExpectedItem()
        {
            //Arrange
            var ExpectedUser = CreateRandomUser();
            _userService.Setup(p => p.GetUserById(2)).Returns(ExpectedUser);
            var Controller = new UserController(mapperStub.Object, _userService.Object);

            //Act
            var result = (OkObjectResult)Controller.GetUserById(2).Result;
            //Assert

            result.Value.Should().BeEquivalentTo(ExpectedUser,
                    options => options.ComparingByMembers<User>());
        }

        private User CreateRandomUser()
        {
            return new()
            {
                Id = 2,
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                Type = Guid.NewGuid().ToString(),
                latitude = rand.Next(45),
                longitude = rand.Next(45),
                RegistrationToken = "",
                Password = Guid.NewGuid().ToString()
            };
        }
    }
}
