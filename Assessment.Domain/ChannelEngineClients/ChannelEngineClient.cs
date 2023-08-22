using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Assessment.Domain.ChannelEngineClients
{
    public class ChannelEngineClient : IChannelEngineClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IChannelEngineClient> _logger;

        public ChannelEngineClient(HttpClient httpClient, ILogger<IChannelEngineClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri($"{ChannelEngineConfiguration.BaseUrl}");
        }

        public async Task<string> GetOrdersByStatus(string status)
        {
            using HttpResponseMessage response = await _httpClient
                .GetAsync($"/api/v2/orders?statuses={status}&apikey={ChannelEngineConfiguration.ApiKey}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"HTTP request failed with status code: {response.StatusCode}");

                return $"HTTP request failed with status code: {response.StatusCode}";
            }
        }

        public async Task<bool> SetProductStock(HttpContent httpContent, string merchantProductNo)
        {
            using HttpResponseMessage response = await _httpClient
                  .PatchAsync($"/api/v2/products/{merchantProductNo}?&apikey={ChannelEngineConfiguration.ApiKey}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                _logger.LogError($"HTTP request failed with status code: {response.StatusCode}");

                return false;
            }
        }
    }
}
