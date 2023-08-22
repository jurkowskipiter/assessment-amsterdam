namespace Assessment.Business.Services.ProductService
{
    public interface IProductService
    {
        public Task<bool> SetProductStockAsync(int stockValue, string? merchantProductNo);
    }
}
