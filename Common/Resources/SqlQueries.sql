SELECT TOP (1000) [ID]
      ,[MalKodu]
      ,[MalAdi]
  FROM [Test].[dbo].[STK]


  SELECT TOP (1000) [ID]
      ,[IslemTur]
      ,[EvrakNo]
      ,[Tarih]
      ,[MalKodu]
      ,[Miktar]
      ,[Fiyat]
      ,[Tutar]
      ,[Birim]
  FROM [Test].[dbo].[STI]

------------------------------------------------------------------------

EXEC GetStockMovements @Malkodu = '10081 SİEMENS', @BaslangicTarihi = '42736', @BitisTarihi = '42775';

------------------------------------------------------------------------

DECLARE @TotalPages INT; 

EXEC GetPagedStockMovements 
    @MalKodu = '10081 SİEMENS', 
    @BaslangicTarihi = 42736, 
    @BitisTarihi = 42775, 
    @PageNumber = 1, 
    @PageSize = 38,
    @TotalPages = @TotalPages OUTPUT; 

PRINT 'Total Pages: ' + CAST(@TotalPages AS NVARCHAR(10));