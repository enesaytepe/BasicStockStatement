using Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
                //Test >>>

                stockCode = "10081 SİEMENS";
                startDate = 42736;
                endDate = 42775;

                //<<< Test


                List<StockMovement> stockMovementList = await _dbContext.StockMovements
        .FromSqlRaw("EXEC dbo.GetStockMovements @MalKodu, @BaslangicTarihi, @BitisTarihi",
            new SqlParameter("@MalKodu", stockCode),
            new SqlParameter("@BaslangicTarihi", startDate),
            new SqlParameter("@BitisTarihi", endDate))
        .ToListAsync();

                if (stockMovementList != null && stockMovementList.Count > 0)
                {
                    result.AddRange(result);

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
