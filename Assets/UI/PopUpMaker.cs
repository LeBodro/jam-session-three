using UnityEngine;

public class PopUpMaker : SceneSingleton<PopUpMaker>
{
    [SerializeField] TransactionPopUp transactionPrefab = null;
    [SerializeField] NotePopUp notePrefab = null;
    [SerializeField] float transactionVerticalOffet = 0.5f;

    Pool<PopUp<decimal>> transactions;
    Pool<PopUp<int>> notes;
    Vector3 transactionOffset;

    void Start()
    {
        notes = new Pool<PopUp<int>>(() => Instantiate(notePrefab, transform));
        transactions = new Pool<PopUp<decimal>>(() => Instantiate(transactionPrefab, transform));
        transactionOffset = Vector3.up * transactionVerticalOffet;
    }

    public static void ShowTransaction(Vector3 position, decimal amount) => Instance._ShowTransaction(position, amount);
    void _ShowTransaction(Vector3 position, decimal amount)
    {
        var popUp = transactions.Get();
        popUp.transform.SetAsLastSibling();
        popUp.Show(position + transactionOffset, amount);
    }

    public static void ShowNote(Vector3 position, int tone) => Instance._ShowNote(position, tone);
    void _ShowNote(Vector3 position, int tone)
    {
        var popUp = notes.Get();
        popUp.transform.SetAsLastSibling();
        popUp.Show(position, tone);
    }
}
