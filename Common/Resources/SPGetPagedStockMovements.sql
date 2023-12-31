CREATE PROCEDURE [dbo].[GetPagedStockMovements]
(
    @MalKodu NVARCHAR(50),
    @BaslangicTarihi INT,
    @BitisTarihi INT,
    @PageNumber INT, -- Aktif sayfa numarası
    @PageSize INT, -- Sayfa başına düşen veri miktarı
	@TotalPages INT OUTPUT -- Toplam sayfa sayısını döndürecek çıkış parametresi
)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT; -- Sayfa numarasına göre atlanacak veri sayısı.
    SET @Offset = (@PageNumber - 1) * @PageSize; -- Offset hesaplama

    -- Toplam veri sayısını hesapla
    DECLARE @TotalItemCount INT;
    SELECT @TotalItemCount = COUNT(*)
    FROM dbo.STI
    WHERE STI.MalKodu = @MalKodu AND STI.Tarih BETWEEN @BaslangicTarihi AND @BitisTarihi;

    -- Toplam sayfa sayısını hesapla
    SET @TotalPages = CEILING(CONVERT(FLOAT, @TotalItemCount) / @PageSize);

    -- Verilerin sayfalanarak alımı
    SELECT
        ROW_NUMBER() OVER (ORDER BY STI.Tarih ASC) AS SiraNo, --Her bir satır için otomatik sayısal değer atanır. EF için sp tanımlamada KEY görevi görür.
        CASE -- İşlem türüne göre ilgili sütun için değer belirtir.
            WHEN STI.IslemTur = 0 THEN 'Giriş'
            WHEN STI.IslemTur = 1 THEN 'Çıkış'
            ELSE ''
        END AS IslemTur,
        STI.EvrakNo,
        CONVERT(VARCHAR(15), CAST(STI.Tarih - 2 AS DATETIME), 104) AS Tarih, -- Int tarih değeri, tarih formatına çevrilerek gösterilir.
        CASE WHEN STI.IslemTur = 0 THEN STI.Miktar ELSE 0 END AS GirisMiktar, -- İşlem türüne göre ilgili sütuna değerler tanımlar.
        CASE WHEN STI.IslemTur = 1 THEN STI.Miktar ELSE 0 END AS CikisMiktar, -- İşlem türüne göre ilgili sütuna değerler tanımlar.
        CAST(0 AS DECIMAL(18, 2)) AS Stok -- Stok hesabı uygulamada yapılacağı için ve decimal olarak 0 gönderilmezse modeldeki prop'a uymadığı için hata çıkartır. Bu sebeple CAST işlemi yapıldı. Fakat sayfalamalı alımlarda stok miktarı doğru hesaplanamayacağı için farklı bir çözüm gerekli.
    FROM dbo.STI
    WHERE STI.MalKodu = @MalKodu AND STI.Tarih BETWEEN @BaslangicTarihi AND @BitisTarihi --Verilen koşullara uygun veriler üzerinden işlem yapmak için kullanılır.
    ORDER BY STI.Tarih ASC -- Alınan verilerin sıralaması eskiden yeniye tarih baz alınarak yapılmasını sağlar.
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; -- Sayfalama için atlanacak veri sayısı ve atlandıktan sonra alınacak veri sayısını belirtir.
END;