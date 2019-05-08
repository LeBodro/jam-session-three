using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Synthetizer : SceneSingleton<Synthetizer>
{
    [SerializeField] float volume = 1;
    [SerializeField] float tempo = 160;
    [SerializeField] int totalMeasures = 8;
    [SerializeField] int beatsPerMeasure = 4;
    [SerializeField] int numberOfTracks = 2;
    [SerializeField] int totalNotes = 16;
    [SerializeField] AudioSource speaker;
    [SerializeField] Instrument[] instruments = null;

    float secondsPerBeat;
    int trackLength;
    int[] notes;
    Segment[] segments;

    // Used to keep track of changing beats between updates
    int lastBeatRaw = 0;

    void Reset()
    {
        speaker = GetComponent<AudioSource>();
    }

    public static void CycleNote(Vector3Int cellCoordinates)
    {
        var noteIndex = ToIndex(cellCoordinates);
        Instance.notes[noteIndex] = (Instance.notes[noteIndex] + 1) % Instance.totalNotes;
    }

    public static void CycleSegment(Vector3Int cellCoordinates)
    {
        Instance.segments[ToIndex(cellCoordinates)]?.CycleSequence();
    }

    public static void RegisterSegment(Vector3Int cellCoordinates, Segment s)
    {
        Instance.segments[ToIndex(cellCoordinates)] = s;
    }

    public static void RemoveSegment(Vector3Int cellCoordinates)
    {
        Instance.segments[ToIndex(cellCoordinates)] = null;
    }

    private static int ToIndex(Vector3Int cellCoordinates)
    {
        // Y is reversed compared to how grid works. Top to bottom instead of bottom to top.
        return cellCoordinates.x + (Instance.trackLength - 1 - cellCoordinates.y) * Instance.trackLength;
    }

    private static int GetIndexForTrackAndMeasure(int track, int measure)
    {
        return Mathf.FloorToInt(measure / Instance.beatsPerMeasure) * Instance.beatsPerMeasure 
        + track * Instance.beatsPerMeasure
        + measure;
    }

    void Start()
    {
        speaker.volume = volume;
        secondsPerBeat = 1 / (tempo / 60f);
        segments = new Segment[totalMeasures * numberOfTracks];
        notes = new int[totalMeasures * numberOfTracks];
        trackLength = totalMeasures / numberOfTracks;
    }

    void FixedUpdate()
    {
        // Counts the current beat and always goes up as time flows
        var currentBeatRaw = Mathf.FloorToInt(Time.time / secondsPerBeat);
        // Counts the current measure and oscillates between 0 and totalMeasures-1
        var currentMeasure = Mathf.FloorToInt(currentBeatRaw / beatsPerMeasure) % totalMeasures;
        // Counts the current beat in the current measure. Oscillates between 0 and beatsPerMeasure-1
        var currentBeatInMeasure = currentBeatRaw % beatsPerMeasure;

        if (currentBeatRaw != lastBeatRaw)
        {
            // When beat changes. Should trigger something in current segment at current measure.

            for (int track = 0; track < numberOfTracks; track++)
            {
                int indexForTrackAndMeasure = GetIndexForTrackAndMeasure(track, currentMeasure);
                Segment s = segments[indexForTrackAndMeasure];
                if (s != null && s.PlayBeat(currentBeatInMeasure))
                {
                    speaker.PlayOneShot(instruments[s.instrument].notes[Instance.notes[indexForTrackAndMeasure]]);
                }
            }
        }
        lastBeatRaw = currentBeatRaw;
    }

    // TODO: This should return data containing volume, tempo and note per cell.
    public void Serialize()
    {
    }

    // TODO: This should take synthetizer data to initialize variables.
    //       "Start" should not be called. initialization should happen here.
    public void Deserialize()
    {
    }
}
