using UnityEngine;
using System.IO;

public class SaveData
{
    [SerializeField] public string bankBalance;
    [SerializeField] public SnappingGridData[] grids;

    public SaveData(string _bankBalance, SnappingGridData[] _grids)
    {
        bankBalance = _bankBalance;
        grids = _grids;
    }
}

public class SaveGame : SceneSingleton<SaveGame>
{
    [SerializeField] SnappingGrid[] grids;

    void Start()
    {
        Load();
    }

    void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "save_01.txt");
        string json = string.Empty;
        using (var stream = File.OpenText(path))
        {
            json = stream.ReadToEnd();
        }
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        PrioritizedStartQueue.Queue(200, () =>
        {
            Bank.SetBalance(decimal.Parse(data.bankBalance));
        });
        for (int i = 0; i < grids.Length; i++)
            grids[i].Deserialize(data.grids[i]);
    }

    public static void Save() => Instance._Save();
    void _Save()
    {
        SnappingGridData[] data = new SnappingGridData[grids.Length];
        for (int i = 0; i < grids.Length; i++)
            data[i] = grids[i].Serialize();

        string bankBalance = Bank.GetBalance().ToString();
        SaveData save = new SaveData(bankBalance, data);

        string json = JsonUtility.ToJson(save);
        using (var stream = File.CreateText(Path.Combine(Application.persistentDataPath, "save_01.txt")))
        {
            Debug.Log(Path.Combine(Application.persistentDataPath, "save_01.txt"));
            stream.Write(json);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Save();
    }
}
