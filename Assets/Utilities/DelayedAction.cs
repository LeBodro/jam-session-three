using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAction : RAIISceneSingleton<DelayedAction>
{
    public static void Invoke(System.Action action, float delay) => Instance._Invoke(action, delay);
    void _Invoke(System.Action action, float delay)
    {
        StartCoroutine(InvokeCoroutine(action, delay));
    }

    IEnumerator InvokeCoroutine(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static void NextFrame(System.Action action) => Instance._NextFrame(action);
    void _NextFrame(System.Action action)
    {
        StartCoroutine(NextFrameCoroutine(action));
    }

    IEnumerator NextFrameCoroutine(System.Action action)
    {
        yield return null;
        action();
    }
}
