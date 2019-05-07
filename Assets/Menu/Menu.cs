using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button inputBlocker;
    [SerializeField] Button toggle;
    [SerializeField] Button resume;
    [SerializeField] Button save;
    [SerializeField] Button saveAndQuit;
    [SerializeField] Button destroyProgress;

    bool available;

    void Start()
    {
        inputBlocker.onClick.AddListener(Hide);
        toggle.onClick.AddListener(Toggle);
        resume.onClick.AddListener(Hide);
        save.onClick.AddListener(Save);
        saveAndQuit.onClick.AddListener(SaveAndQuit);
        destroyProgress.onClick.AddListener(DestroyProgress);
    }

    void Toggle()
    {
        if (available) Hide();
        else Show();
    }

    void Show()
    {
        available = true;
        inputBlocker.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (!available) return;
        available = false;
        inputBlocker.gameObject.SetActive(false);
    }

    void Save()
    {
        if (!available) return;
        SaveGame.Save();
        Hide();
    }

    void SaveAndQuit()
    {
        if (!available) return;
        SaveGame.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void DestroyProgress()
    {
        if (!available) return;
        throw new System.NotImplementedException();
        // TODO: implement chance to opt out of file destruction when pressing this button
    }
}
