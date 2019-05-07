using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    [SerializeField] public int instrument;
    [SerializeField] public int sequence;

    public Segment(int _instrument, int _sequence)
    {
        instrument = _instrument;
        sequence = _sequence;
    }

    public bool PlayBeat(int beat)
    {
        return (beat & sequence) != 0;
    }
}
