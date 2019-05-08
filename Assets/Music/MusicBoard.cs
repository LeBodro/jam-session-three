using UnityEngine;

public class MusicBoard : SnappingGrid
{
    protected override void OnSnap(Module snapped, Vector3Int cellCoordinates)
    {
        if (snapped is VirtualClickerModule)
        {
            Synthetizer.RegisterSegment(cellCoordinates, (snapped as VirtualClickerModule).GetSegment());
            (snapped as VirtualClickerModule).OnMouseClick += VCMMouseClick;
        }
        if (snapped is ButtonModule)
        {
            (snapped as ButtonModule).OnMouseClick += ButtonModuleMouseClick;
        }
    }

    private void VCMMouseClick(VirtualClickerModule clicked)
    {
        int index = -1;
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] == clicked)
            {
                index = i;
                break;
            }
        if (index != -1)
        {
            Synthetizer.CycleSegment(FromIndex(index));
        }
    }

    private void ButtonModuleMouseClick(ButtonModule clicked)
    {
        int index = -1;
        for (int i = 0; i < cells.Length; i++)
            if (cells[i] == clicked)
            {
                index = i;
                break;
            }
        if (index != -1)
        {
            Synthetizer.CycleNote(FromIndex(index));
        }
    }

    protected override void OnUnsnap(Module unsnapped, int index)
    {
        if (unsnapped is VirtualClickerModule)
        {
            Synthetizer.RemoveSegment(FromIndex(index));
            (unsnapped as VirtualClickerModule).OnMouseClick -= VCMMouseClick;
        }
        if (unsnapped is ButtonModule)
        {
            (unsnapped as ButtonModule).OnMouseClick -= ButtonModuleMouseClick;
        }
    }
}
