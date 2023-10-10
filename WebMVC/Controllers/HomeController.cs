using Data.RequestModels;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestContext _dbContext;

        public HomeController(ILogger<HomeController> logger, TestContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> StockMovementList()
        {
            // Model geçerli değilse hata mesajlarıyla birlikte formu tekrar göster
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StockMovementList(StockMovementFilterModel model)
        {
            if (ModelState.IsValid)
            {
                StockRepository stockRepository = new StockRepository(_dbContext);

                //string stockCode = "10081 SİEMENS";
                //int startDate = 42736;
                //int endDate = 42775;

                string stockCode = model.StockCode;
                int startDate = Convert.ToInt32((model.StartDate).ToOADate());
                int endDate = Convert.ToInt32((model.EndDate).ToOADate());

                var data = await stockRepository.GetStockMovements(stockCode, startDate, endDate);

                // Filtrelenmiş verileri view'e gönder
                return View(data);
            }

            // Model geçerli değilse hata mesajlarıyla birlikte formu tekrar göster
            return View(model);
        }
    }
}