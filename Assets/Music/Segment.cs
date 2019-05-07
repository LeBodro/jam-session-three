using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    [SerializeField] int note;
    [SerializeField] int instrument;
    [SerializeField] int sequence;

    public Segment(int _note, int _instrument, int _sequence)
    {
        note = _note;
        instrument = _instrument;
        sequence = _sequence;
    }

    public void PlayBeat(int beat)
    {
        Debug.Log("I was played! " + beat);
    }
}
