﻿namespace SavigsBank;


public class AccountBasic : AccountBase
{
    private Deposit? _deposit;

    public Deposit? Deposit
    {
        get => _deposit;
        set => _deposit = value;
    }

    public AccountBasic(int id, string ownerName, string ownerSurname, 
        string ownerMiddleName, double balance) : base(id, ownerName, ownerSurname, ownerMiddleName, balance)
    {
        _deposit = null;
    }

    public void AssignDeposit(Deposit dep)
    {
        this._deposit = dep;
    }
}