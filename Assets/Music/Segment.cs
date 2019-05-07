using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    [SerializeField] int note;
    [SerializeField] int instrument;
    [SerializeField] int sequence;

    public void PlayBeat(int beat)
    {
        Debug.Log("I was played! " + beat);
    }
}
