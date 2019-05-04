using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable<T> : MonoBehaviour where T : Poolable<T>
{
    Pool<T> container;

    public T DePool(Pool<T> container)
    {
        this.container = container;
        OnDePool();
        return this as T;
    }

    protected virtual void OnDePool()
    {
        gameObject.SetActive(true);
    }

    public void RePool(float delay) => DelayedAction.Invoke(RePool, delay);
    public void RePool()
    {
        OnRePool();
        container.Put(this as T);
    }

    protected virtual void OnRePool()
    {
        gameObject.SetActive(false);
    }
}
