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
    
    private void 

    public void DisplayTable(string table)
    {
        var rdr = dbQuery($"select * from {table}");
        while (rdr.Read())
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                Console.Write($"{rdr[i]}, ");
            }
            Console.WriteLine();
        }
        rdr.Close();
    }
    
    public void OpenAccount(string ownerName, string ownerSurname, string ownerMiddleName)
    {
        var random = new Random();
        int id = random.Next(10000, 99999);
        
        accounts.Add(new Account(id, ownerName, ownerSurname, ownerMiddleName));
        string query = "insert into accounts" +
            $" values(\"{id}\", \"{ownerName}\", \"{ownerSurname}\", \"{ownerMiddleName}\", 0, null)";
        var rdr = dbQuery(query);
        rdr.Close();
    }

    public void OpenDeposit(int accountID, int term, int amount)
    {
        var rnd = new Random();
        int id = rnd.Next(10000, 99999);
        double interest = _interestRatePerMonth * term;
        var dep = new Deposit(id, interest, DateTime.Today, DateTime.Today.AddMonths(term));
        accounts.Find(x => x.ID == accountID).AssignDeposit(dep);
        var rdr = this.dbQuery("insert into deposits" +
                     $" values(id, interest, \"{DateTime.Today.ToString("d")}\", \"{DateTime.Today.AddMonths(term).ToString("d")}\")");
        rdr.Close();
    }
}