namespace SavigsBank;


public class Account
{
    private int _ID;
    public int ID => _ID;
    private string _ownerName; 
    public string OwnerName => _ownerName;
    private string _ownerSurname;
    public string OwnerSurname => _ownerName;
    private string _ownerMiddleName; 
    public string OwnerMiddleName => _ownerMiddleName;
    private double _balance;
    public double Balance => _balance;
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