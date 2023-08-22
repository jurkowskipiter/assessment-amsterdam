using Assessment.Business.Services.OrderService;
using Assessment.Business.Services.ProductService;
using Assessment.Domain.ChannelEngineClients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScoped<HttpClient>();
        services.AddScoped<IChannelEngineClient, ChannelEngineClient>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddLogging(builder => builder.AddConsole());
    });

using var host = builder.Build();

var orderService = host.Services.GetRequiredService<IOrderService>();
var productService = host.Services.GetRequiredService<IProductService>();


//------------------------------------------------------------//

var topFiveProducts = orderService.GetTopFiveProductsSold();

if(topFiveProducts.Any())
{
    Console.WriteLine("Top 5 products sold:");
    foreach (var product in topFiveProducts)
    {
        Console.WriteLine($"Name: {product.Name} + EAN: {product.Ean ?? "N/A"} Total Quantity: {product.TotalQuantity}");
    }
}

else
{
    Console.WriteLine("No products found");
}

//------------------------------------------------------------//

const int productStockValue = 25;
Console.WriteLine("Setting one of the product stock to 25");

var merchantProductNo = topFiveProducts.FirstOrDefault()?.MerchantProductNo;
if (await productService.SetProductStockAsync(productStockValue, merchantProductNo))
{
    Console.WriteLine("\n Product stock set");
}
else
{
    Console.WriteLine("\n Product stock not set");
}

Console.Read();
