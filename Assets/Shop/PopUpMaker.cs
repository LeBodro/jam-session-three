using UnityEngine;

public class PopUpMaker : SceneSingleton<PopUpMaker>
{
    [SerializeField] TransactionPopUp transactionPrefab;

    Pool<TransactionPopUp> transactions;

    void Start()
    {
        transactions = new Pool<TransactionPopUp>(() => Instantiate(transactionPrefab, transform));
    }

    public static void ShowTransaction(Vector3 position, decimal amount) => Instance._ShowTransaction(position, amount);
    void _ShowTransaction(Vector3 position, decimal amount)
    {
        var popUp = transactions.Get();
        popUp.transform.SetAsLastSibling();
        popUp.Show(position, amount);
    }
}
