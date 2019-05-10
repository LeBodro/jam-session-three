using UnityEngine;

public class PopUpMaker : SceneSingleton<PopUpMaker>
{
    [SerializeField] TransactionPopUp transactionPrefab = null;
    [SerializeField] float transactionVerticalOffet = 0.5f;

    Pool<TransactionPopUp> transactions;
    Vector3 transactionOffset;

    void Start()
    {
        transactions = new Pool<TransactionPopUp>(() => Instantiate(transactionPrefab, transform));
        transactionOffset = Vector3.up * transactionVerticalOffet;
    }

    public static void ShowTransaction(Vector3 position, decimal amount) => Instance._ShowTransaction(position, amount);
    void _ShowTransaction(Vector3 position, decimal amount)
    {
        var popUp = transactions.Get();
        popUp.transform.SetAsLastSibling();
        popUp.Show(position + transactionOffset, amount);
    }
}
