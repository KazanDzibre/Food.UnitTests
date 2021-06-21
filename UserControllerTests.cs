using System;
using AutoMapper;
using FluentAssertions;
using Food.Configuration;
using Food.Controllers;
using Food.Core;
using Food.Dtos;
using Food.Model;
using Food.Profiles;
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
            var ExpectedUser = CreateRandomUser(2);
            _userService.Setup(p => p.GetUserById(2)).Returns(ExpectedUser);
            var Controller = new UserController(mapperStub.Object, _userService.Object);

            //Act
            var result = (OkObjectResult)Controller.GetUserById(2).Result;
            //Assert

            result.Value.Should().BeEquivalentTo(ExpectedUser,
                    options => options.ComparingByMembers<User>());
        }

        [Fact]
        public void GetAll_WithExistingUsers_ReturnsAllUsers()
        {
            //Arrange
            var ExpectedUsers = new []{ CreateRandomUser(2), CreateRandomUser(3), CreateRandomUser(4) };
            _userService.Setup(p => p.GetAll()).Returns(ExpectedUsers);
            var Controller = new UserController(mapperStub.Object, _userService.Object);

            //Act
            var results = (OkObjectResult)Controller.GetAll().Result;

            //Assert
            results.Value.Should().BeEquivalentTo(ExpectedUsers,
                    options => options.ComparingByMembers<User>());
        }


		// [Fact]
		// public void Register_WithExistingUser_ReturnsRegisteredUser()
		// {
			// var myProfile = new UserProfile();
			// var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			// var mapper = new Mapper(configuration);
			
			// var RegisterDto = new UserRegisterDto()
			// {
				// FirstName = "Marko",
				// LastName = "Ruzic",
				// UserName = "markoruzic997",
				// Type = "Admin",
				// longitude = 15,
				// latitude = 15,
				// Password = "password"
			// };

			// User ExpectedUser = mapper.Map<User>(RegisterDto);
			// _userService.Setup(p => p.Add(ExpectedUser)).Returns(ExpectedUser);

			// var Controller = new UserController(mapper , _userService.Object);
			// //Act

			// var result = (OkObjectResult)Controller.Register(RegisterDto).Result;

			// //Assert

			// result.Value.Should().BeEquivalentTo(RegisterDto,
					// options => options.ComparingByMembers<User>().ExcludingMissingMembers());

		// }
		
        private User CreateRandomUser(int n)
        {
            return new()
            {
                Id = n,
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
