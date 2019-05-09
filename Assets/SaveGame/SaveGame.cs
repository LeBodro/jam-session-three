using UnityEngine;
using System.IO;

public class SaveData
{
    [SerializeField] public string bankBalance;
    [SerializeField] public SnappingGridData[] grids;
    [SerializeField] public SynthetizerData synth;

    public SaveData(string _bankBalance, SnappingGridData[] _grids, SynthetizerData _synth)
    {
        bankBalance = _bankBalance;
        grids = _grids;
        synth = _synth;
    }
}

public class SaveGame : SceneSingleton<SaveGame>
{
    const string SAVE_FILE_NAME = "save_01.txt";
    const string DEFAULT_JSON = @"{""bankBalance"":""10.00"",""grids"":[{""modules"":[]},{""modules"":[{""index"":0,""prefab"":0,""tier"":0,""bought"":false,""powered"":false,""direction"":0,""segment"":{""instrument"":0,""sequence"":0}},{""index"":1,""prefab"":1,""tier"":0,""bought"":false,""powered"":false,""direction"":0,""segment"":{""instrument"":0,""sequence"":0}},{""index"":2,""prefab"":2,""tier"":0,""bought"":false,""powered"":false,""direction"":0,""segment"":{""instrument"":0,""sequence"":0}}]},{""modules"":[]}],""synth"":{""volume"":1,""tempo"":400,""notes"":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]}}";

    [SerializeField] SnappingGrid[] grids = null;
    [SerializeField] float autosaveDelay = 300;

    float timeLeftToAutosave;

    void Start() => Load();

    void Load()
    {
        timeLeftToAutosave = autosaveDelay;
        string json = LoadJsonFromFile();
        if (string.IsNullOrEmpty(json))
            json = DEFAULT_JSON;

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Bank.SetBalance(decimal.Parse(data.bankBalance));
        for (int i = 0; i < grids.Length; i++)
            grids[i].Deserialize(data.grids[i]);
        
        Synthetizer.Deserialize(data.synth);
    }

    string LoadJsonFromFile()
    {
        string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        if (!File.Exists(path)) return string.Empty;
        string json = string.Empty;
        using (var stream = File.OpenText(path))
        {
            json = stream.ReadToEnd();
        }
        return json;
    }

    public static void Save() => Instance._Save();
    void _Save()
    {
        timeLeftToAutosave = autosaveDelay;
        SnappingGridData[] data = new SnappingGridData[grids.Length];
        for (int i = 0; i < grids.Length; i++)
            data[i] = grids[i].Serialize();

        string bankBalance = Bank.GetBalance().ToString();
        SaveData save = new SaveData(bankBalance, data, Synthetizer.Serialize());

        string json = JsonUtility.ToJson(save);
        using (var stream = File.CreateText(Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME)))
        {
            Debug.Log(Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME));
            stream.Write(json);
        }
    }

    public static void DeleteSave() => Instance._DeleteSave();
    void _DeleteSave()
    {
        string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        if (!File.Exists(path)) return;
        string newPath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME + ".backup");
        File.Move(path, newPath);
    }

    void Update()
    {
        timeLeftToAutosave -= Time.deltaTime;
        if (timeLeftToAutosave <= 0)
            _Save();
    }
}
