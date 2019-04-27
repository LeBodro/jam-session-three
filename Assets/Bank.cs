using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    event System.Action<float, float> _onTransaction = delegate { };
    public event System.Action<float, float> OnTransaction
    {
        add { _onTransaction += value; }
        remove { _onTransaction -= value; }
    }

    [SerializeField]
    float _balance;
    public float Balance
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
            _onTransaction(difference, _balance);
        }
    }

    /// <summary>
    /// Deposits money to the bank account.
    /// </summary>
    /// <param name="amount">Amount of money to deposit.</param>
    public void Deposit(float amount)
    {
        Balance += amount;
    }

    /// <summary>
    /// Withdraws money from the bank account. Cannot put the account below 0.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public void Withdraw(float amount)
    {
        Balance -= amount;
    }

    /// <summary>
    /// Only withdraws money from the bank account if specified amount does not exceed balance.
    /// </summary>
    /// <param name="amount">Amount of money to withdraw.</param>
    public bool TryWithdraw(float amount)
    {
        if (amount >= Balance)
        {
            return false;
        }
        Balance -= amount;
        return true;
    }
}
