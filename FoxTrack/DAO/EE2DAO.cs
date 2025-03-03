using Npgsql;

public class EE2DAO
{
    private readonly string _connectionString;

    public EE2DAO(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<double> GetSumInjectionAsync()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand("SELECT SUM(value) FROM injection", connection);
            var result = await command.ExecuteScalarAsync();
            return result != null ? Convert.ToDouble(result) : 0;
        }
    }

    public async Task<double> GetSumConsumptionAsync()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand("SELECT SUM(value) FROM consumption", connection);
            var result = await command.ExecuteScalarAsync();
            return result != null ? Convert.ToDouble(result) : 0;
        }
    }

    public async Task<double> GetHourlyRateAsync(DateTime timestamp)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new NpgsqlCommand("SELECT value FROM xm_data_hourly_per_agent WHERE record_timestamp = @timestamp", connection);
            command.Parameters.AddWithValue("timestamp", timestamp);
            var result = await command.ExecuteScalarAsync();
            return result != null ? Convert.ToDouble(result) : 0;
        }
    }
}

