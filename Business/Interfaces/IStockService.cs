using Common;
using Data.Models;
using Data.RequestModels;

namespace Business.Interfaces
{
    public interface IStockService
    {
        Task<ExecutionResult<List<StockMovement>>> GetStockMovements(StockMovementFilterModel model);
        Task<ExecutionResult<PagedDataModel<StockMovement>>> GetPagedStockMovements(PagedStockMovementFilterModel model);
    }
}
