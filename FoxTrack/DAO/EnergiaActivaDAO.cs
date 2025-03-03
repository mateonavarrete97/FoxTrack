using Npgsql;
using System;

public class EnergiaActivaDAO
{
    private string connectionString;

    public EnergiaActivaDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public decimal GetCantidadEnergiaActiva()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryEA = "SELECT SUM(value) AS EA FROM consumption;";
            using (var cmdEA = new NpgsqlCommand(queryEA, connection))
            {
                var ea = cmdEA.ExecuteScalar();
                return Convert.ToDecimal(ea);
            }
        }
    }

    public decimal GetTarifaCU()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryCU = "SELECT CU FROM tariffs;";
            using (var cmdCU = new NpgsqlCommand(queryCU, connection))
            {
                var cu = cmdCU.ExecuteScalar();
                return Convert.ToDecimal(cu);
            }
        }
    }
}
