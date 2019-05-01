using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] AudioSource sound = null;
    [SerializeField] SpriteRenderer down = null;
    [SerializeField] SpriteRenderer up = null;

    bool pressIntent;

    public event System.Action OnPress = delegate { };

    public void OnPointerDown(PointerEventData eventData)
    {
        SetDown(true);
        pressIntent = true;
        sound.Play();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetDown(false);
        if (pressIntent) OnPress();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pressIntent = false;
        SetDown(false);
    }

    void SetDown(bool value)
    {
        down.enabled = value;
        up.enabled = !value;
    }
}
