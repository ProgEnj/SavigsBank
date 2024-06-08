﻿using MySql.Data.MySqlClient;

namespace SavigsBank;


public class Departament
{
    private string _departamentName;
    public string DepartamentName => _departamentName;
    private double _interestRatePerMonth; 
    public double InterestRatePerMonth => _interestRatePerMonth;
    private MySqlConnection _DBConnection;

    private Timer timer;
    
    private List<AccountBasic> accounts;

    public Departament(string departamentName, double interestRatePerMonth, string connectionStr)
    {
        _departamentName = departamentName;
        _interestRatePerMonth = interestRatePerMonth;
        accounts = new List<AccountBasic>();
        _DBConnection = EstablishDBConnection(connectionStr);
        this.LoadFromDB();

        timer = new Timer(this.CheckDeposits, null, 100000, 86_400_000);
    }

    ~Departament()
    {
        timer.Dispose();
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
                var account = new AccountBasic(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4));
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
    
    private void LogAction(int accountID, string operationType, double balance_affection, string description)
    {
        var random = new Random();
        int id = random.Next(10000, 99999);
        var rdr = DBQuery("insert into actions_history " +
                          $"values({id}, {accountID}, \"{operationType}\", {balance_affection}, " +
                          $"\"{DateTime.Now.ToString("o")}\", \"{description}\");");
        rdr.Close();
    }

