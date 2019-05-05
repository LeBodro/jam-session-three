using UnityEngine;
using UnityEngine.UI;

public class TransactionPopUp : Poolable<TransactionPopUp>
{
    [SerializeField] Text amountDisplay = null;
    [SerializeField] float delay = 1;
    [SerializeField] float speed = 0.13f;

    public void Show(Vector3 position, decimal amount)
    {
        amountDisplay.text = CurrencyHelper.Beautify(amount);
        transform.position = position;
        RePool(delay);
        amountDisplay.CrossFadeAlpha(0, delay, false);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    protected override void OnRePool()
    {
        base.OnRePool();
        amountDisplay.canvasRenderer.SetAlpha(1);
    }
}
