using UnityEngine;
using UnityEngine.UI;

public class PriceTag : MonoBehaviour
{
    const decimal THOUSAND = 1000;
    const decimal MILLION = THOUSAND * THOUSAND;
    const decimal BILLION = MILLION * THOUSAND;
    const decimal TRILLION = BILLION * THOUSAND;
    const decimal QUADRILLION = TRILLION * THOUSAND;
    const decimal QUINTILLION = QUADRILLION * THOUSAND;
    const decimal SEXTILLION = QUINTILLION * THOUSAND;

    [SerializeField] Text price;

    public void DisplayPrice(decimal value)
    {
        if (value < 100m)
            price.text = value.ToString("0.00");
        else if (value < THOUSAND)
            price.text = value.ToString("0");
        else if (value < MILLION)
            price.text = (value / THOUSAND).ToString("0") + "k";
        else if (value < BILLION)
            price.text = (value / MILLION).ToString("0") + "M";
        else if (value < TRILLION)
            price.text = (value / BILLION).ToString("0") + "B";
        else if (value < QUADRILLION)
            price.text = (value / TRILLION).ToString("0") + "T";
        else if (value < QUINTILLION)
            price.text = (value / QUADRILLION).ToString("0") + "qd";
        else if (value < SEXTILLION)
            price.text = (value / QUINTILLION).ToString("0") + "Qn";
        else
            price.text = (value / SEXTILLION).ToString("0") + "sx";

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
