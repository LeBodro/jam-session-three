using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBox : MonoBehaviour
{
    decimal displayDelta;
    decimal displayedBalance;
    [SerializeField] decimal speed = 0.25m;

    [SerializeField] Text display = null;

    void Awake()
    {
        PrioritizedStartQueue.Queue(200, () =>
        {
            displayedBalance = Bank.GetBalance();
            Display();
        });
    }

    // Update is called once per frame
    void Update()
    {
        var currentBalance = Bank.GetBalance();
        if (displayedBalance == currentBalance)
        {
            return;
        }
        displayDelta = currentBalance * (decimal)Time.deltaTime * speed;
        displayedBalance += displayDelta;
        // Temporary for now, since we just show the full part of the decimal value
        displayedBalance = Math.Ceiling(displayedBalance);

        if (displayedBalance > currentBalance)
        {
            displayedBalance = currentBalance;
        }

        Display();
    }

    void Display()
    {
        // TODO: Format bank value for real display
        display.text = displayedBalance.ToString("0.00");
    }
}
