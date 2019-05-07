using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    const float INVERT_TRANSITION_DELAY = 5;

    [SerializeField] Button inputBlocker;
    [SerializeField] Button toggle;
    [SerializeField] Button resume;
    [SerializeField] Button save;
    [SerializeField] Button saveAndQuit;
    [SerializeField] Button destroyProgress;

    bool available;
    bool isTranslating;
    Vector3 openedPosition;
    Vector3 closedPosition;
    float transitionTime;

    void Start()
    {
        inputBlocker.onClick.AddListener(Hide);
        toggle.onClick.AddListener(Toggle);
        resume.onClick.AddListener(Hide);
        save.onClick.AddListener(Save);
        saveAndQuit.onClick.AddListener(SaveAndQuit);
        destroyProgress.onClick.AddListener(DestroyProgress);
        openedPosition = transform.localPosition;
        closedPosition = openedPosition + Vector3.down * ((RectTransform)transform).sizeDelta.y;
        transform.localPosition = closedPosition;
    }

    void Update()
    {
        if (!isTranslating) return;
        float deltaTime = Time.deltaTime * INVERT_TRANSITION_DELAY;
        if (available)
            transitionTime = Mathf.Min(transitionTime + deltaTime, 1);
        else
            transitionTime = Mathf.Max(transitionTime - deltaTime, 0);

        transitionTime = Mathf.Clamp01(transitionTime);

        transform.localPosition = Vector3.Lerp(closedPosition, openedPosition, transitionTime);
        isTranslating = available ? transitionTime < 1 : transitionTime > 0;
    }

    void StartTransition()
    {
        isTranslating = true;
    }

    void Toggle()
    {
        if (available) Hide();
        else Show();
    }

    void Show()
    {
        available = true;
        StartTransition();
        inputBlocker.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (!available) return;
        available = false;
        StartTransition();
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
