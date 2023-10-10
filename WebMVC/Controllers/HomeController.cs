using Data.Models;
using Data.RequestModels;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> StockMovementList()
        {
            // Model geçerli değilse hata mesajlarıyla birlikte formu tekrar göster
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StockMovementList(StockMovementFilterModel model)
        {
            //Repository tanımlaması yapılır.
            StockRepository stockRepository = new StockRepository(_dbContext);

            //Veriler uygun hale getirilir.
            string stockCode = model.StockCode.Trim();
            int startDate = Convert.ToInt32((model.StartDate).ToOADate());
            int endDate = Convert.ToInt32((model.EndDate).ToOADate());

            //Sonuç repository'den alınır.
            List<StockMovement> stockMovementList = await stockRepository.GetStockMovements(stockCode, startDate, endDate);

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

        [HttpGet]
        public async Task<IActionResult> PagedStockMovementList(PagedStockMovementFilterModel model)
        {
            if (!String.IsNullOrEmpty(model.StockCode))
            {
                //Veriler uygun hale getirilir.
                string stockCode = model.StockCode.Trim();
                int startDate = Convert.ToInt32((model.StartDate).ToOADate());
                int endDate = Convert.ToInt32((model.EndDate).ToOADate());
               int pageNumber = model.PageNumber < 1 ? 1 : model.PageNumber;

                //Default page size tanımlaması
                int pageSize = 5;

                //Repository tanımlaması yapılır.
                StockRepository stockRepository = new StockRepository(_dbContext);

                //Sonuç repository'den alınır.
                PagedDataModel<StockMovement> stockMovementList = await stockRepository.GetPagedStockMovements(stockCode, startDate, endDate, pageNumber, pageSize);

                //Toplam sayfa sayısı, sayfalama elementlerinin oluşturulabilmesi için gönderilir.
                ViewData["TotalPages"] = stockMovementList.TotalPage;

                if (stockMovementList != null && stockMovementList.DataList != null && stockMovementList.DataList.Count > 0)
                {
                    //Eğer data varsa stok hesaplaması yapılır.
                    decimal stock = 0;

                    //TODO Stok hesaplaması, sayfalamalı stok hareketleri sayfasında nasıl olmalı?

                    //foreach (StockMovement item in stockMovementList.DataList)
                    //{
                    //    //TODO string karşılaştırması değiştirilmesi gerekli.
                    //    if (item.IslemTur == "Giriş")
                    //    {
                    //        stock = stock + item.GirisMiktar;
                    //    }
                    //    else if (item.IslemTur == "Çıkış")
                    //    {
                    //        stock = stock - item.CikisMiktar;
                    //    }
                    //    else
                    //    {
                    //        //do nothing
                    //    }

                    //    item.Stok = stock;
                    //}

                    PagedStockMovementListViewModel pagedStockMovementListViewModel = new PagedStockMovementListViewModel();
                    pagedStockMovementListViewModel.PagedData = stockMovementList;
                    pagedStockMovementListViewModel.Filter = model;

                    return View(pagedStockMovementListViewModel);
                }
            }

            return View();
        }
    }
}