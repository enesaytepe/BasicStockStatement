using Business.Interfaces;
using Common;
using Data.Models;
using Data.RequestModels;
using DataAccess.Interfaces;

namespace Business.Managers
{
    public class StockManager : IStockService
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IStockRepository _stockRepository;

        public StockManager(IConnectionStringProvider connectionStringProvider, IStockRepository stockRepository)
        {
            _connectionStringProvider = connectionStringProvider;
            _stockRepository = stockRepository;
        }

        public async Task<ExecutionResult<List<StockMovement>>> GetStockMovements(StockMovementFilterModel model)
        {
            ExecutionResult<List<StockMovement>> result = new ExecutionResult<List<StockMovement>>();

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

                result.Data = stockMovementList;
                result.Success = true;
            }
            else
            {
                //TODO add error on result or nothing
                result.AddError("Data not found.");
            }

            return result;
        }

        public async Task<ExecutionResult<PagedDataModel<StockMovement>>> GetPagedStockMovements(PagedStockMovementFilterModel model)
        {
            ExecutionResult<PagedDataModel<StockMovement>> result = new ExecutionResult<PagedDataModel<StockMovement>>();
            result.Data = new PagedDataModel<StockMovement>();

            if (!String.IsNullOrEmpty(model.StockCode))
            {
                //Veriler uygun hale getirilir.
                string stockCode = model.StockCode.Trim();
                int startDate = Convert.ToInt32((model.StartDate).ToOADate());
                int endDate = Convert.ToInt32((model.EndDate).ToOADate());
                int pageNumber = model.PageNumber < 1 ? 1 : model.PageNumber;

                //Default page size tanımlaması
                int pageSize = 5;

                //Sonuç repository'den alınır.
                PagedDataModel<StockMovement> stockMovementList = await _stockRepository.GetPagedStockMovements(stockCode, startDate, endDate, pageNumber, pageSize);

                if (stockMovementList != null && stockMovementList.DataList != null && stockMovementList.DataList.Count > 0)
                {
                    //Eğer data varsa stok hesaplaması yapılır.
                    //decimal stock = 0;

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

                    result.Data.DataList = stockMovementList.DataList;
                    result.Data.PageNumber = pageNumber;
                    result.Data.PageSize = pageSize;
                    result.Data.TotalPage = stockMovementList.TotalPage;
                    result.Success = true;
                }
                else
                {
                    result.AddError("Data not found.");
                }
            }

            return result;
        }
    }
}
