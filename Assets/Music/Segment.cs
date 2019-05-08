public class Segment
{
    public int instrument;
    public int sequence;

    public Segment(int _instrument, int _sequence)
    {
        instrument = _instrument;
        sequence = _sequence;
    }

    public void CycleSequence()
    {
        sequence = (sequence + 1) % 17;
    }

    public bool PlayBeat(int beat) => ((beat + 1) & sequence) != 0;
}
