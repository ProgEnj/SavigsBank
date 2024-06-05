using Google.Protobuf;
using MySql.Data.MySqlClient;


namespace SavigsBank;

public class Department
{
    private string _departmentName;
    public string DepartmentName => _departmentName;
    private double _interestRatePerMonth; 
    public double InterestRatePerMonth => _interestRatePerMonth;
    private MySqlConnection _DBConnection;
    
    private List<Account> accounts;

    public Department(string departmentName, double interestRatePerMonth, string connectionStr)
    {
        _departmentName = departmentName;
        _interestRatePerMonth = interestRatePerMonth;
        accounts = new List<Account>();
        _DBConnection = EstablishDBConnection(connectionStr);
        // add timer
    }

    ~Department()
    {
        _DBConnection.Close();
    }

    public void OpenAccount(int id, string ownerName, string ownerSurname, string ownerMiddleName, int balance)
    {
        accounts.Add(new Account(id, ownerName, ownerSurname,
            ownerMiddleName, balance));
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
    
    
    
    
    
    
}