    public void CheckDeposits(object? stateInfo)
    {
        var today = DateTime.Today;
        foreach (var item in accounts)
        {
            var dep = item.Deposit;
            if (item.Deposit != null)
            {
                if (today.CompareTo(dep.Ending) == 0)
                {
                    this.CloseDeposit(item);
                }
                else if (today.CompareTo(dep.Ending) < 0 &&
                    dep.Ending.Subtract(today).Days % 30 == 0)
                {
                    double balanceAdd = Math.Round(item.Balance * dep.Interest, 2);
                    item.Balance += balanceAdd;
                    try
                    {
                        var rdr = DBQuery($"update accounts set balance = balance + {balanceAdd} " +
                                          $"where account_id = {item.ID};");
                        rdr.Close();

                        LogAction(item.ID, "Accrual", balanceAdd,  "Accrual of interest");
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }

    public void PrintAccounts()
    {
        // Construct header
        char separator = '|';
        char horizontalSeparator = '-';
        int maxLength = 15;
        string[] header = new[] { "Id", "FirstName", "MiddleName", "LastName", "Balance", "Deposit" };
        
        string rowSeparator = "";
        for (int i = 0; i < ((maxLength + 3) * 6) + 5; i++)
        {
            rowSeparator += horizontalSeparator;
        }
        
        Console.Write("  n  ");

        for (int i = 0; i < header.Length; i++)
        {
            Console.Write($" {header[i]}");
            for (int j = 0; j < (maxLength - header[i].Length); j++)
            {
                Console.Write(" ");
            }
            Console.Write("  ");
        }
        Console.WriteLine();
        Console.WriteLine(rowSeparator);

        int numbering = 1;
        foreach (var item in accounts)
        {
            Console.Write(separator + " " + numbering + " " + separator);
            var list = item.GetAsStringList();
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write($" {list[i]}");
                for (int j = 0; j < maxLength - list[i].Length; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(" " + separator);
            }
            Console.Write("\n" + rowSeparator + "\n");
            numbering++;
        }
        Console.WriteLine();
        Console.WriteLine();
    }
    
    public void PrintDeposits()
    {
        // Construct header
        char separator = '|';
        char horizontalSeparator = '-';
        int maxLength = 15;
        string[] header = new[] { "Id", "Interest", "Opened", "Ending"};
        
        string rowSeparator = "";
        for (int i = 0; i < ((maxLength + 3) * 6) + 5; i++)
        {
            rowSeparator += horizontalSeparator;
        }
        
        Console.Write("  n  ");

        for (int i = 0; i < header.Length; i++)
        {
            Console.Write($" {header[i]}");
            for (int j = 0; j < (maxLength - header[i].Length); j++)
            {
                Console.Write(" ");
            }
            Console.Write("  ");
        }
        Console.WriteLine();
        Console.WriteLine(rowSeparator);

        int numbering = 1;
        foreach (var item in accounts)
        {
            if (item.Deposit != null)
            {
                Console.Write(separator + " " + numbering + " " + separator);
                var list = item.GetAsStringList();
                for (int i = 0; i < list.Count; i++)
                {
                    Console.Write($" {list[i]}");
                    for (int j = 0; j < maxLength - list[i].Length; j++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" " + separator);
                }
                Console.Write("\n" + rowSeparator + "\n");
                numbering++;
            }
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    public void OpenAccount(string ownerName, string ownerSurname, string ownerMiddleName)
    {
        var random = new Random();
        int id = random.Next(10000, 99999);
        
        accounts.Add(new AccountBasic(id, ownerName, ownerSurname, ownerMiddleName, 0));
        string query = "insert into accounts " +
            $"values(\"{id}\", \"{ownerName}\", \"{ownerSurname}\", \"{ownerMiddleName}\", 0, null)";
        var rdr = DBQuery(query);
        rdr.Close();
        
        this.LogAction(id, "Account open", 0, "Opened a new account");
    }
    
    public void CloseAccount(AccountBasic account)
    {
        var rdr = this.DBQuery($"delete from accounts where account_id = {account.ID};");
        rdr.Close();
        if (account.Deposit != null)
        {
            rdr = this.DBQuery($"delete from deposits where deposit_id = {account.Deposit.ID};");
            account.Deposit = null;
        }
        accounts.Remove(account);
        
        this.LogAction(account.ID, "Account close", 0, "Closed account");
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
        
        this.LogAction(accountID, "Deposit open", 0, "Opened a new deposit");
    }

    public void CloseDeposit(AccountBasic account)
    {
        var rdr = this.DBQuery($"update accounts set deposit_id = null where account_id = {account.ID}; " +
                     $"delete from deposits where deposit_id = {account.Deposit.ID};");
        account.Deposit = null;
        rdr.Close();
        
        this.LogAction(account.ID, "Deposit close", 0, "Closed account deposit");
    }

    public void AddFunds(int accountID, double amount)
    {
        var account = accounts.Find(x => x.ID == accountID);
        account.Balance += Math.Round(amount, 2);
        
        var rdr = this.DBQuery($"update accounts set balance = balance + {Math.Round(amount, 2)} " +
                               $"where account_id = {accountID}");
        rdr.Close();

        this.LogAction(accountID, "Add funds", amount, "added funds to account");
    }
    
    public void WithdrawFunds(int accountID, double amount)
    {
        var account = accounts.Find(x => x.ID == accountID);
        account.Balance -= Math.Round(amount, 2);
        
        var rdr = this.DBQuery($"update accounts set balance = balance - {Math.Round(amount, 2)} " +
                               $"where account_id = {accountID}");
        rdr.Close();

        this.LogAction(accountID, "Add funds", (Math.Round(amount, 2)) * (-1), "withdraw funds from account");
    }

    public void TransferFunds(int accountIDFrom, int accountIDTo, double amount)
    {
        var roundedAmount = Math.Round(amount, 2);
        
        var accountFrom = accounts.Find(x => x.ID == accountIDFrom);
        var accountTo = accounts.Find(x => x.ID == accountIDTo);

        accountFrom.Balance -= Math.Round(roundedAmount, 2);
        accountTo.Balance += Math.Round(roundedAmount, 2);

        var rdr = this.DBQuery($"update accounts set balance = balance - {roundedAmount} " +
                               $"where account_id = {accountIDFrom}; " +
                               $"update accounts set balance = balance + {roundedAmount} " +
                               $"where account_id = {accountTo};");
        
        this.LogAction(accountIDFrom, "Transfer funds", Math.Round(roundedAmount, 2) * (-1), "Transfered funds to another account");
        this.LogAction(accountIDTo, "Transfer funds", Math.Round(roundedAmount, 2), "Transfered funds from another account");
    }
    
}