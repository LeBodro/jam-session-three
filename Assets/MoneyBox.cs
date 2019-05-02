using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBox : MonoBehaviour
{
    decimal velocity;
    decimal currentShownBalance;
    [SerializeField] decimal speed = 0.25m;

    [SerializeField] Text display = null;

    void Awake()
    {
        OrderedQueuer.Queue(200, () => {
            currentShownBalance = Bank.GetBalance();
            Display();
        });
    }

    // Update is called once per frame
    void Update()
    {
        var currentBalance = Bank.GetBalance();
        if (currentShownBalance == currentBalance)
        {
            return;
        }
        velocity = currentBalance * (decimal)Time.deltaTime * speed;
        currentShownBalance += velocity;
        // Temporary for now, since we just show the full part of the decimal value
        currentShownBalance = Math.Ceiling(currentShownBalance);

        if (currentShownBalance > currentBalance)
        {
            currentShownBalance = currentBalance;
        }

        Display();
    }

    void Display()
    {
        // TODO: Format bank value for real display
        display.text = currentShownBalance.ToString("0");
    }
}
