namespace SavigsBank;

public abstract class AccountBase
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
    public double Balance
    {
        get => _balance;
        set => _balance = value;
    }
    
    protected AccountBase(int id, string ownerName, string ownerSurname, string ownerMiddleName, double balance)
    {
        _ID = id;
        _ownerName = ownerName;
        _ownerSurname = ownerSurname;
        _ownerMiddleName = ownerMiddleName;
        _balance = balance;
    }
}