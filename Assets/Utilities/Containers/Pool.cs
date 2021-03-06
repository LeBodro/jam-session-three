﻿using System;
using System.Collections.Generic;

public class Pool<T> where T : Poolable<T>
{
    readonly Stack<T> _objects;
    readonly Func<T> _objectGenerator;

    public Pool(Func<T> objectGenerator)
    {
        if (objectGenerator == null)
            throw new ArgumentNullException("objectGenerator");
        _objects = new Stack<T>();
        _objectGenerator = objectGenerator;
    }

    public T Get()
    {
        if (_objects.Count > 0)
        {
            return _objects.Pop().DePool(this);
        }
        return _objectGenerator().DePool(this);
    }

    public void Put(T item)
    {
        _objects.Push(item);
    }
}