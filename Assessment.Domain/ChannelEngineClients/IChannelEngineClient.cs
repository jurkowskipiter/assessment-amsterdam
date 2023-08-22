using Assessment.Domain.Models;

namespace Assessment.Domain.ChannelEngineClients
{
    public interface IChannelEngineClient
    {
        public Task<string> GetOrdersByStatus(string status);

        public Task<bool> SetProductStock(HttpContent httpContent, string merchantProductNo);
    }
}
