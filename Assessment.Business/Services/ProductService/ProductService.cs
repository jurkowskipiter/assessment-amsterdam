using Assessment.Domain.ChannelEngineClients;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assessment.Business.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IChannelEngineClient _channelEngineClient;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IChannelEngineClient channelEngineClient, ILogger<ProductService> logger)
        {
            _channelEngineClient = channelEngineClient;
            _logger = logger;
        }

        public IChannelEngineClient ChannelEngineClient { get; }
        public ILogger<ProductService> Logger { get; }

        public async Task<bool> SetProductStockAsync(int stockValue, string? merchantProductNo)
        {
            if (string.IsNullOrEmpty(merchantProductNo))
            {
                _logger.LogError("Invalid Merchant product number provided.");
                return false;
            }
            var content = new StringContent(SerializeStockData(stockValue), Encoding.UTF8, "text/plain");
            return await _channelEngineClient.SetProductStock(content, merchantProductNo);
        }

    private string SerializeStockData(int stockValue)
    {
        var jsonArray = new[]
        {
                new
                {
                    op = "replace",
                    value = stockValue,
                    path = "Stock"
                }
            };

        return JsonConvert.SerializeObject(jsonArray, Formatting.Indented);
    }
}
}
