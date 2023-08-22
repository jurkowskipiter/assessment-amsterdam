using Assessment.Domain.ChannelEngineClients;
using Assessment.Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Assessment.Business.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IChannelEngineClient _channelEngineClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IChannelEngineClient channelEngineClient, ILogger<OrderService> logger)
        {
            _channelEngineClient = channelEngineClient;
            _logger = logger;
        }

        public IEnumerable<Order> GetAllOrdersWithStatusInProgress()
        {
            const string status = "IN_PROGRESS";
            var ordersToDeserialize = _channelEngineClient.GetOrdersByStatus(status).Result;

            try
            {
                return JsonConvert.DeserializeObject<OrderResponse>(ordersToDeserialize)?.Content ?? Enumerable.Empty<Order>();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error during deserialization: {ex.Message}");
                return Enumerable.Empty<Order>();
            }
        }

        public IEnumerable<Product> GetTopFiveProductsSold()
        {
            var orders = GetAllOrdersWithStatusInProgress();

            return GetTopProductsByQuantity(orders.SelectMany(o => o.Lines), 5);
        }

        private IEnumerable<Product> GetTopProductsByQuantity(IEnumerable<Line> lines, int topCount)
        {
            var topFiveDescending = lines.OrderByDescending(line => line.Quantity)
                         .Take(topCount)
                         .ToList();

            return topFiveDescending.Select(line => new Product
            {
                Name = line.Description,
                Ean = line.Gtin,
                TotalQuantity = line.Quantity,
                MerchantProductNo = line.MerchantProductNo
            }).ToList();
        }
    }
}
