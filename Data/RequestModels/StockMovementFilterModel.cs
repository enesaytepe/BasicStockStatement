namespace Data.RequestModels
{
    public class StockMovementFilterModel
    {
        public string StockCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
