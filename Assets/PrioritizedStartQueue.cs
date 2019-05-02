using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrioritizedStartQueue : SceneSingleton<PrioritizedStartQueue>
{
    const string ERROR_MESSAGE = "PrioritizedStartQueue.{0}() has been called after the Start queue was processed.";

    Dictionary<System.Action, int> queue = new Dictionary<System.Action, int>();
    bool isDone = false;

    public static void Queue(int priority, System.Action action) => Instance._Queue(priority, action);
    void _Queue(int priority, System.Action action)
    {
        if (isDone) Debug.LogErrorFormat(ERROR_MESSAGE, "Queue");
        queue.Add(action, priority);
    }

    public static void Dequeue(System.Action action) => Instance._Dequeue(action);
    void _Dequeue(System.Action action)
    {
        if (isDone) Debug.LogWarningFormat(ERROR_MESSAGE, "Dequeue");
        queue.Remove(action);
    }

    public static void Move(int newPriority, System.Action action) => Instance._Move(newPriority, action);
    void _Move(int newPriority, System.Action action)
    {
        if (isDone) Debug.LogErrorFormat(ERROR_MESSAGE, "Move");
        if (queue.ContainsKey(action)) queue[action] = newPriority;
        else Debug.LogError("PrioritizedStartQueue.Move() cannot be called for a non-queued action.");
    }

    void Start()
    {
        foreach (var pair in Instance.queue.OrderBy(k => k.Value))
        {
            System.Action action = pair.Key;

            action();
        }
        isDone = true;
    }
}
