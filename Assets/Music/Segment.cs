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
        sequence = (sequence + 1) % 16;
    }

    public bool PlayBeat(int beat) => ((1 << beat) & sequence) != 0;
}
