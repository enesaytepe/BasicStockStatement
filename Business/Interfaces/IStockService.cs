using Data.Models;
using Data.RequestModels;

namespace Business.Interfaces
{
    public interface IStockService
    {
        Task<List<StockMovement>> GetStockMovements(StockMovementFilterModel model);
        Task<PagedDataModel<StockMovement>> GetPagedStockMovements(PagedStockMovementFilterModel model);
    }
}
