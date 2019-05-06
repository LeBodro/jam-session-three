using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : SceneSingleton<Bank>
{
    [SerializeField] float initialBalance = 10;

    decimal _balance;

    event System.Action<decimal, decimal> _onTransaction = delegate { };
    public static event System.Action<decimal, decimal> OnTransaction
    {
        add { Instance._onTransaction += value; }
        remove { Instance._onTransaction -= value; }
    }

    decimal Balance
    {
        get => _balance;
        set
        {
            decimal difference = value - _balance;
            _balance = Math.Max(value, 0);
            _onTransaction(difference, _balance);
        }
    }

    public override void Awake()
    {
        base.Awake();
        PrioritizedStartQueue.Queue(100, () =>
        {
            _balance = (decimal)initialBalance;
        });
    }

    /// <summary>
    /// Gets the current balance from the singleton.
    /// </summary>
    public static decimal GetBalance() => Instance.Balance;

    /// <summary>
    /// Sets the current balance on the singleton.
    /// </summary>
    public static decimal SetBalance(decimal value) => Instance.Balance = value;

    /// <summary>
    /// Deposits money to the bank account.
    /// </summary>
    /// <param name="amount">Amount of money to deposit.</param>
    public static void Deposit(decimal amount)
    {
        Instance.Balance += amount;
    }

    /// <summary>
    /// Withdraws money from the bank account. Cannot put the account below 0.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public static void Withdraw(decimal amount)
    {
        Instance.Balance -= amount;
    }

    /// <summary>
    /// Only withdraws money from the bank account if specified amount does not exceed balance.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public static bool TryWithdraw(decimal amount)
    {
        return Instance._TryWithdraw(amount);
    }

    bool _TryWithdraw(decimal amount)
    {
        if (amount > Balance)
        {
            return false;
        }
        Balance -= amount;
        return true;
    }
}
