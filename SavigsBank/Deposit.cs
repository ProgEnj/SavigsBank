namespace SavigsBank;

public class Deposit
{
    private int _ID;
    public int ID => _ID;
    
    private double _interest;
    public double Interest => _interest;
    
    private DateTime _opened;
    public DateTime Opened => _opened;
    
    private DateTime _ending;
    public DateTime Ending => _ending;
    
    public Deposit(int id, double interest, DateTime opened, DateTime ending)
    {
        _ID = id;
        _interest = interest;
        _opened = opened;
        _ending = ending;
    }

    public List<string> GetAsStringList()
    {
        return new List<string>() {ID.ToString(), _interest.ToString(), 
            _opened.Date.ToString("o"),  _ending.Date.ToString("o")};
    }
}