CREATE PROCEDURE GetStockMovements
(
    @MalKodu NVARCHAR(50),
    @BaslangicTarihi INT,
    @BitisTarihi INT
)
AS
BEGIN
    SET NOCOUNT ON; --Etkilenen satır sayısı bilgisi gerekmediği için bu şekilde kullanıldı.
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
		CAST(0 AS DECIMAL(18, 2)) AS Stok --Stok hesabı uygulamada yapılıyor ve decimal olarak 0 gönderilmezse modeldeki prop'a uymadığı için hata çıkartıyor. Bu sebeple CAST işlemi yapıldı.
    FROM dbo.STI
    WHERE STI.MalKodu = @Malkodu AND STI.Tarih BETWEEN @BaslangicTarihi AND @BitisTarihi --Verilen koşullara uygun veriler üzerinden işlem yapmak için kullanılır.
    ORDER BY STI.Tarih ASC; -- Alınan verilerin sıralaması eskiden yeniye tarih baz alınarak yapılmasını sağlar.
END;