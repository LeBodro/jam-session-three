public class MusicBoard : SnappingGrid
{
    protected override void OnSnap(Module snapped, int index)
    {
        Synthetizer.RegisterSegment(index, snapped.GetSegment());
    }

    protected override void OnUnsnap(Module unsnapped, int index)
    {
        Synthetizer.RemoveSegment(index);
    }
}
