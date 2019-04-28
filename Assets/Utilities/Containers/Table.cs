using System;
using System.Collections.Generic;

public class Table<T>
{
    public readonly int Width;
    public readonly int Height;
    public List<List<T>> Content { get; private set; }

    public Table(int width, int height, T defaultValue = default(T))
    {
        Width = width;
        Height = height;
        Content = new List<List<T>>(width);
        for (int x = 0; x < Width; x++)
        {
            Content.Add(new List<T>(Height));
            for (int y = 0; y < Height; y++)
                Content[x].Add(defaultValue);
        }
    }

    public Table(int width, int height, Func<int, int, T> defaultGetter)
    {
        Width = width;
        Height = height;
        Content = new List<List<T>>(width);
        for (int x = 0; x < Width; x++)
        {
            Content.Add(new List<T>(Height));
            for (int y = 0; y < Height; y++)
                Content[x].Add(defaultGetter(x, y));
        }
    }

    public T Get(int x, int y) { return Content[x][y]; }
    public void Set(int x, int y, T value) { Content[x][y] = value; }
}