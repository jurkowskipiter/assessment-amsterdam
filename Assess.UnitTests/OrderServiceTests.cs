using Assessment.Business.Services.OrderService;
using Assessment.Domain.ChannelEngineClients;
using Assessment.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace Assess.UnitTests
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

            string jsonMessage = JsonConvert.SerializeObject(new OrderResponse() { Content = orders});

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonMessage)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var channelEngineClient = new ChannelEngineClient(httpClient, new Mock<ILogger<ChannelEngineClient>>().Object);
            var orderService = new OrderService(channelEngineClient, new Mock<ILogger<OrderService>>().Object);

            //Act
            var topFiveProducts = orderService.GetTopFiveProductsSold();

            //Assert
            Assert.NotEmpty(topFiveProducts);
            Assert.Equal(expectedNumberOfItems, topFiveProducts.Count());
            Assert.Contains(topFiveProducts, tfp => tfp.TotalQuantity == 6);
            Assert.DoesNotContain(topFiveProducts, tfp => tfp.TotalQuantity == 1);
        }
    }
}