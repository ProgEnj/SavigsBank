using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Crypto.Signers;

namespace SavigsBank;

public class Account
{
    public int ID { get; set; }
    public string OwnerName { get; set; }
    public string OwnerSurname { get; set; }
    public string OwnerMiddleName { get; set; }
    public int Balance { get; }
    private Deposit? deposit { get; set; }
    
    public Account(int id, string ownerName, string ownerSurname, string ownerMiddleName, int balance)
    {
        this.ID = id;
        OwnerName = ownerName;
        OwnerSurname = ownerSurname;
        OwnerMiddleName = ownerMiddleName;
        Balance = balance;
        deposit = null;
    }

    public void AssignDeposit(Deposit dep)
    {
        this.deposit = dep;
    }
}