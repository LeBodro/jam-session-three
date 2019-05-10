using UnityEngine;
using UnityEngine.UI;

public class TransactionPopUp : PopUp<decimal>
{
    protected override string Interpret(decimal amount) => CurrencyHelper.Beautify(amount);
}
