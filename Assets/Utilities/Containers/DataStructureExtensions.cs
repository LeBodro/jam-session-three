using System.Collections.Generic;

public static class DataStructureExtensions
{
    public static T Cycle<T>(this List<T> self, int index)
    {
        int count = self.Count;
        index = (index % count + count) % count; // Supports looping and negative values.
        return self[index];
    }
}
