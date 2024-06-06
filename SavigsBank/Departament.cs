using MySql.Data.MySqlClient;


namespace SavigsBank;

public class Departament
{
    private string _departamentName;
    public string DepartamentName => _departamentName;
    private double _interestRatePerMonth; 
    public double InterestRatePerMonth => _interestRatePerMonth;
    private MySqlConnection _DBConnection;
    
    private List<Account> accounts;

    public Departament(string departamentName, double interestRatePerMonth, string connectionStr)
    {
        _departamentName = departamentName;
        _interestRatePerMonth = interestRatePerMonth;
        accounts = new List<Account>();
        _DBConnection = EstablishDBConnection(connectionStr);
        this.LoadFromDB();
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

    private MySqlDataReader DBQuery(string sqlQuery)
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

    private void LoadFromDB()
    {
        try
        {
            var depositIDs = new List<int>();
            var rdr = DBQuery("select account_id, first_name, last_name, middle_name, balance, accounts.deposit_id, interest, opened, ending " +
                              "from accounts left join deposits " +
                              "on accounts.deposit_id = deposits.deposit_id");
            while (rdr.Read())
            {
                var account = new Account(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4));
                accounts.Add(account);
                if (!rdr.IsDBNull(5))
                {
                    account.AssignDeposit(new Deposit(rdr.GetInt32(5), 
                        rdr.GetDouble(6), rdr.GetDateTime(7), rdr.GetDateTime(8)));
                }
            }
            rdr.Close();

        }
        catch (MySqlException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void PrintAccounts()
    {
        foreach (var item in accounts)
        {
            Console.WriteLine($"{item.ID}, {item.OwnerName}, {item.OwnerSurname}, " +
                              $"{item.OwnerMiddleName}, {item.Balance}");
            if (item.Deposit != null)
            {
                var dep = item.Deposit;
                Console.WriteLine("\t" + $"{dep.ID}, {dep.Opened.Date}, {dep.Ending.Date}");
            }
        }
    }

    public void OpenAccount(string ownerName, string ownerSurname, string ownerMiddleName)
    {
        var random = new Random();
        int id = random.Next(10000, 99999);
        
        accounts.Add(new Account(id, ownerName, ownerSurname, ownerMiddleName, 0));
        string query = "insert into accounts " +
            $"values(\"{id}\", \"{ownerName}\", \"{ownerSurname}\", \"{ownerMiddleName}\", 0, null)";
        var rdr = DBQuery(query);
        rdr.Close();
    }

    public void OpenDeposit(int accountID, int term, int amount)
    {
        var rnd = new Random();
        int id = rnd.Next(10000, 99999);
        double interest = _interestRatePerMonth * term;
        var dep = new Deposit(id, interest, DateTime.Today, DateTime.Today.AddMonths(term));
        accounts.Find(x => x.ID == accountID).AssignDeposit(dep);
        var rdr = this.DBQuery("insert into deposits " +
                     $"values({id}, {interest}, \"{DateTime.Today.ToString("o")}\", \"{DateTime.Today.AddMonths(term).ToString("o")}\")");
        rdr.Close();

        rdr = this.DBQuery($"update accounts set deposit_id = {id} where account_id = {accountID}");
        rdr.Close();
    }
}