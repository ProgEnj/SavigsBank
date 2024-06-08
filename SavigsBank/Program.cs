using System.Diagnostics;
using MySql.Data.MySqlClient;
using SavigsBank;

string connStr = "server=localhost;user=root;database=savings_bank;port=3306;password=1234";

Console.WriteLine("Enter departament name, interest rate: ");
string name = Console.ReadLine();
double rate = Double.Parse(Console.ReadLine());

var departament = new Departament("depart1", 0.2, connStr);

bool exit = true;
while (exit)
{
    Console.WriteLine("[1] Mange account and deposits " +
                      "[2] Print accounts " +
                      "[3] Print deposits " +
                      "[4] Manage funds " +
                      "[5] Reports " +
                      "[6] Exit");
    int option = int.Parse(Console.ReadLine());
    if (option == 1)
    {
        Console.WriteLine(
                    "[1] Open account " +
                    "[2] Close account " +
                    "[3] Open deposit " +
                    "[4] Close deposit " +
                    "[5] Change account info");
        int accountOption = int.Parse(Console.ReadLine());
        if (accountOption == 1)
        {
            Console.WriteLine("Enter owner name, surname, middlename: ");
            string n = Console.ReadLine();
            string s = Console.ReadLine();
            string m = Console.ReadLine();
            departament.OpenAccount(n, s, m);
        }
        else if (accountOption == 2)
        {
            Console.WriteLine("Enter account ID: ");
            int id = int.Parse(Console.ReadLine());
            departament.CloseAccount(id);
        }
        else if (accountOption == 3)
        {
            Console.WriteLine("Enter account ID, term");
            int id = int.Parse(Console.ReadLine());
            int term = int.Parse(Console.ReadLine());
            departament.OpenDeposit(id, term);
        }
        else if (accountOption == 4)
        {
            Console.WriteLine("Enter account ID");
            int id = int.Parse(Console.ReadLine());
            departament.CloseDeposit(id);
        }
        else if (accountOption == 5)
        {
            Console.WriteLine(
                "[1] Name " +
                "[2] Surname " +
                "[3] Middlename");
            int changeOption = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter account ID");
            
            int accountID = int.Parse(Console.ReadLine());

            if (changeOption == 1)
            {
                Console.WriteLine("Enter new name");
                
                string newName = Console.ReadLine();
                departament.ChangeName(accountID, newName);
            }
            else if (changeOption == 2)
            {
                Console.WriteLine("Enter new surname");
                
                string newName = Console.ReadLine();
                departament.ChangeSurname(accountID, newName);
            }
            else if (changeOption == 3)
            {
                Console.WriteLine("Enter new middlename");
                
                string newName = Console.ReadLine();
                departament.ChangeMidName(accountID, newName);
            }
        }
    }
    else if (option == 2)
    {
        departament.PrintAccounts();
    }
    else if (option == 3)
    {
        departament.PrintDeposits();
    }
    else if (option == 4)
    {
        Console.WriteLine(
            "[1] Add funds to account " +
            "[2] Withdraw funds from account " +
            "[3] Transfer funds ");
        
        int fundsOption = int.Parse(Console.ReadLine());
        if (fundsOption == 1)
        {
            Console.WriteLine("Enter account ID, amount");
            int ID = int.Parse(Console.ReadLine());
            double amount = int.Parse(Console.ReadLine());
            departament.AddFunds(ID, amount);
        }
        else if (fundsOption == 2)
        {
            Console.WriteLine("Enter account ID, amount");
            int ID = int.Parse(Console.ReadLine());
            double amount = int.Parse(Console.ReadLine());
            departament.WithdrawFunds(ID, amount);
        }
        else if (fundsOption == 3)
        {
            Console.WriteLine("Enter account ID from, account ID to, amount");
            int fromID = int.Parse(Console.ReadLine());
            int toID = int.Parse(Console.ReadLine());
            double amount = int.Parse(Console.ReadLine());
            departament.TransferFunds(fromID, toID, amount);
        }
        
    }
    else if (option == 5)
    {

        Console.WriteLine(
            "[1] Print account report " +
            "[2] Print account balance report");
        int reportOption = int.Parse(Console.ReadLine());
        if (reportOption == 1)
        {
            Console.WriteLine("Enter account ID, date from, date to");
            int ID = int.Parse(Console.ReadLine());
            DateOnly from = DateOnly.Parse(Console.ReadLine());
            DateOnly to = DateOnly.Parse(Console.ReadLine());
            
            departament.AccountReport(ID, from, to);
        }
        else if (reportOption == 2)
        {
            Console.WriteLine("Enter account ID, amount");
            int ID = int.Parse(Console.ReadLine());
            double amount = double.Parse(Console.ReadLine());
            
            departament.AccountBalanceReport(ID, amount);
        }
    }
    else if (option == 6)
    {
        Console.WriteLine("Exiting..");
        exit = false;
    }
}