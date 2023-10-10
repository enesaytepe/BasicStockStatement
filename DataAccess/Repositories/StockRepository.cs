using Data.Models;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Repositories
{
    public class StockRepository
    {
        private readonly TestContext _dbContext;

        public StockRepository(TestContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<StockMovement>> GetStockMovements(string stockCode, int startDate, int endDate)
        {
            //TODO change result model
            List<StockMovement> result = new List<StockMovement>();

            try
            {
                List<StockMovement> stockMovementList = await _dbContext.StockMovements
        .FromSqlRaw("EXEC dbo.GetStockMovements @MalKodu, @BaslangicTarihi, @BitisTarihi",
            new SqlParameter("@MalKodu", stockCode),
            new SqlParameter("@BaslangicTarihi", startDate),
            new SqlParameter("@BitisTarihi", endDate))
        .ToListAsync();

                if (stockMovementList != null && stockMovementList.Count > 0)
                {
                    result.AddRange(stockMovementList);

                    //TODO must be add success value on result model
                }
                else
                {
                    //TODO data not found, add error on result
                }

            }
            catch (Exception ex)
            {
                //TODO add error on result
            }

            return result;
        }

        public async Task<PagedDataModel<StockMovement>> GetPagedStockMovements(string stockCode, int startDate, int endDate, int pageNumber, int pageSize)
        {
            //TODO change result model
            PagedDataModel<StockMovement> result = new PagedDataModel<StockMovement>();
            result.DataList = new List<StockMovement>();

            try
            {
                //Total page from stored procedure
                var totalPagesParam = new SqlParameter("@TotalPages", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                List<StockMovement> stockMovementList = await _dbContext.PagedStockMovements
           .FromSqlRaw("EXEC dbo.GetPagedStockMovements @MalKodu, @BaslangicTarihi, @BitisTarihi, @PageNumber, @PageSize, @TotalPages OUTPUT",
               new SqlParameter("@MalKodu", stockCode),
               new SqlParameter("@BaslangicTarihi", startDate),
               new SqlParameter("@BitisTarihi", endDate),
               new SqlParameter("@PageNumber", pageNumber),
               new SqlParameter("@PageSize", pageSize),
               totalPagesParam // OUTPUT parametresi
           ).ToListAsync();

                if (stockMovementList != null && stockMovementList.Count > 0)
                {
                    result.PageSize = pageSize;
                    result.PageNumber = pageNumber;
                    result.TotalPage = (int)totalPagesParam.Value;
                    result.DataList.AddRange(stockMovementList);

                    //TODO must be add success value on result model
                }
                else
                {
                    //TODO data not found, add error on result
                }

            }
            catch (Exception ex)
            {
                //TODO add error on result
            }

            return result;
        }
    }
}
