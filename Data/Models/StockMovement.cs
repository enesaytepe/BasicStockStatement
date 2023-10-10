namespace Data.Models
{
    /// <summary>
    /// GetStockMovements(Stored Procedure) dönüş bilgilerini almak için oluşturuldu.
    /// </summary>
    public class StockMovement
    {
        public int SiraNo { get; set; }
        public string IslemTur { get; set; }
        public string EvrakNo { get; set; }
        public string Tarih { get; set; }
        public decimal GirisMiktar { get; set; }
        public decimal CikisMiktar { get; set; }
        public decimal Stok { get; set; }
    }
}
