using Npgsql;
using System;

public class EE1DAO
{
    private string connectionString;

    public EE1DAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public decimal GetCantidadEE1()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryEE1 = @"
                SELECT 
                    CASE 
                        WHEN (SELECT SUM(value) FROM injection) <= (SELECT SUM(value) FROM consumption) THEN (SELECT SUM(value) FROM injection)
                        ELSE (SELECT SUM(value) FROM consumption)
                    END AS EE1;";
            using (var cmdEE1 = new NpgsqlCommand(queryEE1, connection))
            {
                var ee1 = cmdEE1.ExecuteScalar();
                return Convert.ToDecimal(ee1);
            }
        }
    }

    public decimal GetNegCU()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryNegCU = "SELECT -CU AS NegCU FROM tariffs;";
            using (var cmdNegCU = new NpgsqlCommand(queryNegCU, connection))
            {
                var negCU = cmdNegCU.ExecuteScalar();
                return Convert.ToDecimal(negCU);
            }
        }
    }
}

