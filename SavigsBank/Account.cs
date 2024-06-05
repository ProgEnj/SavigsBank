namespace SavigsBank;

public class Account
{
    private int _ID;
    public int ID => _ID;
    private string _ownerName; 
    public string OwnerName => _ownerName;
    private string _ownerSurname;
    private string OwnerSurname => _ownerName;
    private string _ownerMiddleName; 
    private string OwnerMiddleName => _ownerMiddleName;
    private int _balance;
    public int Balance => _balance;
    private Deposit? _deposit;
    public Deposit? Deposit => _deposit;

    public Account(int id, string ownerName, string ownerSurname, string ownerMiddleName, int balance)
    {
        _ID = id;
        _ownerName = ownerName;
        _ownerSurname = ownerSurname;
        _ownerMiddleName = ownerMiddleName;
        _balance = balance;
        _deposit = null;
    }

    public void AssignDeposit(Deposit dep)
    {
        this._deposit = dep;
    }
    
    
    
}