using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : SceneSingleton<Bank>
{
    event System.Action<float, float> _onTransaction = delegate { };
    public event System.Action<float, float> OnTransaction
    {
        add { _onTransaction += value; }
        remove { _onTransaction -= value; }
    }

    [SerializeField]
    float _balance;
    private float Balance
    {
        get => _balance;
        set
        {
            float difference = value - _balance;
            if (value > 0)
            {
                _balance = value;
            }
            else
            {
                _balance = 0;
            }
            Debug.LogFormat("Bank balance is now {0} ClickCoins.", value);
            _onTransaction(difference, _balance);
        }
    }

    /// <summary>
    /// Gets the current balance from the singleton
    /// </summary>
    public static float GetBalance()
    {
        return Instance.Balance;
    }

    /// <summary>
    /// Deposits money to the bank account.
    /// </summary>
    /// <param name="amount">Amount of money to deposit.</param>
    public static void Deposit(float amount)
    {
        Instance.Balance += amount;
    }

    /// <summary>
    /// Withdraws money from the bank account. Cannot put the account below 0.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public static void Withdraw(float amount)
    {
        Instance.Balance -= amount;
    }

    /// <summary>
    /// Only withdraws money from the bank account if specified amount does not exceed balance.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public static bool TryWithdraw(float amount)
    {
        return Instance._TryWithdraw(amount);
    }

    bool _TryWithdraw(float amount)
    {
        if (amount > Balance)
        {
            return false;
        }
        Balance -= amount;
        return true;
    }
}
