using Assessment.Business.Services.OrderService;
using Assessment.Domain.Models;
using Moq;
using Xunit;

namespace Assessment.UnitTests
{
    public class OrderServiceTests
    {
        [Fact]
        public void ShouldReturnTopFiveProductsGivenMockedOrderWithMoreThanFiveOrdersAndDifferentQuantity()
        {
            //Arrange
            const int expectedNumberOfItems = 5;

            var orders = new List<Order>
            {
                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "First Product", Quantity = 6, Gtin = "5234567890123" },
                    }
                },
                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "Second Product", Quantity = 4, Gtin = "9876543210984" },
                    }
                },

                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "Second Product", Quantity = 5, Gtin = "9876543210983" },
                    }
                },

                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "Second Product", Quantity = 2, Gtin = "9876543210981" },
                    }
                },

                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "Second Product", Quantity = 3, Gtin = "9876543210989" },
                    }
                },

                new Order
                {
                    Lines = new List<Line>
                    {
                        new Line { Description = "Second Product", Quantity = 1, Gtin = "9876543210980" },
                    }
                },
            };
            var orderServiceMock = new Mock<OrderService>();
            orderServiceMock.Setup(os => os.GetAllOrdersWithStatusInProgress()).Returns(orders);

            //Act
            var topFiveProducts = orderServiceMock.Object.GetTopFiveProductsSold();

            //Assert
            Assert.NotEmpty(topFiveProducts);
            Assert.Equal(expectedNumberOfItems, topFiveProducts.Count());
            Assert.Contains(topFiveProducts, tfp => tfp.TotalQuantity == 6);
            Assert.DoesNotContain(topFiveProducts, tfp => tfp.TotalQuantity == 1);
        }
    }
}
