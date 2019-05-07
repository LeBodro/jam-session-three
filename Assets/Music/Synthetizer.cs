using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthetizer : SceneSingleton<Synthetizer>
{
    [SerializeField] float volume;
    [SerializeField] float tempo;
    [SerializeField] int totalMeasures = 8;
    [SerializeField] int beatsPerMeasure = 4;
    [SerializeField] int numberOfTracks = 2;
    float secondsPerBeat;
    int trackLength;
    Segment[] segments;

    // Used to keep track of changing beats between updates
    int lastBeatRaw = 0;

    public static void RegisterSegment(int cellIndex, Segment s)
    {
        Instance.segments[cellIndex] = s;
    }

    public static void RemoveSegment(int cellIndex)
    {
        Instance.segments[cellIndex] = null;
    }

    private static int ToIndex(Vector3Int cellCoordinates)
    {
        // Y is reversed compared to how grid works. Top to bottom instead of bottom to top.
        return cellCoordinates.x + (Instance.trackLength - 1 - cellCoordinates.y) * Instance.trackLength;
    }

    void Start()
    {
        Instance.secondsPerBeat = 1 / (Instance.tempo / 60f);
        Instance.segments = new Segment[Instance.totalMeasures * Instance.numberOfTracks];
        Instance.trackLength = Instance.totalMeasures / Instance.numberOfTracks;
    }

    void Update()
    {
        // Counts the current beat and always goes up as time flows
        var currentBeatRaw = Mathf.FloorToInt(Time.time / Instance.secondsPerBeat);
        // Counts the current measure and oscillates between 0 and totalMeasures-1
        var currentMeasure = Mathf.FloorToInt(currentBeatRaw / Instance.beatsPerMeasure) % Instance.totalMeasures;
        // Counts the current beat in the current measure. Oscillates between 0 and beatsPerMeasure-1
        var currentBeatInMeasure = currentBeatRaw % Instance.beatsPerMeasure;

        if (currentBeatRaw != Instance.lastBeatRaw)
        {
            // When beat changes. Should trigger something in current segment at current measure.
            Debug.Log(string.Format("Measure: {0} Beat: {1}", currentMeasure, currentBeatInMeasure));

            for (int track = 0; track < Instance.numberOfTracks; track++)
            {
                Segment s = Instance.segments[currentMeasure + (track * totalMeasures)];
                s?.PlayBeat(currentBeatInMeasure);
            }
        }
        Instance.lastBeatRaw = currentBeatRaw;
    }

    public void Serialize()
    {
        // TODO: This should return data containing volume, tempo and note per cell.
    }
}
