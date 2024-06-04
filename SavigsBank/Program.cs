using MySql.Data.MySqlClient;

string connStr = "server=localhost;user=root;database=test;port=3306;password=1234";

var conn = new MySqlConnection(connStr);
try
{
    Console.WriteLine("Connecting to DB...");
    
    conn.Open();

    string sql = "insert into abobas (first_name, last_name, hire_date)" +
                 "values('biba', 'boba', '2024-01-01');";

    string sql1 = "select * from abobas;";

    var cmd = new MySqlCommand(sql1, conn);
    var rdr = cmd.ExecuteReader();

    while (rdr.Read())
    {
        for(int i = 0; i < rdr.FieldCount; i++)
        {
            Console.Write($"{rdr[i]}, ");
        }

        Console.WriteLine();
    }
    rdr.Close();
}
catch (MySqlException e)
{
    Console.WriteLine(e + connStr);
    throw;
}

conn.Close();
Console.WriteLine("Connection closed");