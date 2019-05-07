using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    [SerializeField] public int note;
    [SerializeField] public int instrument;
    [SerializeField] public int sequence;

    public Segment(int _note, int _instrument, int _sequence)
    {
        note = _note;
        instrument = _instrument;
        sequence = _sequence;
    }

    public bool PlayBeat(int beat)
    {
        return (beat & sequence) != 0;
    }
}
