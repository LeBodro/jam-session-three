using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderedQueuer : SceneSingleton<OrderedQueuer>
{
    Dictionary<System.Action, int> queue = new Dictionary<System.Action, int>();
    public static void Queue(int priority, System.Action action)
    {
        Instance.queue.Add(action, priority);
    }

    public static void Dequeue(System.Action action)
    {
        Instance.queue.Remove(action);
    }

    public static void Move(int newPriority, System.Action action)
    {
        Dequeue(action);
        Queue(newPriority, action);
    }

    void Start()
    {
        foreach (var pair in Instance.queue.OrderBy(k => k.Value))
        {
            System.Action action = pair.Key;

            action();
        }
    }
}
