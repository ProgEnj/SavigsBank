namespace SavigsBank;

public abstract class AccountBase
{
    private int _ID;
    public int ID => _ID;
    private string _ownerName;
    public string OwnerName
    {
        get => _ownerName;
        set => _ownerName = value;
    }
    private string _ownerSurname;
    public string OwnerSurname
    {
        get => _ownerSurname;
        set => _ownerSurname = value;
    }
    private string _ownerMiddleName;
    public string OwnerMiddleName
    {
        get => _ownerMiddleName;
        set => _ownerMiddleName = value;
    }
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