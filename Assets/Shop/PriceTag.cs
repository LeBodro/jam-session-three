using UnityEngine;
using UnityEngine.UI;

public class PriceTag : MonoBehaviour
{
    [SerializeField] Text price;

    public void DisplayPrice(decimal value)
    {
        price.text = CurrencyHelper.Beautify(value);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
