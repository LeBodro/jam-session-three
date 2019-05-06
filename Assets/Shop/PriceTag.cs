using System;
using UnityEngine;
using UnityEngine.UI;

public class PriceTag : MonoBehaviour
{
    [SerializeField] Color availablePriceMask = new Color(57 / 255f, 185 / 255f, 113 / 255f);
    [SerializeField] Color unavailableColorMask = Color.white;
    [SerializeField] Text price;
    [SerializeField] Image background;
    private decimal value;

    void Start()
    {
        Bank.OnTransaction += UpdatePriceTagAvailability;
    }

    private void UpdatePriceTagAvailability(decimal difference, decimal _balance)
    {
        background.color = this.value <= _balance
            ? availablePriceMask
            : unavailableColorMask;
    }

    public void DisplayPrice(decimal value)
    {
        price.text = CurrencyHelper.Beautify(value);
        gameObject.SetActive(true);
        this.value = value;
        UpdatePriceTagAvailability(0, Bank.GetBalance());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
