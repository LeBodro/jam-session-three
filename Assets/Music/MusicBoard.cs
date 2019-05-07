using UnityEngine;

public class MusicBoard : SnappingGrid
{
    protected override void OnSnap(Module snapped, Vector3Int cellCoordinates)
    {
        Synthetizer.RegisterSegment(cellCoordinates, snapped.GetSegment());
    }

    protected override void OnUnsnap(Module unsnapped, int index)
    {
        Synthetizer.RemoveSegment(FromIndex(index));
    }
}
