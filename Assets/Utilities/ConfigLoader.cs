#define USE_CONFIG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* ############## WARNING ##############
 * THIS SINGLETON IS TO BE USED AS A WAY
 * TO LOAD CONFIGS. A CONFIG SHOULD BE
 * USED A SINGLE PLACE, NOT MORE. THIS
 * SINGLETON IS FOR DEVELOPMENT PURPOSE
 * ONLY AND SHOULD NOT BE NECESSARY IN
 * THE FINAL BUILD EXCEPT AS A CANDY FOR
 * POWER USERS.
 * #####################################
 */
using System.IO;

public class ConfigLoader : RAIISingleton<ConfigLoader>
{

#if USE_CONFIG
    const string fileName = "config.txt";
    IDictionary<string, string> mConfigs = new Dictionary<string, string>();

    ConfigLoader()
    {
        LoadText();
    }

    void LoadText()
    {
        string fullPath = string.Format("{0}/{1}", Application.streamingAssetsPath, fileName);
        if (File.Exists(fullPath))
        {
            using (StreamReader content = File.OpenText(fullPath))
            {
                bool hasError = false;
                while (content.Peek() >= 0)
                {
                    string[] pair = content.ReadLine().Split(new System.Char[] { '=' });
                    if (pair.Length == 2)
                    {
                        mConfigs.Add(pair[0], pair[1]);
                    }
                    else
                    {
                        Debug.LogWarning("Ignoring invalid config line. Should be formatted as \"KEY=VALUE\"");
                        hasError = true;
                    }
                }
                if (hasError)
                    Debug.LogWarningFormat("Config file loaded with errors: {0}", fullPath);
                else
                    Debug.LogFormat("Config was successfully loaded: {0}", fullPath);
            }
        }
        else
        {
            Debug.LogError(string.Format("Config file not found: {0}. All values will be set to default.", fullPath));
        }
    }

    public static void Get(string key, ref string output)
    {
        string value = Instance.GetImpl(key);
        if (string.IsNullOrEmpty(value))
            KeyError(key);
        else
            output = value;
    }

    public static void Get(string key, ref int output)
    {
        string value = Instance.GetImpl(key);
        int result;
        if (int.TryParse(value, out result))
            output = result;
        else
            KeyError(key);
    }

    public static void Get(string key, ref float output)
    {
        string value = Instance.GetImpl(key);
        float result;
        if (float.TryParse(value, out result))
            output = result;
        else
            KeyError(key);
    }

    public static void Get(string key, ref bool output)
    {
        string value = Instance.GetImpl(key);
        if (string.IsNullOrEmpty(value))
            KeyError(key);
        else
            output = value.ToLower() == "true" || value == "1";
    }

    public static void Get(string key, GameObject toSetActive)
    {
        bool isActive = toSetActive.activeSelf;
        ConfigLoader.Get(key, ref isActive);
        toSetActive.SetActive(isActive);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        string value = Instance.GetImpl(key);
        if (!string.IsNullOrEmpty(value))
            return value.ToLower() == "true" || value == "1";

        KeyError(key);
        return defaultValue;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        string value = Instance.GetImpl(key);
        if (!string.IsNullOrEmpty(value))
            return value;

        KeyError(key);
        return defaultValue;
    }

    static void KeyError(string key)
    {
        Debug.LogErrorFormat("Key \"{0}\" could not not be read.", key);
    }

    string GetImpl(string key)
    {
        if (mConfigs.ContainsKey(key))
        {
            return mConfigs[key];
        }
        else
        {
            return string.Empty;

        }
    }
#else
	public ConfigLoader () {}
	public static void Get (string key, ref float output) {}
	public static void Get (string key, ref string output) {}
	public static void Get (string key, ref int output) {}
	public static void Get (string key, ref bool output) {}
	public static void Get (string key, GameObject toSetActive) {}
#endif

}
