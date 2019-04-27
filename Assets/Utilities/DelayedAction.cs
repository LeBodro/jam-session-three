using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAction : MonoBehaviour
{
    float delay;
    System.Action action;

    public static void Invoke(System.Action action, float delay)
    {
        var holder = new GameObject();
        var invoker = holder.AddComponent<DelayedAction>();
        invoker.delay = delay;
        invoker.action = action;
        invoker.StartCoroutine(invoker.WaitTimeImpl());
    }

    public static void NextFrame(System.Action action)
    {
        var holder = new GameObject();
        var invoker = holder.AddComponent<DelayedAction>();
        invoker.action = action;
        invoker.StartCoroutine(invoker.NextFrameImpl());
    }

    IEnumerator WaitTimeImpl()
    {
        yield return new WaitForSeconds(delay);
        action();
        Destroy(gameObject);
    }

    IEnumerator NextFrameImpl()
    {
        yield return null;
        action();
        Destroy(gameObject);
    }
}
