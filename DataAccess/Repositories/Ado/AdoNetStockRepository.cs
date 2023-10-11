using Common;
using Data.Models;
using DataAccess.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess.Repositories.Ado
{
    public class AdoNetStockRepository : IStockRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public AdoNetStockRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<List<StockMovement>> GetStockMovements(string stockCode, int startDate, int endDate)
        {
            List<StockMovement> result = new List<StockMovement>();

            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.DefaultConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetStockMovements", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MalKodu", stockCode);
                    command.Parameters.AddWithValue("@BaslangicTarihi", startDate);
                    command.Parameters.AddWithValue("@BitisTarihi", endDate);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StockMovement stockMovement = new StockMovement
                            {
                                SiraNo = (long)reader["SiraNo"],
                                IslemTur = reader["IslemTur"].ToString() ?? "",
                                EvrakNo = reader["EvrakNo"].ToString() ?? "",
                                Tarih = reader["Tarih"].ToString() ?? "",
                                GirisMiktar = (decimal)reader["GirisMiktar"],
                                CikisMiktar = (decimal)reader["CikisMiktar"],
                                Stok = (decimal)reader["Stok"]
                            };

                            result.Add(stockMovement);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<PagedDataModel<StockMovement>> GetPagedStockMovements(string stockCode, int startDate, int endDate, int pageNumber, int pageSize)
        {
            PagedDataModel<StockMovement> result = new PagedDataModel<StockMovement>();
            result.DataList = new List<StockMovement>();

            using (SqlConnection connection = new SqlConnection(_connectionStringProvider.DefaultConnectionString))
            {
                connection.Open();

                try
                {
                    using (SqlCommand command = new SqlCommand("dbo.GetPagedStockMovements", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MalKodu", stockCode);
                        command.Parameters.AddWithValue("@BaslangicTarihi", startDate);
                        command.Parameters.AddWithValue("@BitisTarihi", endDate);
                        command.Parameters.AddWithValue("@PageNumber", pageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        // Output parameter for total pages
                        SqlParameter totalPagesParam = new SqlParameter("@TotalPages", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(totalPagesParam);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                StockMovement stockMovement = new StockMovement
                                {
                                    SiraNo = (long)reader["SiraNo"],
                                    IslemTur = reader["IslemTur"].ToString() ?? "",
                                    EvrakNo = reader["EvrakNo"].ToString() ?? "",
                                    Tarih = reader["Tarih"].ToString() ?? "",
                                    GirisMiktar = (decimal)reader["GirisMiktar"],
                                    CikisMiktar = (decimal)reader["CikisMiktar"],
                                    Stok = (decimal)reader["Stok"]
                                };

                                result.DataList.Add(stockMovement);
                            }
                        }

                        result.PageSize = pageSize;
                        result.PageNumber = pageNumber;
                        result.TotalPage = (int)totalPagesParam.Value;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here and update the result with an error message.
                }
            }

            return result;
        }
    }
}
