using AutoMapper;
using FluentAssertions;
using Food.Controllers;
using Food.Core;
using Food.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Food.UnitTests
{
	public class OrderControllertests
	{
		private readonly Mock<IMapper> mapperStub = new();

		private readonly Mock<IOrderService> _orderService = new Mock<IOrderService>();

		[Fact]
		public void GetOrderById_WithExistingOrder_ReturnsExpectedOrder() 
		{
			//Arrange
			var ExpectedOrder = CreateRandomOrder(2);
			_orderService.Setup(p => p.GetById(2)).Returns(ExpectedOrder);
			var Controller = new OrderController(mapperStub.Object, _orderService.Object);

			var result = (OkObjectResult)Controller.GetOrderById(2).Result;

			result.Value.Should().BeEquivalentTo(ExpectedOrder,
					options => options.ComparingByMembers<Order>());
		}

		[Fact]
		public void GetAll_WithExistingOrders_ReturnsAllOrders()
		{
			//Arrange
			var ExpectedOrders = new []{ CreateRandomOrder(2), CreateRandomOrder(3), CreateRandomOrder(4) };
			_orderService.Setup(p => p.GetAll()).Returns(ExpectedOrders);
			var Controller = new OrderController(mapperStub.Object, _orderService.Object);

			//Act
			var results = (OkObjectResult)Controller.GetAll().Result;

			//Assert
			results.Value.Should().BeEquivalentTo(ExpectedOrders,
					options => options.ComparingByMembers<Order>());	
		}

		private Order CreateRandomOrder(int n)
		{
			return new()
			{
				Id = n,
				Restaurant = "Kfc",
				Address = "Safarikova 35",
				FoodChoice = "Krilca"
			};
		}

	}
}
