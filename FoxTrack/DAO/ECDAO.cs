using Npgsql;
using System;

public class ECDAO
{
    private string connectionString;

    public ECDAO(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public decimal GetCantidadEC()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryEC = "SELECT SUM(value) AS EC FROM injection;";
            using (var cmdEC = new NpgsqlCommand(queryEC, connection))
            {
                var ec = cmdEC.ExecuteScalar();
                return Convert.ToDecimal(ec);
            }
        }
    }

    public decimal GetTarifaC()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string queryC = "SELECT C FROM tariffs;";
            using (var cmdC = new NpgsqlCommand(queryC, connection))
            {
                var c = cmdC.ExecuteScalar();
                return Convert.ToDecimal(c);
            }
        }
    }
}

