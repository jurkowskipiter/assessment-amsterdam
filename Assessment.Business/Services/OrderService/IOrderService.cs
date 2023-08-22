using Assessment.Domain.Models;

namespace Assessment.Business.Services.OrderService
{
    public interface IOrderService
    {
        
        /// <summary>
        /// Returns list of orders with status in progress
        /// </summary>
        public IEnumerable<Order> GetAllOrdersWithStatusInProgress();

        /// <summary>
        /// Returns list of orders with status in progress
        /// </summary>
        public IEnumerable<Product> GetTopFiveProductsSold();
    }
}
