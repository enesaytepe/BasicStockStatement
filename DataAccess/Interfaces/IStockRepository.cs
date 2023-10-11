using Data.Models;

namespace DataAccess.Interfaces
{
    public interface IStockRepository
    {
        Task<List<StockMovement>> GetStockMovements(string stockCode, int startDate, int endDate);
        Task<PagedDataModel<StockMovement>> GetPagedStockMovements(string stockCode, int startDate, int endDate, int pageNumber, int pageSize);
    }
}
