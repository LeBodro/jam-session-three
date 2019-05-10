using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp<T> : Poolable<PopUp<T>>
{
    [SerializeField] Text display = null;
    [SerializeField] float delay = 1;
    [SerializeField] float speed = 0.13f;

    protected virtual string Interpret(T value) => value.ToString();

    public void Show(Vector3 position, T value)
    {
        display.text = Interpret(value);
        transform.position = position;
        RePool(delay);
        display.CrossFadeAlpha(0, delay, false);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    protected override void OnRePool()
    {
        base.OnRePool();
        display.canvasRenderer.SetAlpha(1);
    }
}
