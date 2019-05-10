using UnityEngine;

public class MusicBoard : SnappingGrid
{
    protected override void OnSnap(Module snapped, Vector3Int cellCoordinates)
    {
        if (snapped is VirtualClickerModule)
        {
            Synthetizer.RegisterSegment(cellCoordinates, (snapped as VirtualClickerModule).GetSegment());
            (snapped as VirtualClickerModule).OnMouseClick += CycleSegment;
        }
        if (snapped is ButtonModule)
        {
            (snapped as ButtonModule).OnMouseClick += CycleNote;
        }
    }

    private void CycleSegment(VirtualClickerModule clicked)
    {
        int index = GetModuleId(clicked);
        if (index != -1)
        {
            Synthetizer.CycleSegment(FromIndex(index));
        }
    }

    private void CycleNote(ButtonModule clicked)
    {
        int index = GetModuleId(clicked);
        if (index != -1)
        {
            Synthetizer.CycleNote(FromIndex(index));
        }
    }

    int GetModuleId(Module target)
    {
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] == target)
                return i;
        return -1;
    }

    protected override void OnUnsnap(Module unsnapped, int index)
    {
        if (unsnapped is VirtualClickerModule)
        {
            Synthetizer.RemoveSegment(FromIndex(index));
            (unsnapped as VirtualClickerModule).OnMouseClick -= CycleSegment;
        }
        if (unsnapped is ButtonModule)
        {
            (unsnapped as ButtonModule).OnMouseClick -= CycleNote;
        }
    }
}
