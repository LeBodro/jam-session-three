using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

public class SaveGame : RAIISingleton<SaveGame>
{
    IList<SavedComponent> registry = new List<SavedComponent>();

    public static void Register(SavedComponent c) => Instance.registry.Add(c);
    public static void Unregister(SavedComponent c) => Instance.registry.Remove(c);

    public static void Save() => Instance._Save();
    void _Save()
    {
        StringBuilder content = new StringBuilder();
        foreach (SavedComponent c in registry)
        {
            content.Append(c.Serialize());
        }
    }

    /*
    IDictionary<string, int> data;

    SaveGame(string file)
    {
        data = new Dictionary<string, int>();

        string path = Application.persistentDataPath + "/" + file;
        if (File.Exists(path))
        {
            using (FileStream fs = File.OpenRead(path))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    int value = reader.ReadInt32();
                    data[key] = value;
                }
            }
        }
    }

    public static SaveGame Load(string file)
    {
        return new SaveGame(file);
    }

    public void Save(string file)
    {
        using (FileStream fs = File.OpenWrite(Application.persistentDataPath + "/" + file))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            writer.Write(data.Count);
            foreach (var pair in data)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
    }

    public bool ReadBool(string key, bool def = default(bool))
    {
        if (data.ContainsKey(key))
        {
            return data[key] != 0;
        }
        return def;
    }

    public void WriteBool(string key, bool value)
    {
        data[key] = value ? 1 : 0;
    }

    public int ReadInt(string key, int def = default(int))
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        return def;
    }

    public void WriteInt(string key, int value)
    {
        data[key] = value;
    }

    public float ReadFloat(string key, float def = default(float))
    {
        if (data.ContainsKey(key))
        {
            int value = data[key];
            BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }
        return def;
    }

    public void WriteFloat(string key, float value)
    {
        data[key] = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
    }
    */
}
