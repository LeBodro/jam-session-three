using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBox : MonoBehaviour
{
    float velocity;
    int currentShownBalance;
    [SerializeField] float speed = 0.25f;

    [SerializeField] Text display;

    // Update is called once per frame
    void Update()
    {
        var currentBalance = Bank.GetBalance();
        if (currentShownBalance == currentBalance)
        {
            return;
        }
        velocity = currentBalance * Time.deltaTime * speed;
        currentShownBalance += Mathf.CeilToInt(velocity);

        if (currentShownBalance > currentBalance)
        {
            currentShownBalance = (int)currentBalance;
        }

        // TODO: Format bank value for real display
        display.text = Mathf.CeilToInt(currentShownBalance).ToString();
    }
}
