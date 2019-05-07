using UnityEngine;

public class MusicBoard : SnappingGrid
{
    protected override void OnSnap(Module snapped, Vector3Int cellCoordinates)
    {
        if (snapped is VirtualClickerModule)
        {
            Synthetizer.RegisterSegment(cellCoordinates, (snapped as VirtualClickerModule).GetSegment());
        }
    }

    protected override void OnUnsnap(Module unsnapped, int index)
    {
        if (unsnapped is VirtualClickerModule)
        {
            Synthetizer.RemoveSegment(FromIndex(index));
        }
    }
}
