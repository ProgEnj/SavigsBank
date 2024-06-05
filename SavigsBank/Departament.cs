using MySql.Data.MySqlClient;


namespace SavigsBank;

public class Departament
{
    private string _departmentName;
    public string DepartmentName => _departmentName;
    private double _interestRatePerMonth; 
    public double InterestRatePerMonth => _interestRatePerMonth;
    private MySqlConnection _DBConnection;
    
    private List<Account> accounts;

    public Departament(string departmentName, double interestRatePerMonth, string connectionStr)
    {
        _departmentName = departmentName;
        _interestRatePerMonth = interestRatePerMonth;
        accounts = new List<Account>();
        _DBConnection = EstablishDBConnection(connectionStr);
        // add timer
    }

    ~Departament()
    {
        _DBConnection.Close();
    }
    

    private MySqlConnection EstablishDBConnection(string connStr)
    {
        var conn = new MySqlConnection(connStr);
        try
        {
            conn.Open();
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }

        return conn;
    }

    private MySqlDataReader dbQuery(string sqlQuery)
    {
        try
        {
            var cmd = new MySqlCommand(sqlQuery, _DBConnection);
            var rdr = cmd.ExecuteReader();
            return rdr;
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void DisplayTable(string table)
    {
        var rdr = dbQuery($"select * from {table}");
        while (rdr.Read())
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                Console.Write($"rdr[i], ");
            }
            Console.WriteLine();
        }
    }
    
    public void OpenAccount(string ownerName, string ownerSurname, string ownerMiddleName)
    {
        var random = new Random();
        int id = random.Next(10000, 99999);
        
        accounts.Add(new Account(id, ownerName, ownerSurname, ownerMiddleName));

        dbQuery("insert into accounts" +
                $"values(\"{id}\", \"{ownerName}\", \"{ownerSurname}\", \"{ownerMiddleName}\", 0);");
    }
}