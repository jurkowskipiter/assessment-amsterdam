using Assessment.Business.Services.OrderService;
using Assessment.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Assessment.WebApi.Controllers
{
    public class ChannelEngineController : Controller
    {
        private readonly IOrderService _orderService;

        public ChannelEngineController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult PrivacyTwo()
        {
            return View();
        }

        public IActionResult TopFiveProducts()
        {
            var topFiveSoldProducts = _orderService.GetTopFiveProductsSold();

            return View(topFiveSoldProducts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}