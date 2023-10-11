using Common;
using Data.Models;
using Data.RequestModels;
using DataAccess.Interfaces;
using DataAccess.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace WebDxMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly TestContext _dbContext;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IStockRepository _stockRepository;

        public HomeController(TestContext dbContext, IConnectionStringProvider connectionStringProvider, IStockRepository stockRepository)
        {
            _dbContext = dbContext;
            _connectionStringProvider = connectionStringProvider;
            _stockRepository = stockRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
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
            //Veriler uygun hale getirilir.
            string stockCode = model.StockCode.Trim();
            int startDate = Convert.ToInt32((model.StartDate).ToOADate());
            int endDate = Convert.ToInt32((model.EndDate).ToOADate());

            //Sonuç repository'den alınır.
            List<StockMovement> stockMovementList = await _stockRepository.GetStockMovements(stockCode, startDate, endDate);

            if (stockMovementList != null && stockMovementList.Count > 0)
            {
                //Eğer data varsa stok hesaplaması yapılır.
                decimal stock = 0;

                foreach (StockMovement item in stockMovementList)
                {
                    //TODO string karşılaştırması değiştirilmesi gerekli.
                    if (item.IslemTur == "Giriş")
                    {
                        stock = stock + item.GirisMiktar;
                    }
                    else if (item.IslemTur == "Çıkış")
                    {
                        stock = stock - item.CikisMiktar;
                    }
                    else
                    {
                        //do nothing
                    }

                    item.Stok = stock;
                }

                return View(stockMovementList);
            }
            else
            {
                return View();
            }
        }
    }
}
