using Business.Interfaces;
using Common;
using Data.Models;
using Data.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace WebDxMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IStockService _stockManager;

        public HomeController(ILogger<HomeController> logger, IConnectionStringProvider connectionStringProvider, IStockService stockManager)
        {
            _logger = logger;
            _connectionStringProvider = connectionStringProvider;
            _stockManager = stockManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StockMovementList()
        {
            // Model geçerli değilse hata mesajlarıyla birlikte formu tekrar göster
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StockMovementList(StockMovementFilterModel model)
        {
            ExecutionResult<List<StockMovement>> result = await _stockManager.GetStockMovements(model);

            if (result.Success)
            {
                return View(result);
            }
            else
            {
                //TODO log
            }

            return View();
        }
    }
}
