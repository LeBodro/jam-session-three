using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    const float INVERT_TRANSITION_DELAY = 5;

    [SerializeField] Button inputBlocker = null;
    [SerializeField] Button toggle = null;
    [SerializeField] Button resume = null;
    [SerializeField] Button save = null;
    [SerializeField] Button saveAndQuit = null;
    [SerializeField] Button destroyProgress = null;
    [SerializeField] Button confirmDestruction = null;
    [SerializeField] GameObject secondChance = null;
    [SerializeField] new AudioSource audio = null;

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
        destroyProgress.onClick.AddListener(ShowDestructionPrompt);
        confirmDestruction.onClick.AddListener(DestroyProgress);
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
        secondChance.SetActive(false);
    }

    void Hide()
    {
        if (!available) return;
        audio.Play();
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

    void ShowDestructionPrompt()
    {
        audio.Play();
        secondChance.SetActive(true);
    }

    void DestroyProgress()
    {
        if (!available) return;
        SaveGame.DeleteSave();
        SceneManager.LoadScene(0);
    }
}
