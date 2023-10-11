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

        public async Task<List<StockMovement>> GetStockMovements(StockMovementFilterModel model)
        {
            List<StockMovement> result = new List<StockMovement>();

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

                result.AddRange(stockMovementList);
            }
            else
            {
                //TODO add error on result or nothing
            }

            return result;
        }

        public Task<PagedDataModel<StockMovement>> GetPagedStockMovements(PagedStockMovementFilterModel model)
        {
            throw new NotImplementedException();
        }
    }
}
