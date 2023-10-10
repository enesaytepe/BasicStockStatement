using Data.Models;
using Data.RequestModels;

namespace WebMVC.Models
{
    public class PagedStockMovementListViewModel
    {
        public PagedDataModel<StockMovement> PagedData { get; set; }
        public PagedStockMovementFilterModel Filter { get; set; }
    }
}
