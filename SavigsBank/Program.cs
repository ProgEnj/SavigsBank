using MySql.Data.MySqlClient;
using SavigsBank;

string connStr = "server=localhost;user=root;database=savings_bank;port=3306;password=1234";

var a = new Departament("aboba", 1.2, connStr);

a.OpenAccount("Ander", "Serpov", "Ivanov");

a.PrintAccounts();


