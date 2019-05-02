using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : SceneSingleton<Bank>
{
    event System.Action<decimal, decimal> _onTransaction = delegate { };
    public event System.Action<decimal, decimal> OnTransaction
    {
        add { _onTransaction += value; }
        remove { _onTransaction -= value; }
    }

    [SerializeField]
    float initialBalance;
    decimal _balance;
    private decimal Balance
    {
        get => _balance;
        set
        {
            decimal difference = value - _balance;
            if (value > 0)
            {
                _balance = value;
            }
            else
            {
                _balance = 0;
            }
            //Debug.LogFormat("Bank balance is now {0} ClickCoins.", value);
            _onTransaction(difference, _balance);
        }
    }

    public override void Awake()
    {
        base.Awake();
        OrderedQueuer.Queue(100, () => {
            _balance = (decimal)initialBalance;
        });
    }

    /// <summary>
    /// Gets the current balance from the singleton
    /// </summary>
    public static decimal GetBalance()
    {
        return Instance.Balance;
    }

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
